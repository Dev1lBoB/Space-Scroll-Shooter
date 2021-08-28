using System;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spice_Scroll_Shooter.Srcs.Enemies
{
    public class FastEnemy : AEnemy
    {
        public FastEnemy() : base(20, 0, 5, 1, "FastEnemy", new Rectangle
        {
            Tag = "Enemy",
            Height = 50,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack1.png"))
            }
        })
        { }
    }
}
