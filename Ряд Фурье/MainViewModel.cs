using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
namespace Ряд_Фурье
{
    class MainViewModel
    {
        static public PlotModel MyModel { get; private set; }

        public MainViewModel()
        {
            MyModel = new PlotModel { Title = "Graph", TextColor = OxyColors.Black };
        }
        public static void draw(List<DataPoint> listFourier, List<DataPoint> listOriginal)
        {
            MyModel.Series.Clear();
            LineSeries line1 = new LineSeries
            {
                Color = OxyColors.Blue
            };
            LineSeries line2 = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColors.Red
            };
            line1.Points.AddRange(listFourier);
            line2.Points.AddRange(listOriginal);
            MyModel.Series.Add(line1);
            MyModel.Series.Add(line2);
            MyModel.InvalidatePlot(true);
        }
    }
}
