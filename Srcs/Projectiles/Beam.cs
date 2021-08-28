using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class Beam : AProjectile
    {
        public Beam(double angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 50,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/beam.png"))
            }
        }, 10, 20, angle, 200, true)
        { }
    }
}
