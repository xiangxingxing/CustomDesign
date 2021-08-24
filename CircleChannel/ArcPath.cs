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

            //dc.DrawArc(_brush, _pen, _position, _startAngle, _endAngle, _radiusX, _radiusY);
            dc.DrawArcToLeft(provider.Brush, provider.Pen, provider.Position, provider.StartAngle, provider.EndAngle,
                provider.RadiusX, provider.RadiusY);
            // if (provider.LeftArea)
            // {
            //     dc.DrawArcToLeft(provider.Brush, provider.Pen, provider.Position, provider.StartAngle, provider.EndAngle,
            //         provider.RadiusX, provider.RadiusY);
            // }
            // else
            // {
            //     dc.DrawArcToRight(provider.Brush, provider.Pen, provider.Position, provider.StartAngle, provider.EndAngle,
            //         provider.RadiusX, provider.RadiusY);
            // }
        }
    }
}