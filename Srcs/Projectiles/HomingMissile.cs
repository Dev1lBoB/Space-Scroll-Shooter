using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class HomingMissile : AProjectile
    {
        public int Side { get; set; }
        public HomingMissile() : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/missile.png"))
            }
        }, 10, 10, 0, 200, true)
        { }
        public HomingMissile(int side) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/missile.png"))
            }
        }, 10, 10, 0, 200, true)
        { Side = side; }
    }
}
