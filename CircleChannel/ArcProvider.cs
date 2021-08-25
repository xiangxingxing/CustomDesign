using System.Windows;
using System.Windows.Media;

namespace CircleChannel
{
    public interface IProvider
    {
        Brush Brush { get; set; }
        Pen Pen { get; set; }
        Point Position { get; set; }
        double StartAngle { get; set; }
        double EndAngle { get; set; }
        double RadiusX { get; set; }
        double RadiusY { get; set; }
    }
}