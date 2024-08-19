using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: LineGraph is a wrapper for Cartesian charts which live updates data.
 */

namespace Growers.io.Models
{
    /// <summary>
    /// LineGraph which displays new data live from Reading of type T.
    /// </summary>
    /// <typeparam name="T">Type of reading</typeparam>
    public class LineGraph<T>
    {
        /// <summary>
        /// Chart for the view
        /// </summary>
        public CartesianChart Chart { get; private set; }
        /// <summary>
        /// Reference to data to display & listen to updates on
        /// </summary>
        public ObservableCollection<Reading<T>> Data { get; private set; }
        /// <summary>
        /// List of points on graph
        /// </summary>
        public List<DateTimePoint> Points { get; private set; }

        private LineSeries<DateTimePoint> series;
        private Func<Reading<T>, (DateTime, double)> dataToPoint;

        public event EventHandler DataAdded;
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Creates instance of live updating chart.
        /// </summary>
        /// <param name="data">Data to display and update on</param>
        /// <param name="chart">Chart to update Series on</param>
        /// <param name="dataToPoint">Function to convert Reading to a point</param>
        public LineGraph(ObservableCollection<Reading<T>> data, CartesianChart chart, Func<Reading<T>, (DateTime, double)> dataToPoint) 
        { 
            Chart = chart;
            Data = data;
            Points = new();
            this.dataToPoint = dataToPoint;

            series = new LineSeries<DateTimePoint>
            {
                Name = "",
                Values = Points,
                Stroke = new SolidColorPaint(SKColors.BlueViolet) { StrokeThickness = 2 },
                GeometrySize = 0,
                GeometryStroke = null,
                LineSmoothness = 0.65,
            };

            chart.Series = new LineSeries<DateTimePoint>[]
            {
                series
            };
            chart.AutoUpdateEnabled = true;

            LastUpdated = DateTime.Now;

            UpdateSeriesValues(data);
            // Listen for updates and add any new data to chart.
            Data.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                UpdateSeriesValues(e.NewItems.OfType<Reading<T>>().ToList());
                LastUpdated = DateTime.Now;
                OnDataAdded(EventArgs.Empty);        
            };
        }

        // Adds new data to graph. Also limits x-axis to 25 values.
        private void UpdateSeriesValues(IList<Reading<T>> data)
        {
            foreach(var dataPoint in data)
            {
                (var time, var value) = dataToPoint(dataPoint);
                Points.Add(new DateTimePoint(time, value));

                // The x-axis uses the DateTime::Ticks to determine where the value should be displayed,
                // we set the MinLimit based on the 25th newest value.
                Chart.XAxes = new Axis[] { new DateTimeAxis(TimeSpan.FromMinutes(0.5), d => d.ToString("HH:mm:ss")) { MinLimit = Data[Math.Max(Data.Count - 25, 0)].TimeStamp.Ticks } };
            }
        }

        protected virtual void OnDataAdded(EventArgs e)
        {
            DataAdded?.Invoke(this, e);
        }
    }
}
