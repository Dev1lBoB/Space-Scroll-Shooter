using System;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spice_Scroll_Shooter.Srcs.Enemies
{
    public class StrongEnemy : AEnemy
    {
        public StrongEnemy() : base(10, 0, 5, 3, "StrongEnemy", new Rectangle
        {
            Tag = "Enemy",
            Height = 50,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack2.png"))
            }
        })
        { }
    }
}
