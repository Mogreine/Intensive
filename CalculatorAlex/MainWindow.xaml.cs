using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorAlex
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _сlicked;
        private string _lang;
        private bool _isLastOperationWrong;
        private bool _mayComeIn;
        private bool _isErrorCheckingAlive;
        private Task _waitingErrorTask;
        private readonly List<string> _allResults;
        private readonly List<string> _calculationHistory;
        private readonly GoogleRecognizer _recognizer;
        private readonly AutoResetEvent _errorEvent;

        public MainWindow()
        {
            _isErrorCheckingAlive = false;
            _isLastOperationWrong = false;
            _mayComeIn = true;
            _allResults = new List<string>();
            _calculationHistory = new List<string>();
            _errorEvent = new AutoResetEvent(false);
            _recognizer = new GoogleRecognizer(_errorEvent);
            InitializeComponent();
        }

        private async void RecordButton(object sender, RoutedEventArgs e)
        {
            if (!_mayComeIn)
                return;
            _mayComeIn = false;
            var brush = new ImageBrush();
            if (!_сlicked)
            {
                try
                {
                    if (!_isErrorCheckingAlive)
                    {
                        _isErrorCheckingAlive = true;
                        _waitingErrorTask = Task.Run(() =>
                        {
                            _errorEvent.WaitOne();
                            _errorEvent.Reset();
                        });
                        WaitError();
                    }

                    await _recognizer.Start(_lang);
                }
                catch (Grpc.Core.RpcException)
                {
                    ChangeScreenText(new List<string> { "Ошибка соединения!" });
                    _isLastOperationWrong = true;
                    _mayComeIn = true;
                    return;
                }

                if (_isLastOperationWrong)
                {
                    CancelOperation(_isLastOperationWrong);
                    _isLastOperationWrong = false;
                }

                brush.ImageSource = new BitmapImage(new Uri("../../../Resources/micro2.png", UriKind.Relative));
                _сlicked = true;
                Record.SetValue(Button.BackgroundProperty, brush);
            }
            else
            {
                try
                {
                    await StopRecording();
                }
                catch (Grpc.Core.RpcException)
                {
                    ChangeScreenText(new List<string> { "Ошибка соединения!" });
                    _isLastOperationWrong = true;
                }

                brush.ImageSource = new BitmapImage(new Uri("../../../Resources/micro.png", UriKind.Relative));
                _сlicked = false;
                Record.ClearValue(Button.BackgroundProperty);
                Record.SetCurrentValue(Button.BackgroundProperty, brush);

            }
            _mayComeIn = true;
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            _allResults.Clear();
            _calculationHistory.Clear();
            _isLastOperationWrong = false;
            OutputCalculation.Text = "";
        }

        private async Task StopRecording()
        {
            var res = "";
            res = await _recognizer.Stop();

            var con = new Converter(_lang);

            if (res.Length == 0)
            {
                ChangeScreenText(new List<string> { "Ничего не удалось распознать!" });
                _isLastOperationWrong = true;
                return;
            }

            res = con.PreConvertation(res);

            if (_allResults.Count != 0)
            {
                if (Char.IsDigit(res[0]))
                    res = res.Insert(0, _allResults[_allResults.Count - 1] + " + ");
                else
                {
                    if (res.Length >= 2 && res[0] == '-' && Char.IsDigit(res[1]))
                        res = res.Insert(1, " ");
                    res = res.Insert(0, _allResults[_allResults.Count - 1] + " ");
                }
            }

            var equation = con.ConvertTextToEquation(res);
            var operations = EquationParser.Steps(equation);

            _isLastOperationWrong = !EquationParser.Success;

            ChangeScreenText(operations);
            _allResults.AddRange(EquationParser.AllValues);
        }

        private async void WaitError()
        {
            await _waitingErrorTask;
            RecordButton(Record, new RoutedEventArgs());
            _isErrorCheckingAlive = false;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CombBoxLang.SelectedIndex)
            {
                case 0:
                    _lang = Culture.Ru;
                    break;
                case 1:
                    _lang = Culture.Eng;
                    break;
                default:
                    _lang = Culture.Ru;
                    break;
            }
        }

        private void CancelLastOperation(object sender, RoutedEventArgs e)
        {
            CancelOperation(_isLastOperationWrong);
        }

        private void CancelOperation(bool isError)
        {
            if (_allResults.Count == 0 && _calculationHistory.Count == 0) return;

            if (!isError && _allResults.Count != 0) _allResults.RemoveAt(_allResults.Count - 1);
            _calculationHistory.RemoveAt(_calculationHistory.Count - 1);

            ChangeScreenText();
            _isLastOperationWrong = false;
        }

        private void ChangeScreenText()
        {
            var steps = new StringBuilder();

            foreach (var op in _calculationHistory)
            {
                steps.AppendLine(op);
            }

            OutputCalculation.Text = steps.ToString();
        }

        private void ChangeScreenText(List<string> lastOperations)
        {
            var steps = new StringBuilder();

            foreach (var op in lastOperations)
            {
                _calculationHistory.Add(op);
                steps.AppendLine(op);
            }
            OutputCalculation.Text += steps.ToString();
        }
        
        private void HideButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void FullButton(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MoveEvent(object sender, RoutedEventArgs e)
        {
            this.DragMove();
        }
    }
}
