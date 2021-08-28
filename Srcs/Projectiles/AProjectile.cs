using System.Windows.Shapes;


namespace Spice_Scroll_Shooter.Srcs.Projectiles
{
    public class AProjectile : AObject
    {

        public int Damage { get; }
        public int Lifetime { get; set; }
        public bool Destructible { get; }
        public AProjectile(Rectangle model, int damage, int speed, double angle, int lifetime, bool destructible) : base(model, speed, angle)
        {
            Damage = damage;
            Lifetime = lifetime;
            Destructible = destructible;
        }
    }
}
