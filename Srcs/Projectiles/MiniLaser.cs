using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class MiniLaser : AProjectile
    {
        public MiniLaser(double angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 5,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/laserRed.png"))
            }
        }, 5, 25, angle, 200, true)
        { }
    }
}
