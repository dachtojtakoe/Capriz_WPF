using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Windows.Forms.Integration;
using System.Printing;
using System.Runtime.Serialization.Formatters;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using UserControl = System.Windows.Controls.UserControl;
using Capriz_WPF.Data;
using System.Windows.Markup;
using PrintDialog = System.Windows.Controls.PrintDialog;
using System.Drawing.Imaging;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomChart : UserControl
    {
        LineSeries series = new OxyPlot.Series.LineSeries();

        public int PointsCount { get; set; }
        public static PlotModel plotModel { get; set; }


        private double _initialXMin;
        private double _initialXMax;
        private double _initialYMin;
        private double _initialYMax;

        int cursorMode = 3;

        DataPoint LastClickedPoint = new DataPoint(); 

        DataTable dt = null;
        public DataTable Dt
        {
            set
            {
                dt = value;
            }
        }

        public CustomChart()
        {
            InitializeComponent();

            DataContext = this;

            plotModel = new PlotModel { Title = "Data Chart" };
            PlotViewWinForms.PlotView.Model = plotModel;
            PlotViewWinForms.PlotView.Model.Background = OxyColor.FromRgb(13, 52, 93);
            ChangeCursor();

            InitializePlotModel();
        }

        private void InitializePlotModel()
        {
            var valueAxis = new LinearAxis
            {
                TextColor = OxyColors.AliceBlue,
                AxislineColor = OxyColors.AliceBlue,
                AxislineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                MajorGridlineThickness = 0.5,
                MajorGridlineStyle = LineStyle.Dash,
                FontWeight = OxyPlot.FontWeights.Bold,

            };  
            plotModel.Axes.Add(valueAxis);

            var dateTimeAxis = new DateTimeAxis
            {
                StringFormat = "dd-MM-yyyy\r\n  HH:mm:ss",
                TextColor = OxyColors.AliceBlue,
                AxislineColor = OxyColors.AliceBlue,
                AxislineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                MajorGridlineThickness = 0.5,
                MajorGridlineStyle = LineStyle.Dash,
                FontWeight = OxyPlot.FontWeights.Bold,
                IntervalLength = 75,
                

            };

            plotModel.Axes.Add(dateTimeAxis);

            plotModel.TextColor = OxyColors.AliceBlue;

            series = new OxyPlot.Series.LineSeries
            {
                //Title = chParam,
                MarkerType = MarkerType.Circle,
                MarkerStroke = OxyColors.Black,
                MarkerFill = OxyColors.AliceBlue,
                Color = OxyColors.Orange,
                StrokeThickness = 2,
                MarkerSize = 4,
                
            };

            series.MouseDown += Series_MouseDown;
            series.Decimator = Decimator.Decimate;
        }
        private bool FilterData(DateTime date, DateTime[] d)
        {
            return (DateTime.Compare(date, d[0]) >= 0)
                && (DateTime.Compare(date, d[1]) <= 0);
        }

        public int GetCountPoints()
        {
            return plotModel.Series.Count;
        }

        public void AllDtPointsToChart(string chParam)
        {
            //ClearPlotData(PlotViewWinForms.PlotView.Model);
            plotModel.Annotations.Clear();

            if (dt.Rows.Count > 0)
            {
                //var valueAxis = new LinearAxis
                //{
                //    TextColor = OxyColors.AliceBlue,
                //    AxislineColor = OxyColors.AliceBlue,
                //    AxislineStyle = LineStyle.Solid,

                //    MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                //    MajorGridlineThickness = 0.5,
                //    MajorGridlineStyle = LineStyle.Dash,

                //};
                //PlotModel.Axes.Add(valueAxis);
                //// Добавляем ось X с типом DateTimeAxis
                //var dateTimeAxis = new DateTimeAxis
                //{
                //    StringFormat = "dd-MM-yyyy\r\n  HH:mm:ss",
                //    TextColor = OxyColors.AliceBlue,
                //    AxislineColor = OxyColors.AliceBlue,
                //    AxislineStyle = LineStyle.Solid,
                //    MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                //    MajorGridlineThickness = 0.5,
                //    MajorGridlineStyle = LineStyle.Dash,
                //};
                //PlotModel.Axes.Add(dateTimeAxis);

                //PlotModel.TextColor = OxyColors.AliceBlue;
                // Добавляем серию данных
                //var series = new OxyPlot.Series.LineSeries
                //{
                //    Title = chParam,
                //    MarkerType = MarkerType.Circle,
                //    MarkerStroke = OxyColors.Black,
                //    MarkerFill = OxyColors.AliceBlue,
                //    Color = OxyColors.Orange,
                //    //StrokeThickness = 3,
                //    MarkerSize = 4,
                //};

                //var dateValuePairs = dt.AsEnumerable()
                //.Select(row => new
                //{
                //    DateTime = row.Field<DateTime>("DateTime"),
                //    Value = row[chParam]
                //})
                //.ToList();

                //PointsCount = dateValuePairs.Count;

                //foreach (var pair in dateValuePairs)
                //{
                //    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(pair.DateTime), Convert.ToDouble(pair.Value)));
                //}

                series.Points.Clear();
                foreach (var row in dt.AsEnumerable())
                {
                    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(row.Field<DateTime>("DateTime")), Convert.ToDouble(row[chParam])));
                }

                PointsCount = series.Points.Count;

                //series.MouseDown += Series_MouseDown;
                //series.Decimator = Decimator.Decimate;
                plotModel.Title = Data.ConvertData.GetNameColumn(chParam);

                plotModel.Series.Clear();
                plotModel.Series.Add(series);

                var xAxis = plotModel.Axes[0];
                var yAxis = plotModel.Axes[1];

                _initialXMin = xAxis.Minimum;
                _initialXMax = xAxis.Maximum;
                _initialYMin = yAxis.Minimum;
                _initialYMax = yAxis.Maximum;

                xAxis.Zoom(_initialXMin, _initialXMax);
                yAxis.Zoom(_initialYMin, _initialYMax);

                plotModel.InvalidatePlot(true);

            }
        }

        public void AllDtPointsToChartOLD(string chParam, string[] filterDates)
        {
            //ClearPlotData(PlotViewWinForms.PlotView.Model);

            if (dt == null) return;

            DateTime[] dates = new DateTime[2] { Convert.ToDateTime(filterDates[0]), Convert.ToDateTime(filterDates[1]) };
            if (dt.Rows.Count > 0)
            {
                DataTable newdt = Data.DataToTable.CopyDtChart(dt, chParam);

                var query = from myRow in newdt.AsEnumerable()
                            where FilterData(Convert.ToDateTime(string.Concat(myRow.Field<string>("Date") + " " + myRow.Field<string>("Time"))), dates) == true
                            select myRow;

                DataTable dtresult = new DataTable();
                if (query != null && query.AsDataView().Count > 0)
                    dtresult = query.CopyToDataTable();
                else
                    dtresult = newdt;
                query = null;

                var dateValuePairs = dtresult.AsEnumerable()
                   .Select(row => new
                   {
                       DateTime = Convert.ToDateTime(string.Concat(row.Field<string>("Date"), " ", row.Field<string>("Time"))),
                       Value = row.Field<double>(chParam)
                   })
                   .ToList();

                PointsCount = dateValuePairs.Count;

                var valueAxis = new LinearAxis
                {
                    TextColor = OxyColors.AliceBlue,
                    AxislineColor = OxyColors.AliceBlue,
                    AxislineStyle = LineStyle.Solid,

                    MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                    MajorGridlineThickness = 0.5,
                    MajorGridlineStyle = LineStyle.Dash,

                };
                plotModel.Axes.Add(valueAxis);
                // Добавляем ось X с типом DateTimeAxis
                var dateTimeAxis = new DateTimeAxis
                {
                    StringFormat = "dd-MM-yyyy\r\n  HH:mm:ss",
                    TextColor = OxyColors.AliceBlue,
                    AxislineColor = OxyColors.AliceBlue,
                    AxislineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(128, OxyColors.AliceBlue),
                    MajorGridlineThickness = 0.5,
                    MajorGridlineStyle = LineStyle.Dash,
                };
                plotModel.Axes.Add(dateTimeAxis);

                plotModel.TextColor = OxyColors.AliceBlue;
                // Добавляем серию данных
                var series = new OxyPlot.Series.LineSeries
                {
                    Title = chParam,
                    MarkerType = MarkerType.Circle,
                    MarkerStroke = OxyColors.Black,
                    MarkerFill = OxyColors.AliceBlue,
                    Color = OxyColors.Orange,
                    //StrokeThickness = 3,
                    MarkerSize = 4,
                };

                foreach (var pair in dateValuePairs)
                {
                    //if(!(pair.Value == -999))     !!!
                    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(pair.DateTime), pair.Value));
                }

                series.MouseDown += Series_MouseDown;
                series.Decimator = Decimator.Decimate;

                plotModel.Title = Data.ConvertData.GetNameColumn(chParam);

                plotModel.Series.Add(series);
                plotModel.InvalidatePlot(true);

                var xAxis = plotModel.Axes[0];
                var yAxis = plotModel.Axes[1];

                _initialXMin = xAxis.Minimum;
                _initialXMax = xAxis.Maximum;
                _initialYMin = yAxis.Minimum;
                _initialYMax = yAxis.Maximum;

            }
        }

        private void Series_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.HitTestResult.Element is OxyPlot.Series.LineSeries ls)
            {
                var clickedPoint = ls.GetNearestPoint(e.Position, false).DataPoint;
                if(LastClickedPoint.X != clickedPoint.X || LastClickedPoint.Y != clickedPoint.Y)
                {
                    UpdateCrosshair(clickedPoint);
                    LastClickedPoint = clickedPoint;
                }
                else
                {
                    plotModel.Annotations.Clear();
                    plotModel.InvalidatePlot(true);
                    LastClickedPoint = new DataPoint();
                }
            }
            //else
            //    PlotModel.Annotations.Clear();
        }

        private void UpdateCrosshair(DataPoint point)
        {
            var lineAnnotationH = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = point.Y,
                Color = OxyColors.Red,
                StrokeThickness = 1
            };
            var lineAnnotationV = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = point.X,
                Color = OxyColors.Red,
                StrokeThickness = 1
            };

            plotModel.Annotations.Clear();

            plotModel.Annotations.Add(lineAnnotationH);
            plotModel.Annotations.Add(lineAnnotationV);
            plotModel.InvalidatePlot(true);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            var xAxis = plotModel.Axes[0];
            var yAxis = plotModel.Axes[1];

            xAxis.ZoomAtCenter(1.1);
            yAxis.ZoomAtCenter(1.1);

            plotModel.InvalidatePlot(true);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            var xAxis = plotModel.Axes[0];
            var yAxis = plotModel.Axes[1];

            xAxis.ZoomAtCenter(1 / 1.1);
            yAxis.ZoomAtCenter(1 / 1.1);

            plotModel.InvalidatePlot(true);
        }

        private void PanLeft_Click(object sender, RoutedEventArgs e)
        {
            var xAxis = plotModel.Axes[1];
            xAxis.Pan(0.003 * Math.Abs(xAxis.ActualMaximum));
            plotModel.InvalidatePlot(true);
        }

        private void PanRight_Click(object sender, RoutedEventArgs e)
        {
            var xAxis = plotModel.Axes[1];
            xAxis.Pan(-0.003 * (xAxis.ActualMaximum));
            plotModel.InvalidatePlot(true);
        }

        private void PanTop_Click(object sender, RoutedEventArgs e)
        {
            var yAxis = plotModel.Axes[0];
            var range = yAxis.ActualMaximum - yAxis.ActualMinimum;

            var coef = range > 2000 ? 0.01 : range > 1000 ? 0.02 : range > 200 ? 0.1 : range > 150 ? 0.2 : range > 70 ? 0.3 : range > 18 ? 2 : range > 9 ? 1 : range > 3 ? 2 : range > 0.5 ? 20 : range > 0.3 ? 25 : range > 0.1 ? 30 : range > 0.01 ? 500 : 2500;
            yAxis.Pan(coef * range);

            plotModel.InvalidatePlot(true);
        }

        private void PanBottom_Click(object sender, RoutedEventArgs e)
        {
            var yAxis = plotModel.Axes[0];
            var range = yAxis.ActualMaximum - yAxis.ActualMinimum;

            var coef = range > 2000 ? 0.01 : range > 1000 ? 0.02 : range > 200 ? 0.1 : range > 150 ? 0.2 : range > 70 ? 0.3 : range > 18 ? 2 : range > 9 ? 1 : range > 3 ? 2 : range > 0.5 ? 20 : range > 0.3 ? 25 : range > 0.1 ? 30 : range > 0.01 ? 500 : 2500;

            yAxis.Pan(-coef * range);

            plotModel.InvalidatePlot(true);
        }

        private void PanCenter_Click(object sender, RoutedEventArgs e)
        {
            var xAxis = plotModel.Axes[0];
            var yAxis = plotModel.Axes[1];

            if (xAxis != null && yAxis != null)
            {
                xAxis.Zoom(_initialXMin, _initialXMax);
                yAxis.Zoom(_initialYMin, _initialYMax);

                // Обновляем график
                plotModel.InvalidatePlot(true);
            }
        }

        private void ChangeCursor_Click(object sender, RoutedEventArgs e)
        {
            ChangeCursor();
        }

        private void ChangeCursor()
        {
            var myController = new PlotController();
            PlotViewWinForms.PlotView.Controller = myController;
            if (cursorMode == 1)
            {
                myController.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.PanAt);
                CursorMode.BtnText = "↔";
                CursorMode.BtnFontSize = 24;
                cursorMode = 2;
            }
            else if (cursorMode == 2)
            {
                myController.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.ZoomRectangle);
                CursorMode.BtnText = "▯";
                CursorMode.BtnFontSize = 20;
                cursorMode = 3;
            }
            else if (cursorMode == 3)
            {
                myController.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.PointsOnlyTrack);
                CursorMode.BtnText = "XY";
                CursorMode.BtnFontSize = 16;

                cursorMode = 1;
            }
        }

        private void ClearPlotData(PlotModel plotModel)
        {
            if (plotModel != null)
            {
                plotModel.Series.Clear(); // Очищаем все серии данных
                plotModel.Axes.Clear(); // Очищаем все оси
                plotModel.Annotations.Clear();
            }
        }

        public void SaveChart(string ChartName)
        {
            var pngExporter = new OxyPlot.WindowsForms.PngExporter { Width = 1200, Height = 850 };
            OxyPlot.WindowsForms.ExporterExtensions.ExportToFile(pngExporter, plotModel, DataFile.ChartPath(ChartName, "png"));
            //Process.Start(DataFile.ChartPath(ChartName));

            using (var stream = File.Create(DataFile.ChartPath(ChartName, "svg")))
            {
                var exporter = new OxyPlot.WindowsForms.SvgExporter { Width = 1527, Height = 1080 };
                exporter.Export(plotModel, stream);
            }
            //Process.Start(DataFile.ChartPath(ChartName));
        }
    }
}

