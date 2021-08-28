using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Spice_Scroll_Shooter.Srcs.Bonuses
{
    public class MoneyBonus : ABonus
    {
        public override void Effect(Player player)
        {
            if (player.Score < int.MaxValue - 10)
            {
                player.Score += 10;
            }
        }
        public MoneyBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Money.png"))
            }
        }, 10, 0)
        { }
    }
}
