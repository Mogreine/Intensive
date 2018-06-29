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
            rec = new GoogleRec("ru-RU");
            InitializeComponent();
        }
        
        private void RecordButton(object sender, RoutedEventArgs e)
        {
            if (!clicked)
            {
                rec.Start();
                clicked = true;
            }
            else
            {
                StopRecording();
                clicked = false;
            }
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {
            lastResult = null;
            OutputCalculation.Text = "";
        }

        private async void StopRecording()
        {
            var res = await rec.Stop();
            var con = new Converter();

            if (lastResult != null)
            {
                res = res.Insert(0, lastResult);
            }

            var equation = con.ConvertTextToEquation(res);
            var operations = EquationParser.Steps(equation);
            var steps = new StringBuilder();

            foreach (var op in operations)
            {
                steps.AppendLine(op);
            }

            lastResult = operations.Last().Split().Last() + " ";

            OutputCalculation.Text += steps;
        }
    }
}
