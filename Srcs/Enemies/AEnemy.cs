using System;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Spice_Scroll_Shooter.Srcs.Enemies
{
    public abstract class AEnemy : AObject
    {
        public bool IsDead { get; set; }
        public int Damage { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; }
        public string Type { get; set; }


        public virtual void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                Hp = 0;
                IsDead = true;
                ToRemove = true;
            }
        }
        public AEnemy(int speed, double angle, int damage, int hp, string type, Rectangle model) : base(model, speed, angle)
        {
            Damage = damage;
            Hp = hp;
            MaxHp = hp;
            Type = type;
            IsDead = false;
        }

    }
}
