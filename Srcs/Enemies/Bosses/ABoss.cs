using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Spice_Scroll_Shooter.Srcs.Projectiles;

namespace Spice_Scroll_Shooter.Srcs.Enemies.Bosses
{
    public abstract class ABoss : AEnemy
    {
        public static Canvas MyCanvas { get; set; }
        public List<Attack> Attacks;
        public int AttackSpeed { get; set; }
        public int AttackCount { get; set; }
        public readonly DispatcherTimer AttackTimer = new DispatcherTimer();
        public Rectangle HealthBar = new Rectangle
        {
            Height = 25,
            Fill = Brushes.GreenYellow
        };
        public Polygon HBox = new Polygon
        {
            //
            // Uncomment this section to see hitboxes of every boss
            //
            //   Fill = Brushes.White,
            //   Stroke = Brushes.White
        };
        public static string IsAttack { get; set; }
        public static int TimeOfAttack { get; set; }
        public static int MaxTimeOfAttack { get; set; }
        protected static Rectangle CreateBossModel(int height, int width, string uri)
        {
            return new Rectangle
            {
                Tag = "Enemy",
                Height = height,
                Width = width,
                Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(uri))
                }
            };
        }
        public override double Angle
        {
            get
            {
                return _Angle;
            }
            set
            {
                _Angle = value;
                Model.RenderTransform = new RotateTransform(-_Angle / Math.PI * 180);
                if (Angle == 0)
                {
                    HorizontalSpeed = -HorizontalSpeed;
                }
                else
                {
                    HorizontalSpeed = RawSpeed * Math.Cos(Angle);
                    VerticalSpeed = RawSpeed * Math.Sin(Angle);
                }
            }
        }

        public override void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                foreach (AEnemy x in AObject.Objects.Where(obj => obj is AEnemy))
                {
                    x.Hp = 0;
                    x.IsDead = true;
                    x.ToRemove = true;
                }
                MyCanvas.Children.Remove(HealthBar);
                TimeOfAttack = 0;
                MaxTimeOfAttack = 0;
                IsAttack = null;
            }
            else
            {
                HealthBar.Width = (540.0 / MaxHp) * Hp;
            }
        }
        public ABoss(Rectangle model, string name, List<Attack> atck, int attackSpeed, int hSpeed, int hp) : base(hSpeed, 0, 0, hp, name, model)
        {
            Attacks = atck;
            AttackSpeed = attackSpeed;
            AttackCount = attackSpeed;
            HorizontalSpeed = hSpeed;
            VerticalSpeed = 0;
        }
    }
}
