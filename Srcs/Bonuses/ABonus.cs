using System.Windows.Shapes;

namespace Spice_Scroll_Shooter.Srcs.Bonuses
{
    public abstract class ABonus : AObject
    {
        public abstract void Effect(Player player);
        public bool IsTaken { get; set; } = false;
        public ABonus(Rectangle model, int speed, double angle) : base(model, speed, angle) { }
    }
}
