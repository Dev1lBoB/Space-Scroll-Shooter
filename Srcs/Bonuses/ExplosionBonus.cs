using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using Spice_Scroll_Shooter.Srcs.Enemies;
using System.Linq;
using System.Windows.Controls;

namespace Spice_Scroll_Shooter.Srcs.Bonuses
{
    public class ExplosionBonus : ABonus
    {
        public static Rectangle ExplosionModel { get; } = new Rectangle
        {
            Width = 400,
            Height = 400,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Explosion.png"))
            }
        };
        public static bool IsActive { get; set; } = false;
        public override void Effect(Player player)
        {
            if (IsActive == false)
            {
                Player.MyCanvas.Children.Add(ExplosionModel);
                IsActive = true;
            }
            foreach (AEnemy x in AObject.Objects.Where(en => en is AEnemy))
            {
                x.TakeDamage(1);
            }
        }
        public ExplosionBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Bang.png"))
            }
        }, 10, 0)
        { Canvas.SetLeft(ExplosionModel, 100); Canvas.SetTop(ExplosionModel, 100); }
    }
}
