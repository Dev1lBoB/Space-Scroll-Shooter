using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace Spice_Scroll_Shooter.Srcs.Bonuses
{
    public class RepairBonus : ABonus
    {
        public override void Effect(Player player)
        {
            player.Repair(10);
        }

        public RepairBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Cross.png"))
            }
        }, 10, 0)
        { }
    }
}
