using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomCircularProgress : UserControl
    {
        public event EventHandler CancelRequested;

        private double _value;

        public double Value
        {
            get => _value;
            set
            {
                double newVal = Math.Max(0, Math.Min(100, value));
                int oldInt = (int)_value;
                int newInt = (int)newVal;
                if (newInt == oldInt && newVal < 100) return;

                _value = newVal;
                UpdateArc();
                PercentText.Text = $"{newInt}%";
            }
        }

        public CustomCircularProgress()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateArc();
            SizeChanged += (_, __) => UpdateArc();
        }

        private void UpdateArc()
        {
            if (ProgressPath == null) return;

            double w = ProgressPath.ActualWidth;
            double h = ProgressPath.ActualHeight;
            if (w <= 0 || h <= 0) return;

            double cx = w / 2;
            double cy = h / 2;
            double r = Math.Min(w, h) / 2 - ProgressPath.StrokeThickness / 2;

            if (r <= 0)
            {
                ProgressPath.Data = null;
                return;
            }

            if (_value <= 0)
            {
                ProgressPath.Data = null;
                return;
            }

            if (_value >= 100)
            {
                ProgressPath.Data = new EllipseGeometry(new Point(cx, cy), r, r);
                return;
            }

            double angle = _value * 3.6;
            double startAngle = -90;
            double endAngle = startAngle + angle;

            double startRad = startAngle * Math.PI / 180.0;
            double endRad = endAngle * Math.PI / 180.0;

            var startPt = new Point(cx + r * Math.Cos(startRad), cy + r * Math.Sin(startRad));
            var endPt = new Point(cx + r * Math.Cos(endRad), cy + r * Math.Sin(endRad));

            var figure = new PathFigure { StartPoint = startPt, IsClosed = false };
            figure.Segments.Add(new ArcSegment
            {
                Point = endPt,
                Size = new Size(r, r),
                IsLargeArc = angle > 180,
                SweepDirection = SweepDirection.Clockwise
            });

            var geo = new PathGeometry();
            geo.Figures.Add(figure);
            ProgressPath.Data = geo;
        }

        private void Root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}