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
        private const double CenterX = 150;
        private const double CenterY = 150;
        private const double Radius = 30;
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
        public bool LeftArea { get; set; }
        
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
            //Brush = new SolidColorBrush(Color.FromArgb(255 ,17, 18, 19));
            Pen = new Pen(_penBrush, 7.0);

            Position = new Point(CenterX, CenterY);
            StartAngle = Standard;
            EndAngle = Standard;
            RadiusX = Radius;
            RadiusY = Radius;

            LeftArea = true;
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

        private void UpdateView()
        {
            ArcView.InvalidateVisual();
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
    }
}