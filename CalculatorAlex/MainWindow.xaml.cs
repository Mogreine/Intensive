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
        GoogleRec rec;
        public MainWindow()
        {
            InitializeComponent();
            rec = new GoogleRec("ru-RU");
        }
        
        private void RecordButton(object sender, RoutedEventArgs e)
        {
            rec.Start();
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            OutputSpeech.Text = "";
            OutputCalculation.Text = "";
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            var res = await rec.Stop();
            var con = new Converter();
            var equation = con.ConvertTextToEquation(res);
            var operations = EquationParser.Steps(equation);
            var steps = new StringBuilder();

            foreach (var op in operations)
            {
                steps.AppendLine(op);
            }

            OutputCalculation.Text += steps;
=======
            string ans = await rec.Stop();
            var converter = new Converter();
            ans = converter.ConvertTextToEquation(ans);
            OutputCalculation.Text = ans;
>>>>>>> feature/TASK_17-Andrew
        }
    }
}
