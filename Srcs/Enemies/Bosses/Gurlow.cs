using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Spice_Scroll_Shooter.Srcs.Projectiles;

namespace Spice_Scroll_Shooter.Srcs.Enemies.Bosses
{
    public class Gurlow : ABoss
    {
        private static PointCollection CreateGurlowHBox()
        {
            return new PointCollection()
            {
                new Point
                {
                    X = 360,
                    Y = 75
                },
                new Point
                {
                    X = 340,
                    Y = 125
                },
                new Point
                {
                    X = 330,
                    Y = 125
                },
                new Point
                {
                    X = 310,
                    Y = 85
                },
                new Point
                {
                    X = 290,
                    Y = 125
                },
                new Point
                {
                    X = 260,
                    Y = 125
                },
                new Point
                {
                    X = 230,
                    Y = 125
                },
                new Point
                {
                    X = 210,
                    Y = 85
                },
                new Point
                {
                    X = 190,
                    Y = 125
                },
                new Point
                {
                    X = 180,
                    Y = 125
                },
                new Point
                {
                    X = 160,
                    Y = 75
                }
            };
        }
        public static void GurlowLaserAttack()
        {
            Gurlow gurlow = (Gurlow)AObject.Objects.FirstOrDefault(y => y is ABoss);
            GurlowLaser Laser1 = new GurlowLaser(true);
            AObject.Objects.Add(Laser1);
            Canvas.SetLeft(Laser1.Model, Canvas.GetLeft(gurlow.Model) + Laser1.Side + gurlow.Model.Width / 2 - 4);
            Canvas.SetTop(Laser1.Model, gurlow.HBox.Points[5].Y);
            _ = MyCanvas.Children.Add(Laser1.Model);
            GurlowLaser Laser2 = new GurlowLaser(false);
            AObject.Objects.Add(Laser2);
            Canvas.SetLeft(Laser2.Model, Canvas.GetLeft(gurlow.Model) + Laser2.Side + gurlow.Model.Width / 2 - 4);
            Canvas.SetTop(Laser2.Model, gurlow.HBox.Points[5].Y);
            _ = MyCanvas.Children.Add(Laser2.Model);
            Gurlow.IsAttack = "GurlowLaserAttack";
            Gurlow.MaxTimeOfAttack = 50;
        }
        public static void GurlowMinionsSpawnAttack()
        {
            AEnemy NewEnemy1 = new DangerousEnemy();
            AEnemy NewEnemy2 = new DangerousEnemy();
            AObject.Objects.Add(NewEnemy1);
            Canvas.SetTop(NewEnemy1.Model, -100);
            Canvas.SetLeft(NewEnemy1.Model, 80);
            AObject.Objects.Add(NewEnemy2);
            Canvas.SetTop(NewEnemy2.Model, -100);
            Canvas.SetLeft(NewEnemy2.Model, 400);
            _ = MyCanvas.Children.Add(NewEnemy1.Model);
            _ = MyCanvas.Children.Add(NewEnemy2.Model);
        }
        public static void GurlowFireballsAttack()
        {
            List<Fireball> fireballs = new List<Fireball>
            {
                new Fireball(0),
                new Fireball(-15),
                new Fireball(15),
                new Fireball(-30),
                new Fireball(30),
                new Fireball(-45),
                new Fireball(45),
                new Fireball(-60),
                new Fireball(60)
            };
            foreach (var x in fireballs)
            {
                AObject.Objects.Add(x);
                Canvas.SetLeft(x.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[5].X);
                Canvas.SetTop(x.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[5].Y);
                MyCanvas.Children.Add(x.Model);
            }
        }
        public Gurlow(Canvas canvas) : base(CreateBossModel(120, 200, "pack://application:,,,/images/Gurlow.png"), "Gurlow",
            new List<Attack>
            {
                GurlowLaserAttack,
                GurlowMinionsSpawnAttack,
                GurlowFireballsAttack
            }, 75, 8, 100)
        {
            HBox.Points = CreateGurlowHBox();
            MyCanvas = canvas;
        }
    }
}
