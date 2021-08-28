using System;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spice_Scroll_Shooter.Srcs.Enemies
{
    public class StrafingEnemy : AEnemy
    {
        static readonly Random Rnd = new Random();
        static readonly int[] angles = new int[2] { 25, -25 };
        public StrafingEnemy() : base(10, angles[Rnd.Next(0, 2)], 5, 1, "StrafingEnemy", new Rectangle
        {
            Tag = "Enemy",
            Height = 50,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack5.png"))
            }
        })
        { }
    }
}
