using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CircleChannel
{
    enum Direction
    {
        Left,
        Right
    }

    public partial class RoundChannel : UserControl, IProvider
    {
        private const double CenterX = 13;
        private const double CenterY = 13;
        private const double Radius = 13;
        private const double Standard = 90;

        private const float IncreaseRatio = 1.0f;
        private const float DecreaseRatio = -1.0f;

        private const int LeftBoundAngle = 270;
        private const int RightBoundAngle = -90;

        private readonly SolidColorBrush _penBrush = new SolidColorBrush(Color.FromArgb(125, 209, 209, 210));
                
        public Brush Brush { get; set; }
        public Pen Pen { get; set; }
        public Point Position { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }

        public double RotateAngle
        {
            get => (double) this.GetValue(RotateAngleProperty);
            set => this.SetValue(RotateAngleProperty, value);
        }

        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register("RotateAngle", typeof(double), typeof(RoundChannel), new PropertyMetadata(0.0));

        
        public string PanText
        {
            get => (string) this.GetValue(PanTextProperty);
            set => this.SetValue(PanTextProperty, value);
        }

        public static readonly DependencyProperty PanTextProperty =
            DependencyProperty.Register("PanText", typeof(string), typeof(RoundChannel), new PropertyMetadata(""));

        public double Pan
        {
            get => (double) this.GetValue(PanProperty);
            set => this.SetValue(PanProperty, value);
        }

        public static readonly DependencyProperty PanProperty =
            DependencyProperty.Register("Pan", typeof(double), typeof(RoundChannel), new PropertyMetadata(0.0, OnPanValueChanged));

        private static void OnPanValueChanged(DependencyObject dp,
            DependencyPropertyChangedEventArgs args)
        {
            var newPan = (double)args.NewValue;
            ((RoundChannel) dp).PanPropertyChanged(newPan);
        }
        
        private void PanPropertyChanged(double newPan)
        {
            if (_isLeftMouseDown)
            {
                return;
            }
            PanValueToAngle(newPan);
            UpdateView();
        }

        private void PanValueToAngle(double newPan)
        {
            const int allAngle = LeftBoundAngle - RightBoundAngle;
            var allLen = Maximum - Minimum;
            if (newPan > 0)
            {
                EndAngle = Standard;
                var angle = (int)((newPan - (Maximum + Minimum) / 2) / allLen * allAngle);
                StartAngle = Standard - angle;
            }
            else
            {
                StartAngle = Standard;
                var angle = (int)(((Maximum + Minimum) / 2 - newPan) / allLen * allAngle);
                EndAngle = Standard + angle;
            } 
        }
        
        public double Maximum { get; set; }
        public double Minimum { get; set; }

        public RoundChannel()
        {
            InitializeComponent();
            
            BgEllipse.MouseLeftButtonDown += BgEllipse_OnMouseLeftButtonDown;
            BgEllipse.MouseMove += BgEllipse_OnMouseMove;
            BgEllipse.MouseLeftButtonUp += BgEllipse_OnMouseLeftButtonUp;

            BgEllipse.Width = 2 * Radius;
            BgEllipse.Height = 2 * Radius;
            BgEllipse.SetValue(Canvas.LeftProperty, CenterX - Radius);
            BgEllipse.SetValue(Canvas.TopProperty, CenterY  - Radius);
            
            this.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
            
            CenterPoint.SetValue(Canvas.LeftProperty, CenterX);
            CenterPoint.SetValue(Canvas.TopProperty, CenterY);
                        
            InitProvider();
            ArcView.provider = this;
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartAngle = Standard;
            EndAngle = Standard;
            UpdateView();
        }

        private void InitProvider()
        {
            Brush = Brushes.Transparent;
            Pen = new Pen(_penBrush, 7.0);

            Position = new Point(CenterX, CenterY);
            StartAngle = Standard;
            EndAngle = Standard;
            RadiusX = Radius;
            RadiusY = Radius;
        }
        
        private Point _startPoint;
        private bool _isLeftMouseDown;
        
        private void BgEllipse_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(this.BgCanvas);
            BgEllipse.CaptureMouse();
            _isLeftMouseDown = true;
        }

        private void BgEllipse_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isLeftMouseDown)
            {
                Point currentPoint = e.GetPosition(this.BgCanvas);
                double valueOffset = _startPoint.X - currentPoint.X;
                Debug.WriteLine($"valueOffset = _startPoint.X - currentPoint.X : {valueOffset} = {_startPoint.X} - {currentPoint.X}");
                if(valueOffset != 0)
                {
                    _startPoint = currentPoint;
                    //左
                    if (valueOffset > 0)
                    {
                        UpdateRatioView(IncreaseRatio, Direction.Left);
                    }
                    //右
                    else
                    {
                        UpdateRatioView(DecreaseRatio, Direction.Right);
                    }
                }
            }
        }

        private void BgEllipse_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isLeftMouseDown)
            {
                _isLeftMouseDown = false;
                BgEllipse.ReleaseMouseCapture();
            }
        }

        private void UpdateRatioView(float changeRatio, Direction direction)
        {
            if (EndAngle > Standard && Math.Abs(StartAngle - Standard) == 0)
            {
                EndAngle = Math.Max(Math.Min(LeftBoundAngle, EndAngle + changeRatio), Standard);
            }
            else if(Math.Abs(EndAngle - Standard) == 0 && StartAngle < Standard)
            {
                StartAngle =
                    Math.Min(Math.Max(RightBoundAngle, StartAngle + changeRatio), Standard);
            }
            else if (Math.Abs(StartAngle - Standard) == 0 && Math.Abs(EndAngle - Standard) == 0)
            {
                if (direction == Direction.Left)
                {
                    EndAngle = Math.Max(Math.Min(LeftBoundAngle, EndAngle + changeRatio), Standard);
                }
                else
                {
                    StartAngle = Math.Min(Math.Max(RightBoundAngle, StartAngle + changeRatio), Standard);
                }
            }
            
            UpdateView();
        }
        
        
        private void UpdateView()
        {
            AngleToPanValue();
            ArcView.InvalidateVisual();
        }

        private void AngleToPanValue()
        {
            const int allAngle = LeftBoundAngle - RightBoundAngle;
            var allLen = Maximum - Minimum;
            
            var angle = Math.Abs(StartAngle - EndAngle);

            if (StartAngle < Standard && Math.Abs(EndAngle - Standard) == 0)
            {
                Pan = allLen * angle / allAngle;
                PanText = $"R {Pan:0.00}";
                RotateAngle = Standard - StartAngle;
            }
            else if (EndAngle > Standard && Math.Abs(StartAngle - Standard) == 0)
            {
                Pan = - allLen * angle / allAngle;
                PanText = $"L {Math.Abs(Pan):0.00}";
                RotateAngle = Standard - EndAngle;
            }
            else if (Math.Abs(EndAngle - Standard) == 0 && Math.Abs(StartAngle - Standard) == 0)
            {
                PanText = "0.00";
                RotateAngle = 0;
            }
        }
    }
}