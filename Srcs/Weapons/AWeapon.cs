using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Weapons
{
    public abstract class AWeapon
    {
        public abstract void Attack(Rectangle PlayerModel, Canvas MyCanvas);
    }
}
