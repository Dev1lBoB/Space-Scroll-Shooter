using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class Fireball : AProjectile
    {
        public Fireball(int angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 30,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/fireball.png"))
            }
        }, 10, 15, angle, 200, true)
        { }
    }
}
