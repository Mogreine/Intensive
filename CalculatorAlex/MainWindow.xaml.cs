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
            OutputCalculation.Text = await rec.Stop();
        }
    }
}
