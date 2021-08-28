using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Spice_Scroll_Shooter.Srcs.Bonuses
{
    public class SheildBonus : ABonus
    {
        public override void Effect(Player player)
        {
            player.ShieldTimer = 150;
        }
        public SheildBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/SheildBonus.png"))
            }
        }, 10, 0)
        { }
    }
}
