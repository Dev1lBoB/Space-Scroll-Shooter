
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class GurlowLaser : AProjectile
    {
        public int Side { get; } = 75;
        public GurlowLaser(bool side) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = Application.Current.MainWindow.Height,
            Width = 10,
            Fill = Brushes.Blue,
            Stroke = Brushes.Purple
        }, 1, 0, 0, 50, false)
        {
            if (side)
            {
                Side = -Side;
            }
        }
    }
}
