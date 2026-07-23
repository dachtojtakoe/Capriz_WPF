using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomRoundWindPanel : INotifyPropertyChanged //UserControl
    {
        public static readonly DependencyProperty IsSplitAnglesProperty =
            DependencyProperty.Register("IsSplitAngles", typeof(bool), typeof(CustomToolTip), new PropertyMetadata(false));

        public bool IsSplitAngles
        {
            get { return (bool)GetValue(IsSplitAnglesProperty); }
            set
            {
                SetValue(IsSplitAnglesProperty, value);     
                if (value)
                {
                    TypicalAngles.Visibility = Visibility.Collapsed;
                    SplitAngles.Visibility = Visibility.Visible;
                }
            }
        }

        private Geometry _windDirectionPath2;
        private Geometry _windDirectionPath10;
        private Geometry _averageLine2;
        private Geometry _averageLine10;

        private string _valueCurs = "Н.Д.";
        private string _valueSpeed = "Н.Д.";
        private string _valueDir = "Н.Д.";

        private string _valueDir_2mid = "Н.Д.";
        private string _valueDir_2min = "Н.Д.";
        private string _valueDir_2max = "Н.Д.";
        private string _valueDir_10mid = "Н.Д.";
        private string _valueDir_10min = "Н.Д.";
        private string _valueDir_10max = "Н.Д.";

        public string ValueSpeed
        {
            get { return _valueSpeed; }
            set
            {
                if (_valueSpeed != value)
                {
                    if (value != "Н.Д.")
                        _valueSpeed = value + " м/с";
                    else
                        _valueSpeed = value;
                    OnPropertyChanged(nameof(ValueSpeed));
                }
            }
        }
        public string ValueDir
        {
            get { return _valueDir; }
            set
            {
                if (_valueDir != value)
                {
                    if (value != "Н.Д.")
                    {
                        if (IsSplitAngles)
                        {
                            if (Convert.ToInt32(value) > 180)
                            {
                                value = (360 - Convert.ToInt32(value)).ToString();
                                _valueDir = value + "° ЛБ";
                            }
                            else
                            {
                                _valueDir = value + "° ПБ";
                            }
                        }
                        else
                            _valueDir = value + "°";

                    }
                    else
                        _valueDir = value;
                    OnPropertyChanged(nameof(ValueDir));
                }
            }
        }

        public string ValueCurs
        {
            get => _valueCurs;
            set
            {
                _valueCurs = value;
                UpdateShipCurs();
                OnPropertyChanged(nameof(ValueCurs));
            }
        }

        public string ValueDir_2mid
        {
            get => _valueDir_2mid;
            set
            {
                _valueDir_2mid = value;
                UpdateAverage2();
                OnPropertyChanged(nameof(ValueDir_2mid));
            }
        }

        public string ValueDir_2min
        {
            get => _valueDir_2min;
            set
            {
                _valueDir_2min = value;
                //UpdateSector2();
                OnPropertyChanged(nameof(ValueDir_2min));
            }
        }

        public string ValueDir_2max
        {
            get => _valueDir_2max;
            set
            {
                _valueDir_2max = value;
                UpdateSector2();
                OnPropertyChanged(nameof(ValueDir_2max));
            }
        }

        public string ValueDir_10mid
        {
            get => _valueDir_10mid;
            set
            {
                _valueDir_10mid = value;
                UpdateAverage10();
                OnPropertyChanged(nameof(ValueDir_10mid));
            }
        }

        public string ValueDir_10min
        {
            get => _valueDir_10min;
            set
            {
                _valueDir_10min = value;
                //UpdateSector10();
                OnPropertyChanged(nameof(ValueDir_10min));
            }
        }

        public string ValueDir_10max
        {
            get => _valueDir_10max;
            set
            {
                _valueDir_10max = value;
                UpdateSector10();
                OnPropertyChanged(nameof(ValueDir_10max));
            }
        }

        public Geometry WindDirectionPath2
        {
            get { return _windDirectionPath2; }
            private set
            {
                _windDirectionPath2 = value;
                OnPropertyChanged(nameof(WindDirectionPath2));
            }
        }

        public Geometry WindDirectionPath10
        {
            get { return _windDirectionPath10; }
            private set
            {
                _windDirectionPath10 = value;
                OnPropertyChanged(nameof(WindDirectionPath10));
            }
        }

        public Geometry AverageLine2
        {
            get { return _averageLine2; }
            private set
            {
                _averageLine2 = value;
                OnPropertyChanged(nameof(AverageLine2));
            }
        }

        public Geometry AverageLine10
        {
            get { return _averageLine10; }
            private set
            {
                _averageLine10 = value;
                OnPropertyChanged(nameof(AverageLine10));
            }
        }

        public CustomRoundWindPanel()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UpdateSector2()
        {
            int begin_sec = -1, end_sec = -1;
            if (_valueDir_2min.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_2min.Trim(), out begin_sec);
                if (_valueDir_2max.Trim() != "Н.Д.")
                {
                    Int32.TryParse(_valueDir_2max.Trim(), out end_sec);
                    if ((begin_sec >= 0) && (end_sec >= 0))
                    {
                        bool isLargeArc = begin_sec > end_sec ? Math.Abs(360 - (begin_sec - end_sec)) > 180 : Math.Abs(begin_sec - end_sec) > 180;

                        var centerX = 184.5;
                        var centerY = 184.5;
                        var radius = 121;

                        var startPoint = new Point(centerX + radius * Math.Cos((begin_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((begin_sec - 90) * Math.PI / 180));
                        var endPoint = new Point(centerX + radius * Math.Cos((end_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((end_sec - 90) * Math.PI / 180));


                        var pathGeometry = new PathGeometry();
                        var pathFigure = new PathFigure { StartPoint = startPoint };
                        pathFigure.Segments.Add(new ArcSegment { Point = endPoint, Size = new Size(radius, radius), IsLargeArc = isLargeArc, SweepDirection = SweepDirection.Clockwise });
                        pathGeometry.Figures.Add(pathFigure);

                        WindDirectionPath2 = pathGeometry;
                    }
                }
            }
            else
                WindDirectionPath2 = null;
        }

        private void UpdateSector10()
        {
            int begin_sec = -1, end_sec = -1;
            if (_valueDir_10min.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_10min.Trim(), out begin_sec);
                if (_valueDir_10max.Trim() != "Н.Д.")
                {
                    Int32.TryParse(_valueDir_10max.Trim(), out end_sec);
                    if ((begin_sec >= 0) && (end_sec >= 0))
                    {
                        bool isLargeArc = begin_sec > end_sec ? Math.Abs(360 - (begin_sec - end_sec)) > 180 : Math.Abs(begin_sec - end_sec) > 180;

                        var centerX = 184.5;
                        var centerY = 184.5;
                        var radius = 136;

                        var startPoint = new Point(centerX + radius * Math.Cos((begin_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((begin_sec - 90) * Math.PI / 180));
                        var endPoint = new Point(centerX + radius * Math.Cos((end_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((end_sec - 90) * Math.PI / 180));


                        var pathGeometry = new PathGeometry();
                        var pathFigure = new PathFigure { StartPoint = startPoint };
                        pathFigure.Segments.Add(new ArcSegment { Point = endPoint, Size = new Size(radius, radius), IsLargeArc = isLargeArc, SweepDirection = SweepDirection.Clockwise });
                        pathGeometry.Figures.Add(pathFigure);

                        WindDirectionPath10 = pathGeometry;
                    }
                }
            }
            else
                WindDirectionPath10 = null;
        }

        private void UpdateAverage2()
        {
            //Среднее за 2 минуты
            int result = -1;
            if (_valueDir_2mid.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_2mid.Trim(), out result);
                if (result >= 0)
                {
                    var averageLineGeometry2 = new LineGeometry(new Point(184.5, 69), new Point(184.5, 57));
                    AverageAngle2.Angle = result;
                    AverageLine2 = averageLineGeometry2;
                }
            }
            else
                AverageLine2 = null;
        }

        private void UpdateAverage10()
        {
            //Среднее за 10 минут
            int result = -1;
            if (_valueDir_10mid.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_10mid.Trim(), out result);
                if (result >= 0)
                {
                    var averageLineGeometry10 = new LineGeometry(new Point(184.5, 55), new Point(184.5, 43));
                    AverageAngle10.Angle = result;
                    AverageLine10 = averageLineGeometry10;
                }
            }
            else
                AverageLine10 = null;
        }

        private void UpdateShipCurs()
        {
            int result = -1;
            Int32.TryParse(_valueCurs.Trim(), out result);
            if ((result > 0) || (result == 0))
            {
                ShipRotateTransform.Angle = result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

