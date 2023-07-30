// ЗАОКОННЫЙ КОД ГЛАВНОГО ОКНА
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Lab_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Integral integral;
        BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();
            worker = (BackgroundWorker)this.Resources["worker"];
        }

        private void buttonParams_Click(object sender, RoutedEventArgs e)
        {
            var window1 = new Window1();
            if (window1.ShowDialog() == false) return;
            integral = window1.integral;
            MessageBox.Show(integral.ToString());
        }

        private void buttonD_Click(object sender, RoutedEventArgs e)
        {
            if (integral == null) return;
            Thread thread = new(Calculate);
            thread.Start();            
        }

        private void buttonW_Click(object sender, RoutedEventArgs e)
        {
            if (integral == null) return;
            buttonD.IsEnabled = false;
            buttonW.IsEnabled = false;
            buttonA.IsEnabled = false;
            worker.RunWorkerAsync();
        }    

        private async void buttonA_Click(object sender, RoutedEventArgs e)
        {
            if (integral == null) return;
            buttonW.IsEnabled = false;
            buttonD.IsEnabled = false;
            buttonA.IsEnabled = false;
            ListBox.Items.Clear();            
            IAsyncEnumerable<(double, double, double)> data = integral.GetDoublesAsync();
            await foreach (var trio in data)
            {
                ListBox.Items.Add($"x = {trio.Item1:0.00} S={trio.Item2:0.00000}");
                pBar.Value = trio.Item3 * 100;
            }
            buttonW.IsEnabled = true;
            buttonD.IsEnabled = true;
            buttonA.IsEnabled = true;
        }
        private void Calculate()
        {
            if (integral == null) return;
            int n = integral.N;
            double h = (integral.B - integral.A) / n;
            var step = Math.Round((double)n / 100);
            double S = 0;
            for (int i = 0; i <= n; i++)
            {
                double x = integral.A + h * i;
                S += integral.f(x) * h;
                if (i % step == 0)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => pBar.Value = i / step));
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => ListBox.Items.Add($"x = {x:0.00} S={S:0.00000}")));
                Thread.Sleep(100);
            }            
        }    

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (integral == null) return;
            int n = integral.N;
            double h = (integral.B - integral.A) / n;
            var step = Math.Round((double)n / 100);
            double S = 0;
            for (int i = 0; i <= n; i++)
            {
                double x = integral.A + h * i;
                S += integral.f(x) * h;
                if (i % step == 0)
                {
                    if (worker != null && worker.WorkerReportsProgress)
                    {
                        worker.ReportProgress((int)(i / step));
                    }
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() => ListBox.Items.Add($"x = {x:0.00} S={S:0.00000}")));
                Thread.Sleep(100);
            }           
        }

        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            buttonD.IsEnabled = true;
            buttonW.IsEnabled = true;
            buttonA.IsEnabled = true;
        }
    }
}
