using Growers.io.Models;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: Static class for creating charts.
 */

namespace Growers.io.DataRepos
{
    public static class ChartsRepo
    {
        /// <summary>
        /// Creates barebones Cartesian chart
        /// </summary>
        /// <param name="chartTitle">Chart title</param>
        /// <param name="yMin">Y-axis min</param>
        /// <param name="yMax">Y-axis max</param>
        /// <param name="isBool">Is bool value?</param>
        /// <param name="minlbl">Boolean min label</param>
        /// <param name="maxlbl">Boolean max label</param>
        /// <returns>The configured CartesianChart</returns>
        public static CartesianChart GetChart(
            string chartTitle,
            double yMin,
            double yMax,
            bool isBool = false, 
            string minlbl = "", 
            string maxlbl = "")
        {
            CartesianChart chart = new();

            if (isBool)
            {
                chart.YAxes = new List<Axis> { new Axis { Labels = new[] { minlbl, maxlbl } } };
            }
            else
            {
                chart.YAxes = new Axis[] { new Axis { MinLimit = yMin, MaxLimit = yMax } };
            }

            chart.XAxes = new Axis[] { new DateTimeAxis(TimeSpan.FromSeconds(1), d => d.ToString("MM:d:H:m")) };
            chart.Title = new LabelVisual()
            {
                Text = chartTitle,
                TextSize = 18,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            return chart;
        }
    }
}
