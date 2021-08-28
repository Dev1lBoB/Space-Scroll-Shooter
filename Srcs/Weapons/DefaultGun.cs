using Spice_Scroll_Shooter.Srcs.Bullets;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Weapons
{
    public class DefaultGun : AWeapon
    {
        public override void Attack(Rectangle PlayerModel, Canvas MyCanvas)
        {
            ABullet bullet = new Bullet(0);
            Canvas.SetLeft(bullet.Model, Canvas.GetLeft(PlayerModel) + PlayerModel.Width / 2 - 2);
            Canvas.SetTop(bullet.Model, Canvas.GetTop(PlayerModel) - bullet.Model.Height);
            AObject.Objects.Add(bullet);
            _ = MyCanvas.Children.Add(bullet.Model);
        }
    }
}
