using System;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spice_Scroll_Shooter.Srcs.Enemies
{
    public class DangerousEnemy : AEnemy
    {
        public DangerousEnemy() : base(10, 0, 10, 1, "DangerousEnemy", new Rectangle
        {
            Tag = "Enemy",
            Height = 50,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack3.png"))
            }
        })
        { }
    }
}
