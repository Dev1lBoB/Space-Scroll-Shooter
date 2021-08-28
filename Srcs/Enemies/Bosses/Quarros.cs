using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Spice_Scroll_Shooter.Srcs.Projectiles;

namespace Spice_Scroll_Shooter.Srcs.Enemies.Bosses
{
    public class Quarros : ABoss
    {
        public Rectangle SheildModel { get; } = new Rectangle
        {
            Height = 275,
            Width = 275,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/QuarrosSheild.png"))
            }
        };
        public int SheildHp { get; set; } = 0;
        public double InSheildHorizontalSpeed { get; set; } = 12;
        public int IntSheildAttackSpeed { get; set; } = 25;
        public bool IsSheildActive { get; set; } = false;
        private static PointCollection CreateQuarrosHBox()
        {
            return new PointCollection()
            {
                new Point
                {
                    X = 390,
                    Y = 160
                },
                new Point
                {
                    X = 380,
                    Y = 195
                },
                new Point
                {
                    X = 352,
                    Y = 196
                },
                new Point
                {
                    X = 330,
                    Y = 180
                },
                new Point
                {
                    X = 326,
                    Y = 191
                },
                new Point
                {
                    X = 315,
                    Y = 170
                },
                new Point
                {
                    X = 295,
                    Y = 197
                },
                new Point
                {
                    X = 273,
                    Y = 197
                },
                new Point
                {
                    X = 260,
                    Y = 185
                },
                new Point
                {
                    X = 247,
                    Y = 197
                },
                new Point
                {
                    X = 225,
                    Y = 197
                },
                new Point
                {
                    X = 205,
                    Y = 170
                },
                new Point
                {
                    X = 194,
                    Y = 191
                },
                new Point
                {
                    X = 190,
                    Y = 180
                },
                new Point
                {
                    X = 168,
                    Y = 196
                },
                new Point
                {
                    X = 140,
                    Y = 195
                },
                new Point
                {
                    X = 130,
                    Y = 160
                }
            };
        }
        public override void TakeDamage(int damage)
        {
            if (SheildHp == 0)
            {
                base.TakeDamage(damage);
            }
            else
            {
                SheildHp -= damage;
                if (SheildHp <= 0)
                {
                    base.TakeDamage(-SheildHp);
                    SheildHp = 0;
                    MyCanvas.Children.Remove(SheildModel);
                }
            }
        }
        public static void QuarrosAttack1()
        {
            List<HomingMissile> missiles = new List<HomingMissile>
            {
                new HomingMissile(0),
                new HomingMissile(100),
                new HomingMissile(-100)
            };
            foreach (var x in missiles)
            {
                AObject.Objects.Add(x);
                Canvas.SetLeft(x.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[8].X + x.Side);
                Canvas.SetTop(x.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[8].Y);
                MyCanvas.Children.Add(x.Model);
            }
        }
        public static void QuarrosAttack2()
        {
            IsAttack = "QuarrosBeamAttack";
            MaxTimeOfAttack = 10;
            Beam beam = new Beam((((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[9].X - Canvas.GetLeft(Player.Model) - Player.Model.Width / 2) / (((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[9].Y - Canvas.GetTop(Player.Model)) * 180.0 / Math.PI);
            Canvas.SetLeft(beam.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[9].X);
            Canvas.SetTop(beam.Model, ((ABoss)AObject.Objects.FirstOrDefault(b => b is ABoss)).HBox.Points[9].Y);
            AObject.Objects.Add(beam);
            _ = MyCanvas.Children.Add(beam.Model);
        }
        public Quarros(Canvas canvas) : base(CreateBossModel(200, 275, "pack://application:,,,/images/Quarros.png"), "Quarros",
            new List<Attack>
            {
                QuarrosAttack1,
                QuarrosAttack2
            }, 40, 4, 150)
        {
            HBox.Points = CreateQuarrosHBox();
            MyCanvas = canvas;
        }
    }
}
