using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class PowerSphere : AProjectile
    {
        public PowerSphere() : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 70,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/RedEnergyBall.png"))
            }
        }, 30, 5, 0, 50, true)
        { }
    }
}
