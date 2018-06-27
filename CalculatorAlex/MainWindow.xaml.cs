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
            var recognitionResult = textRecognizer.result;

            var converter = new Converter();
            var equation = converter.ConvertTextToEquation(recognitionResult);

            var result = EquationParser.Parse(equation);

            if (result.HasValue)
            {

                OutputSpeech.Text = result.Value.ToString();
                OutputCalculation.Text = result.Value.ToString();

            }
            else
            {
                OutputCalculation.Text = "Математическое выражение составлено неправильно";
            }
            
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            OutputSpeech.Text = "";
            OutputCalculation.Text = "";
        }
    }
}
