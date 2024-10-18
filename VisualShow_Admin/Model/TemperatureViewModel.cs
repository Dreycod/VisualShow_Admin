using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;

namespace VisualShow_Admin.Model
{
    public class TemperatureViewModel
    {
        public PlotModel TemperaturePlot { get; private set; }

        public TemperatureViewModel(List<Temp_Hum> tempHumList, bool showMax)
        {
            // Create the plot model
            TemperaturePlot = new PlotModel { Title = "Temperature Over Time" };

            // Create the series for temperature
            var temperatureSeries = new LineSeries
            {
                Title = showMax ? "Max Temperature" : "Min Temperature",
                Color = showMax ? OxyColors.Red : OxyColors.Blue
            };

            // Add points from the Temp_Hum data
            foreach (var tempHum in tempHumList)
            {
                DateTime date = DateTime.Parse(tempHum.Date);
                double xValue = DateTimeAxis.ToDouble(date);  // Convert DateTime to OxyPlot format
                double temp = Double.Parse(tempHum.temperature, CultureInfo.InvariantCulture);
                temperatureSeries.Points.Add(new DataPoint(xValue,temp));
            }

            // Add the LineSeries to the PlotModel
            TemperaturePlot.Series.Add(temperatureSeries);

            // Configure X Axis (DateTime)
            TemperaturePlot.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Hours
            });

            // Configure Y Axis (Temperature)
            TemperaturePlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature (°C)"
            });
        }
    }
}
