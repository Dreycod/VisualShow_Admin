using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;


namespace VisualShow_Admin.Model
{
    public class HumidityViewModel
    {
        public PlotModel HumidityPlot { get; private set; }

        public HumidityViewModel(List<Temp_Hum> tempHumList)
        {
            // Create the plot model
            HumidityPlot = new PlotModel { Title = "Humidity Over Time" };

            // Create the series for humidity
            var humiditySeries = new LineSeries
            {
                Title = "Humidity",
                Color = OxyColors.Blue
            };

            // Add points for humidity data
            foreach (var tempHum in tempHumList)
            {
                try
                {
                    DateTime date = DateTime.Parse(tempHum.Date);
                    double xValue = DateTimeAxis.ToDouble(date);  // Convert DateTime to OxyPlot format
                    double humidity = Double.Parse(tempHum.humidite, CultureInfo.InvariantCulture);
                    humiditySeries.Points.Add(new DataPoint(xValue, humidity));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing humidity: {ex.Message}");
                }
            }

            // Add the LineSeries to the PlotModel
            HumidityPlot.Series.Add(humiditySeries);

            // Configure X Axis (DateTime)
            HumidityPlot.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Hours
            });

            // Configure Y Axis (Humidity)
            HumidityPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Humidity (%)",
            });
        }
    }
}