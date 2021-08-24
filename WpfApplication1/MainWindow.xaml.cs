﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
       
        }
        private bool _isPressed = false;
        private Point _startPoint;
        private Canvas _templateCanvas = null;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Enable moving mouse to change the value.
            _isPressed = true;

            //Find the parent canvas.
            if (_templateCanvas == null)
            {
                _templateCanvas = MyHelper.FindParent<Canvas>(e.Source as Ellipse);
                if (_templateCanvas == null) return;
            }
            _templateCanvas.CaptureMouse();
            _startPoint = e.GetPosition(_templateCanvas);
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPressed)
            {
                //Canculate the current rotation angle and set the value.
                const double radius = 150;
                var currentPoint = e.GetPosition(_templateCanvas);

                var angle = MyHelper.GetAngleR(currentPoint, _startPoint, radius);
                knob.Value = (knob.Maximum - knob.Minimum) * angle / (2 * Math.PI);
            }
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Disable moving mouse to change the value.
            if (_isPressed)
            {
                _isPressed = false;
                _templateCanvas.ReleaseMouseCapture();
            }
        }
    }

    //The converter used to convert the value to the rotation angle.
    public class ValueAngleConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            double value = (double)values[0];
            double minimum = (double)values[1];
            double maximum = (double)values[2];

            return MyHelper.GetAngle(value, maximum, minimum);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
              System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    //Convert the value to text.
    public class ValueTextConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
                  System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            return String.Format("{0:F2}", v);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class MyHelper
    {
        //Get the parent of an item.
        public static T FindParent<T>(FrameworkElement current)
          where T : FrameworkElement
        {
            do
            {
                current = VisualTreeHelper.GetParent(current) as FrameworkElement;
                if (current is T)
                {
                    return (T)current;
                }
            }
            while (current != null);
            return null;
        }

        //Get the rotation angle from the value
        public static double GetAngle(double value, double maximum, double minimum)
        {
            double current = (value / (maximum - minimum)) * 360;
            if (current == 360)
                current = 359.999;

            return current;
        }

        //Get the rotation angle from the position of the mouse
        public static double GetAngleR(Point currentPoint, Point startPoint, double radius)
        {
            //Calculate out the distance(r) between the center and the position
            var center = new Point(radius, radius);
            //double xDiff = center.X - pos.X;
            //double yDiff = center.Y - pos.Y;

            double xDiff = currentPoint.X - startPoint.X;
            //约定：水平方向 1像素 = 0.5°
            double xAngle = 0.5;

            double targetAngle = 0; 
            if (xDiff > 0)
            {
                //move right
                targetAngle = xAngle * xDiff;
            }
            else if (xDiff < 0)
            {
                //move left
                targetAngle = xAngle * xDiff;
            }


            return targetAngle;
            //double r = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            //Calculate the angle
            // double angle = Math.Acos((center.Y - currentPoint.Y) / r);
            // Console.WriteLine("r:{0},y:{1},angle:{2}.", r, currentPoint.Y, angle);
            // if (currentPoint.X < radius)
            //     angle = 2 * Math.PI - angle;
            // if (Double.IsNaN(angle))
            //     return 0.0;
            // else
            //     return angle;
        }
    
}
}