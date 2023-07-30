//КЛАСС ИНТЕГРАЛ
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Lab_6
{
    public class Integral: IDataErrorInfo
    {
        double a, b;
        int n;
        public Func<double, double> f = (x)=>Math.Cos(2*x);

        public Integral(double a, double b, int n)
        {
            A = a;
            B = b;
            N = n;
        }

        public Integral()
        {
        }

        public double A { get => a; set => a = value; }
        public double B { get => b; set => b = value; }
        public int N { get => n; set => n = value; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName] 
        {
            get 
            {
                string error = "";
                switch (columnName)
                {
                    case "A":
                        if (A < 0 || A > Math.PI) error = "Начало диапазона должно быть [0; Pi]";
                        break;
                    case "B":
                        if (B < 0 || B > Math.PI) error = "Конец диапазона должен быть [0; Pi]";
                        break;
                    case "N":
                        if (N < 100) error = "Количество точек разбиения не должно быть меньше 100";
                        break;
                }
                return error;
            }
        }
        public async IAsyncEnumerable<(double, double, double)> GetDoublesAsync()
        {
            double h = (B - A) / N;
            double S = 0;
            for (int i = 0; i <= n; i++)
            {
                double x = A + h * i;
                S += f(x) * h;
                await Task.Delay(100);
                yield return (x, S, (double)i / N);
            }
        }

        public override string? ToString() => $"A = {A} B = {B} N = {N}";
    }
}
