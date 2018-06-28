using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private async void RecordButton(object sender, RoutedEventArgs e)
        {
            
            var textRecognizer = new TextRecognizer();
            await textRecognizer.RecoFromMicrophoneAsync("ru-RU");
            var recognitionResult = textRecognizer.Result;

            if (recognitionResult == null)
            {
                OutputCalculation.Text = "Не удалось распознать речь.";
                return;
            }

            var converter = new Converter();
            var equation = converter.ConvertTextToEquation(recognitionResult);

            var result = EquationParser.Steps(equation);
            
            if (result != null)
            {
                var res = new StringBuilder();
                foreach (var operation in result)
                {
                    res.AppendLine(operation);
                }
                OutputCalculation.Text = res.ToString();
            }
            else
            {
                OutputCalculation.Text = "Математическое выражение составлено неправильно";
            }
            
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
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
    }
}
