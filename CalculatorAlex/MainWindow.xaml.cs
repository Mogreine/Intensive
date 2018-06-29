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
        private GoogleRec rec;
        private bool clicked;
        private string lastResult;

        public MainWindow()
        {
            rec = new GoogleRec();
            InitializeComponent();
        }
        
        private async void RecordButton(object sender, RoutedEventArgs e)
        {
            var brush = new ImageBrush();
            if (!clicked)
            {
                await rec.Start("ru-RU");
                brush.ImageSource = new BitmapImage(new Uri("../../../Resources/micro2.png", UriKind.Relative));
                clicked = true;
            }
            else
            {
                StopRecording();
                brush.ImageSource = new BitmapImage(new Uri("../../../Resources/micro.png", UriKind.Relative));
                clicked = false;
            }
            Record.Background = brush;
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            lastResult = null;
            OutputCalculation.Text = "";
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

        private async void StopRecording()
        {
            var res = await rec.Stop();
            var con = new Converter("ru-RU");

            if (lastResult != null)
            {
                if (Char.IsDigit(res[0]))
                    res = res.Insert(0, lastResult + " + ");
                else
                {
                    if (res.Length >= 2 && res[0] == '-' && Char.IsDigit(res[1]))
                        res = res.Insert(1, " ");
                    res = res.Insert(0, lastResult + " ");
                }
            }

            var equation = con.ConvertTextToEquation(res);
            var operations = EquationParser.Steps(equation);
            var steps = new StringBuilder();

            foreach (var op in operations)
            {
                steps.AppendLine(op);
            }

            lastResult = EquationParser.LastValue.ToString(DoubleParser.EngInfo);

            OutputCalculation.Text += steps;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
