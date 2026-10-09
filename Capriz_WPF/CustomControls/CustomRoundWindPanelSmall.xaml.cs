using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capriz_WPF.CustomControls
{
    public partial class CustomRoundWindPanelSmall : INotifyPropertyChanged //UserControl
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

        public static readonly DependencyProperty BrushColorProperty =
            DependencyProperty.Register(nameof(BrushColor), typeof(Brush), typeof(CustomRoundWindPanelSmall), new PropertyMetadata(Brushes.AliceBlue));

        public Brush BrushColor
        {
            get => (Brush)GetValue(BrushColorProperty); 
            set => SetValue(BrushColorProperty, value);
        }

        private Geometry _windDirectionPath;
        private Geometry _averageLine;

        private string _valueDir_mid = "Н.Д.";
        private string _valueDir_min = "Н.Д.";
        private string _valueDir_max = "Н.Д.";


        public void SetData(List<string> data)
        {
            ValueDir_mid = data[0];
            ValueDir_min = data[1];
            ValueDir_max = data[2];
        }

        public void ClearData()
        {
            WindDirectionPath = null;
            AverageLine = null;
        }

        public string ValueDir_mid
        {
            get => _valueDir_mid;
            set
            {
                _valueDir_mid = value;
                UpdateAverage();
                OnPropertyChanged(nameof(ValueDir_mid));
            }
        }

        public string ValueDir_min
        {
            get => _valueDir_min;
            set
            {
                _valueDir_min = value;
                //UpdateSector10();
                OnPropertyChanged(nameof(ValueDir_min));
            }
        }

        public string ValueDir_max
        {
            get => _valueDir_max;
            set
            {
                _valueDir_max = value;
                UpdateSector();
                OnPropertyChanged(nameof(ValueDir_max));
            }
        }

        public Geometry WindDirectionPath
        {
            get { return _windDirectionPath; }
            private set
            {
                _windDirectionPath = value;
                OnPropertyChanged(nameof(WindDirectionPath));
            }
        }

        public Geometry AverageLine
        {
            get { return _averageLine; }
            private set
            {
                _averageLine = value;
                OnPropertyChanged(nameof(AverageLine));
            }
        }

        public CustomRoundWindPanelSmall()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UpdateSector()
        {
            int begin_sec = -1, end_sec = -1;
            if (_valueDir_min.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_min.Trim(), out begin_sec);
                if (_valueDir_max.Trim() != "Н.Д.")
                {
                    Int32.TryParse(_valueDir_max.Trim(), out end_sec);
                    if ((begin_sec >= 0) && (end_sec >= 0))
                    {
                        bool isLargeArc = begin_sec > end_sec ? Math.Abs(360 - (begin_sec - end_sec)) > 180 : Math.Abs(begin_sec - end_sec) > 180;

                        var centerX = 245;
                        var centerY = 245;
                        var radius = 178;

                        var startPoint = new Point(centerX + radius * Math.Cos((begin_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((begin_sec - 90) * Math.PI / 180));
                        var endPoint = new Point(centerX + radius * Math.Cos((end_sec - 90) * Math.PI / 180), centerY + radius * Math.Sin((end_sec - 90) * Math.PI / 180));


                        var pathGeometry = new PathGeometry();
                        var pathFigure = new PathFigure { StartPoint = startPoint };
                        pathFigure.Segments.Add(new ArcSegment { Point = endPoint, Size = new Size(radius, radius), IsLargeArc = isLargeArc, SweepDirection = SweepDirection.Clockwise });
                        pathGeometry.Figures.Add(pathFigure);

                        WindDirectionPath = pathGeometry;
                    }
                }
            }
            else
                WindDirectionPath = null;
        }

        private void UpdateAverage()
        {
            //Среднее
            int result = -1;
            if (_valueDir_mid.Trim() != "Н.Д.")
            {
                Int32.TryParse(_valueDir_mid.Trim(), out result);
                if (result >= 0)
                {
                    var averageLineGeometry = new LineGeometry(new Point(245, 78.5), new Point(245, 57));
                    AverageAngle.Angle = result;
                    AverageLine = averageLineGeometry;
                }
            }
            else
                AverageLine = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            { }
        }
    }
}
