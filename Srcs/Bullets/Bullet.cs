using System.Windows.Media;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Bullets
{
    public class Bullet : ABullet
    {
        public Bullet(double angle) : base(new Rectangle
        {
            Height = 20,
            Width = 5,
            Fill = Brushes.Orange,
            Stroke = Brushes.Red
        }, -20, angle)
        { Damage = 1; }
    }
}
