using System;
using System.Collections.Generic;
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
using OxyPlot;
namespace Ряд_Фурье
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate double Func(double x);

        public struct Koef
        {
            public double an;
            public double bn;
        }

        public class Harmonic
        {
            public int harmonicNumber { get; set; }
            public string content { get; set; }
            public Harmonic(int n, string c)
            {
                this.harmonicNumber = n;
                this.content = c;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        double PI = 3.1415;
        List<DataPoint> list = new List<DataPoint>();
        List<DataPoint> list2 = new List<DataPoint>();

        private double Rect(double a, double b, int n, Func func)
        {
            double h = (b - a) / n;
            double res = 0;
            for (double i = a; i < b; i += h)
            {
                res += h * func(i + (h / 2));
            }
            return res;
        }


        double func_orig(double x)
        {
            return x * x * x;
        }

        double func_Cos(double x)
        {
            return func_orig(x) * Math.Cos((PI * n * x) / length);
        }

        double func_Sin(double x)
        {
            return func_orig(x) * Math.Sin((PI * n * x) / length);
        }

        int n = 1;
        double length;

        private void Btn_Draw_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            list2.Clear();
            ListViewHarmonics.Items.Clear();
            List<Koef> koefs = new List<Koef>();
            int harmonicsN = int.Parse(TextBox_Harmonics.Text);
            int divideN = int.Parse(TextBox_divideNumber.Text);
            double xmin;
            double xmax;
            double drawmin;
            double drawmax;
            if (!double.TryParse(TextBox_calculateBegin.Text, out xmin) || !double.TryParse(TextBox_calculateEnd.Text, out xmax) || xmin >= xmax)
            {
                MessageBox.Show("Неверные пределы разложения");
                return;
            }
            if (!double.TryParse(TextBlock_drawBegin.Text, out drawmin) || !double.TryParse(TextBlock_drawEnd.Text, out drawmax) || drawmin >= drawmax)
            {
                MessageBox.Show("Неверные пределы отрисовки");
                return;
            }
            length = (xmax - xmin) / 2;
            koefs = calculateKoeffs(harmonicsN, xmin, xmax, divideN);
            CalculateDots(drawmin, drawmax, xmin, xmax, divideN, koefs);
            MainViewModel.draw(list, list2);
        }

        public List<Koef> calculateKoeffs(int harmonicsN, double min, double max, int divideN)
        {
            List<Koef> koefs = new List<Koef>();
            double length = (max - min) / 2;
            for (n = 1; n <= harmonicsN; n++)
            {
                Koef k = new Koef();
                k.an = Rect(min, max, divideN, func_Cos) / length;
                k.bn = Rect(min, max, divideN, func_Sin) / length;
                koefs.Add(k);
            }
            return koefs;
        }

        public void CalculateDots(double drawmin, double drawmax, double xmin, double xmax, int divideN, List<Koef> koefs)
        {
            double length = (xmax - xmin) / 2.0;
            double a0 = Rect(xmin, xmax, divideN, func_orig) / length;
            double sum;
            for (double x = drawmin; x <= drawmax; x += 0.1)
            {
                sum = a0 / 2;
                if (x == xmin)
                {
                    Console.WriteLine("f(x) = " + sum);
                }
                for (n = 1; n <= koefs.Count; n++)
                {
                    sum += koefs[n - 1].an * Math.Cos((PI * n * x) / length) + koefs[n - 1].bn * Math.Sin((PI * n * x) / length);
                    if (x == xmin)
                    {
                        string harmonicInfo = koefs[n - 1].an + " * cos(" + n * PI / length + "x) + \t" + koefs[n - 1].bn + " * sin(" + n * PI / length + "x)";
                        ListViewHarmonics.Items.Add(new Harmonic(n, harmonicInfo));
                        Console.WriteLine(n + ":\t" + koefs[n - 1].an + " * cos(" + n * PI / length + "x) + \t" + koefs[n - 1].bn + " * sin(" + n * PI / length + "x)");
                    }
                }
                list.Add(new DataPoint(x, sum));
                list2.Add(new DataPoint(x, func_orig(x)));
                ListViewHarmonics.Items.Refresh();
            }
        }
    }
}
