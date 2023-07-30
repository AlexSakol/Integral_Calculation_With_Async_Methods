//ЗАОКОННЫЙ КОД ДИАЛОГОВОГО ОКНА
using System;
using System.Windows;


namespace Lab_6
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Integral integral;
        public Window1()
        {
            InitializeComponent();
            integral = new Integral(0, Math.PI, 100);
            stackPanel1.DataContext = integral;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
