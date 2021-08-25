using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CircleChannel
{
    public class ArcPath : Border
    {
        // private readonly Brush _brush;
        // private readonly Pen _pen;
        // private readonly Point _position;
        // private readonly double _startAngle;
        // private readonly double _endAngle;
        // private readonly double _radiusX;
        // private readonly double _radiusY;

        public IProvider provider;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawArcToLeft(provider.Brush, provider.Pen, provider.Position, provider.StartAngle, provider.EndAngle,
                provider.RadiusX, provider.RadiusY);
        }
    }
}