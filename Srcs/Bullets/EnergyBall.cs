using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Bullets
{
    public class EnergyBall : ABullet
    {
        public EnergyBall(double angle) : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/EnergyBall.png"))
            }
        }, -20, angle)
        { Damage = 2; }
    }
}
