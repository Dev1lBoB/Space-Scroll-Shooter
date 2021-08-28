using Spice_Scroll_Shooter.Srcs.Bullets;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Weapons
{
    public class TripleGun : AWeapon
    {
        public override void Attack(Rectangle PlayerModel, Canvas MyCanvas)
        {
            List<ABullet> bullets = new List<ABullet>
            {
                new Bullet(0),
                new Bullet(10),
                new Bullet(-10)
            };
            foreach (var bullet in bullets)
            {
                Canvas.SetLeft(bullet.Model, Canvas.GetLeft(PlayerModel) + PlayerModel.Width / 2 - 2);
                Canvas.SetTop(bullet.Model, Canvas.GetTop(PlayerModel) - bullet.Model.Height);
                AObject.Objects.Add(bullet);
                _ = MyCanvas.Children.Add(bullet.Model);
            }
        }
    }
}
