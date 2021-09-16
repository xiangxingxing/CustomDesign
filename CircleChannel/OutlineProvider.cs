using System.Windows;
using System.Windows.Media;

namespace CircleChannel
{
    public class OutlineProvider : IProvider
    {
        public Brush Brush { get; set; }
        public Pen Pen { get; set; }
        public Point Position { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }

        public OutlineProvider(double radius)
        {
            // ReSharper disable once PossibleNullReferenceException
            Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#383B3E"));
            Pen = new Pen(Brush, 0);
            RadiusX = radius;
            RadiusY = radius;
        }
    }
}