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
        private readonly List<string> _allResults;
        private readonly List<string> _calculationHistory;
        private readonly GoogleRecognizer _recognizer;

        public MainWindow()
        {
            _isLastOperationWrong = false;
            _mayComeIn = true;
            _allResults = new List<string>();
            _calculationHistory = new List<string>();
            _recognizer = new GoogleRecognizer();
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
                await _recognizer.Start(_lang);
                if (_isLastOperationWrong)
                {
                    CalncelOperation(_isLastOperationWrong);
                    _isLastOperationWrong = false;
                }

                brush.ImageSource = new BitmapImage(new Uri("../../../Resources/micro2.png", UriKind.Relative));
                _сlicked = true;
                Record.SetValue(Button.BackgroundProperty, brush);
            }
            else
            {
                StopRecording();
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

        private async void StopRecording()
        {
            var res = await _recognizer.Stop();
            var con = new Converter(_lang);

            if (res.Length == 0)
            {
                ChangeScreenText(new List<string> { "Ничего не удалось распознать\n" });
                _isLastOperationWrong = true;
                return;
            }

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
            CalncelOperation(_isLastOperationWrong);
        }

        private void CalncelOperation(bool isError)
        {
            if (_allResults.Count == 0 && _calculationHistory.Count == 0) return;

            if (!isError) _allResults.RemoveAt(_allResults.Count - 1);
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
