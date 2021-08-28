using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Bullets
{
    public class ABullet : AObject
    {
        public int Damage { get; set; }
        public ABullet(Rectangle model, int speed, double angle) : base(model, speed, angle) { }
    }
}
