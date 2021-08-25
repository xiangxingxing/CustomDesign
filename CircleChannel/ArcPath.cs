using System.Windows.Controls;
using System.Windows.Media;

namespace CircleChannel
{
    public class ArcPath : Border
    {
        public IProvider provider;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawArcToLeft(provider.Brush, provider.Pen, provider.Position, provider.StartAngle, provider.EndAngle,
                provider.RadiusX, provider.RadiusY);
        }
    }
}