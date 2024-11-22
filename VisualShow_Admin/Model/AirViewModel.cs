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
    public class AirViewModel
    {
        public PlotModel AirPlot { get; private set; }

        public AirViewModel(List<Air> airList)
        {
            // Create the plot model
            AirPlot = new PlotModel { Title = "Air Quality Over Time" };

            // Create the series for air quality
            var airSeries = new LineSeries
            {
                Title = "Air Quality",
                Color = OxyColors.Orange
            };

            // Add points for air data
            foreach (var air in airList)
            {
                try
                {
                    DateTime date = DateTime.Parse(air.date);
                    double xValue = DateTimeAxis.ToDouble(date);  // Convert DateTime to OxyPlot format
                    double airQuality = Double.Parse(air.air, CultureInfo.InvariantCulture);
                    airSeries.Points.Add(new DataPoint(xValue, airQuality));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing air quality data: {ex.Message}");
                }
            }

            // Add the LineSeries to the PlotModel
            AirPlot.Series.Add(airSeries);

            // Configure X Axis (DateTime)
            AirPlot.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Hours
            });

            // Configure Y Axis (Air Quality)
            AirPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Air Quality Index"  // Adjust title based on your air quality measurement
            });
        }
    }
}
