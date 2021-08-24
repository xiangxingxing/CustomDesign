using System.Windows;
using System.Windows.Controls;
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
        
        bool LeftArea { get; set; }
    }
    
    // public class ArcProvider : IProvider
    // {
    //     public Brush Brush { get; set; }
    //     public Pen Pen { get; set; }
    //     public Point Position { get; set; }
    //     public double StartAngle { get; set; }
    //     public double EndAngle { get; set; }
    //     public double RadiusX { get; set; }
    //     public double RadiusY { get; set; }
    // }
}