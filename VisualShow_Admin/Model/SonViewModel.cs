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
    public class SonViewModel
    {
        public PlotModel SonPlot { get; private set; }

        public SonViewModel(List<Son> sonList)
        {
            // Create the plot model
            SonPlot = new PlotModel { Title = "Sound Over Time" };

            // Create the series for sound
            var sonSeries = new LineSeries
            {
                Title = "Sound Level",
                Color = OxyColors.Green
            };

            // Add points for sound data
            foreach (var son in sonList)
            {
                try
                {
                    DateTime date = DateTime.Parse(son.date);
                    double xValue = DateTimeAxis.ToDouble(date);  // Convert DateTime to OxyPlot format
                    double soundLevel = Double.Parse(son.son, CultureInfo.InvariantCulture);
                    sonSeries.Points.Add(new DataPoint(xValue, soundLevel));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing sound data: {ex.Message}");
                }
            }

            // Add the LineSeries to the PlotModel
            SonPlot.Series.Add(sonSeries);

            // Configure X Axis (DateTime)
            SonPlot.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days,
                MinorIntervalType = DateTimeIntervalType.Hours
            });

            // Configure Y Axis (Sound Level)
            SonPlot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Sound Level (dB)"  // Assuming sound level is measured in decibels
            });
        }
    }
}