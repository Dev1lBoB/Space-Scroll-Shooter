using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace Spice_Scroll_Shooter
{
    public delegate void Attack();
    public class Player
    {
        public static Canvas MyCanvas { get; set; }
        public int Speed { get; set; }
        public Label HealthText { get; set; } = new Label
        {
            Content = "100%",
            FontSize = 18,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.LightBlue
        };
        private int _Health;
        public int Health
        {
            get => _Health;
            set
            {
                _Health = value;
                HealthText.Content = $"{(double)value / MaxHealth * 100}%";
            }
        }
        public int MaxHealth { get; }
        public Label ScoreText { get; set; } = new Label
        {
            Content = "Score: 0",
            FontSize = 18,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White
        };
        private int _Score;
        public int Score
        {
            get => _Score;
            set
            {
                _Score = value;
                ScoreText.Content = "Score: " + value;
            }
        }
        public Rect HBox { get; set; }
        public AWeapon Weapon { get; set; } = new DefaultGun();
        public static Rectangle Model { get; set; } = new Rectangle
        {
            Width = 60,
            Height = 50,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip2_red.png"))
            }
        };
        public Rectangle ShieldModel { get; set; } = new Rectangle
        {
            Height = 60,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Sheild.png"))
            }
        };
        public int ShieldTimer { get; set; }

        public void Attack()
        {
            Weapon.Attack(Model, MyCanvas);
        }
        public void TakeDamage(int damage)
        {
            if (ShieldTimer == 0)
            {
                Health -= damage;
                if (Health < 0)
                {
                    Health = 0;
                }
            }
        }
        public void Repair(int amount)
        {
            if (ShieldTimer == 0)
            {
                Health += amount;
                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
            }
        }
        public Player(int speed, int health, Canvas myCanvas)
        {
            Speed = speed;
            MaxHealth = health;
            Health = health;
            MyCanvas = myCanvas;
            Canvas.SetLeft(ScoreText, 5);
            Canvas.SetTop(ScoreText, Application.Current.MainWindow.Height - 75);
            Canvas.SetLeft(HealthText, Application.Current.MainWindow.Width - 100);
            Canvas.SetTop(HealthText, Application.Current.MainWindow.Height - 75);
        }
    }
    public abstract class AWeapon
    {
        public abstract void Attack(Rectangle PlayerModel, Canvas MyCanvas);
    }
    public class ABullet : AObject
    {
        public int Damage { get; set; }
        public ABullet(Rectangle model, int speed, double angle) : base(model, speed, angle) { }
    }
    public class Bullet : ABullet
    {
        public Bullet(double angle) : base(new Rectangle
        {
            Height = 20,
            Width = 5,
            Fill = Brushes.Orange,
            Stroke = Brushes.Red
        }, -20, angle)
        { Damage = 1; }
    }
    public class EnergyBall : ABullet
    {
        public EnergyBall(double angle) : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/EnergyBall.png"))
            }
        }, -20, angle)
        { Damage = 2; }
    }
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
    public class EnergyGun : AWeapon
    {
        public override void Attack(Rectangle PlayerModel, Canvas MyCanvas)
        {
            ABullet bullet = new EnergyBall(0);
            Canvas.SetLeft(bullet.Model, Canvas.GetLeft(PlayerModel) + PlayerModel.Width / 2 - 10);
            Canvas.SetTop(bullet.Model, Canvas.GetTop(PlayerModel) - bullet.Model.Height);
            AObject.Objects.Add(bullet);
            _ = MyCanvas.Children.Add(bullet.Model);
        }
    }
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
    public abstract class AObject
    {
        public double VerticalSpeed { get; set; }
        public double HorizontalSpeed { get; set; }
        public int RawSpeed { get; set; }
        protected double _Angle;
        public virtual double Angle
        {
            get
            {
                return _Angle;
            }
            set
            {
                _Angle = value;
                Model.RenderTransform = new RotateTransform(-_Angle / Math.PI * 180);
                VerticalSpeed = RawSpeed * Math.Cos(Angle);
                HorizontalSpeed = RawSpeed * Math.Sin(Angle);
            }
        }
        public Rectangle Model;
        public Rect HitBox { get; set; }
        public bool ToRemove { get; set; } = false;
        public static List<AObject> Objects { get; } = new List<AObject>();
        public AObject(Rectangle model, int speed, double angle)
        {
            Model = model;
            Angle = angle * Math.PI / 180;
            VerticalSpeed = speed * Math.Cos(Angle);
            HorizontalSpeed = speed * Math.Sin(Angle);
            RawSpeed = speed;
        }
    }
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
    public class FastEnemy : AEnemy
    {
        public FastEnemy() : base(20, 0, 5, 1, "FastEnemy", new Rectangle
        {
            Tag = "Enemy", Height = 50, Width = 60, Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack1.png"))
            }
        }) { }
    }
    public class StrongEnemy : AEnemy
    {
        public StrongEnemy() : base(10, 0, 5, 3, "StrongEnemy", new Rectangle
        {
            Tag = "Enemy", Height = 50, Width = 60, Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack2.png"))
            }
}) { }
    }
    public class DangerousEnemy : AEnemy
    {
        public DangerousEnemy() : base(10, 0, 10, 1, "DangerousEnemy", new Rectangle
        {
            Tag = "Enemy", Height = 50, Width = 60, Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack3.png"))
            }
}) { }
    }
    public class StrafingEnemy : AEnemy
    {
        static readonly Random Rnd = new Random();
        static readonly int[] angles = new int[2] { 25, -25 };
        public StrafingEnemy() : base(10, angles[Rnd.Next(0, 2)], 5, 1, "StrafingEnemy", new Rectangle
        {
            Tag = "Enemy",
            Height = 50,
            Width = 60,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/enemyBlack5.png"))
            }
        })
        { }
    }
    public class Boss : AEnemy
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
        public Boss(Rectangle model, string name, List<Attack> atck, int attackSpeed, int hSpeed, int hp) : base(hSpeed, 0, 0, hp, name, model)
        {
            Attacks = atck;
            AttackSpeed = attackSpeed;
            AttackCount = attackSpeed;
            HorizontalSpeed = hSpeed;
            VerticalSpeed = 0;
        }
    }
    public abstract class ABonus : AObject
    {
        public abstract void Effect(Player player);
        public bool IsTaken { get; set; } = false;
        public ABonus(Rectangle model, int speed, double angle) : base(model, speed, angle) { }
    }
    public class MoneyBonus : ABonus
    {
        public override void Effect(Player player)
        {
            if (player.Score < int.MaxValue - 10)
            {
                player.Score += 10;
            }
        }
        public MoneyBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Money.png"))
            }
        }, 10, 0)
        { }
    }
    public class SheildBonus : ABonus
    {
        public override void Effect(Player player)
        {
            player.ShieldTimer = 150;
        }
        public SheildBonus() : base(new Rectangle
        { 
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/SheildBonus.png"))
            }
        }, 10, 0) { }
    }
    public class RepairBonus : ABonus
    {
        public override void Effect(Player player)
        {
            player.Repair(10);
        }

        public RepairBonus() : base(new Rectangle
        {
            Height = 20,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Cross.png"))
            }
        }, 10, 0)
        { }
    }
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
    public class Fireball : AProjectile
    {
        public Fireball(int angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 30,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/fireball.png"))
            }
        }, 10, 15, angle, 200, true) { }
    }
    public class Beam : AProjectile
    {
        public Beam(double angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 50,
            Width = 20,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/beam.png"))
            }
        }, 10, 20, angle, 200, true)
        { }
    }
    public class MiniLaser : AProjectile
    {
        public MiniLaser(double angle) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 5,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/laserRed.png"))
            }
        }, 5, 25, angle, 200, true)
        { }
    }
    public class GurlowLaser : AProjectile
    {
        public int Side { get; } = 75;
        public GurlowLaser(bool side) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = Application.Current.MainWindow.Height,
            Width = 10,
            Fill = Brushes.Blue,
            Stroke = Brushes.Purple
        }, 1, 0, 0, 50, false)
        {
            if (side)
            {
                Side = -Side;
            }
        }
    }
    public class HomingMissile : AProjectile
    {
        public int Side { get; set; }
        public HomingMissile() : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/missile.png"))
            }
        }, 10, 10, 0, 200, true)
        { }
        public HomingMissile(int side) : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 15,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/missile.png"))
            }
        }, 10, 10, 0, 200, true)
        { Side = side; }
    }
    public class PowerSphere : AProjectile
    {
        public PowerSphere() : base(new Rectangle
        {
            Tag = "Projectile",
            Height = 40,
            Width = 70,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/RedEnergyBall.png")) 
            }
        }, 30, 5, 0, 50, true){ }
    }
    public class Quarros : Boss
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
                Canvas.SetLeft(x.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[8].X + x.Side);
                Canvas.SetTop(x.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[8].Y);
                MyCanvas.Children.Add(x.Model);
            }
        }
        public static void QuarrosAttack2()
        {
            IsAttack = "QuarrosBeamAttack";
            MaxTimeOfAttack = 10;
            Beam beam = new Beam((((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[9].X - Canvas.GetLeft(Player.Model) - Player.Model.Width / 2) / (((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[9].Y - Canvas.GetTop(Player.Model)) * 180.0 /  Math.PI);
            Canvas.SetLeft(beam.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[9].X);
            Canvas.SetTop(beam.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[9].Y);
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
    public class Gurlow : Boss
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
            Gurlow gurlow = (Gurlow)AObject.Objects.FirstOrDefault(y => y is Boss);
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
                Canvas.SetLeft(x.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[5].X);
                Canvas.SetTop(x.Model, ((Boss)AObject.Objects.FirstOrDefault(b => b is Boss)).HBox.Points[5].Y);
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
    public class Drake : Boss
    {
        public static Rectangle Lightning1 { get; set; } = new Rectangle
        {
            Height = 20,
            Width = 72,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/lightning2.png"))
            },
            RenderTransformOrigin = new Point
            {
                X = 0.5,
                Y = 0.5
            }
        };
        public static Rectangle Lightning2 { get; set; } = new Rectangle
        {
            Height = 20,
            Width = 72,
            Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/lightning1.png"))
            },
            RenderTransformOrigin = new Point
            {
                X = 0.5,
                Y = 0.5
            }
        };
        public ScaleTransform ScaleTransform { get; set; } = new ScaleTransform { ScaleY = -1 };
        private static PointCollection CreateDrakeHBox()
        {
            return new PointCollection()
            {
                new Point
                {
                    X = 362,
                    Y = 75
                },
                new Point
                {
                    X = 335,
                    Y = 148
                },
                new Point
                {
                    X = 330,
                    Y = 148
                },
                new Point
                {
                    X = 320,
                    Y = 70
                },
                new Point
                {
                    X = 300,
                    Y = 62
                },
                new Point
                {
                    X = 292,
                    Y = 115
                },
                new Point
                {
                    X = 280,
                    Y = 115
                },
                new Point
                {
                    X = 260,
                    Y = 135
                },
                new Point
                {
                    X = 240,
                    Y = 115
                },
                new Point
                {
                    X = 228,
                    Y = 115
                },
                new Point
                {
                    X = 220,
                    Y = 62
                },
                new Point
                {
                    X = 200,
                    Y = 70
                },
                new Point
                {
                    X = 190,
                    Y = 148
                },
                new Point
                {
                    X = 185,
                    Y = 148
                },
                new Point
                {
                    X = 158,
                    Y = 75
                }
            };
        }
        public static void DrakeSphereAttack()
        {
            PowerSphere Sphere = new PowerSphere();
            Canvas.SetLeft(Sphere.Model, ((Boss)AObject.Objects.FirstOrDefault(obj => obj is Drake)).HBox.Points[7].X - 32);
            Canvas.SetTop(Sphere.Model, ((Boss)AObject.Objects.FirstOrDefault(obj => obj is Drake)).HBox.Points[7].Y - 10);
            AObject.Objects.Add(Sphere);
            Drake drake = (Drake)AObject.Objects.FirstOrDefault(obj => obj is Drake);
            Canvas.SetLeft(Lightning1, drake.HBox.Points[2].X - Lightning1.Width);
            Canvas.SetTop(Lightning1, drake.HBox.Points[2].Y - 10);
            Canvas.SetLeft(Lightning2, drake.HBox.Points[12].X);
            Canvas.SetTop(Lightning2, drake.HBox.Points[12].Y - 10);
            _ = MyCanvas.Children.Add(Lightning1);
            _ = MyCanvas.Children.Add(Lightning2);
            _ = MyCanvas.Children.Add(Sphere.Model);
            IsAttack = "DrakeSphereAttack";
            MaxTimeOfAttack = 9;
        }
        public static void DrakeLasersAttack()
        {
            Drake drake = (Drake)AObject.Objects.FirstOrDefault(obj => obj is Drake);
            MiniLaser laser1 = new MiniLaser(0);
            MiniLaser laser2 = new MiniLaser(0);
            Canvas.SetLeft(laser1.Model, drake.HBox.Points[6].X);
            Canvas.SetLeft(laser2.Model, drake.HBox.Points[8].X);
            Canvas.SetTop(laser1.Model, drake.HBox.Points[6].Y);
            Canvas.SetTop(laser2.Model, drake.HBox.Points[8].Y);
            AObject.Objects.Add(laser1);
            AObject.Objects.Add(laser2);
            _ = MyCanvas.Children.Add(laser1.Model);
            _ = MyCanvas.Children.Add(laser2.Model);
            IsAttack = "DrakeLasersAttack";
            MaxTimeOfAttack = 15;
        }
        public static void DrakeMissilesAttack()
        {
            Drake drake = (Drake)AObject.Objects.FirstOrDefault(obj => obj is Drake);
            HomingMissile missile = new HomingMissile();
            Canvas.SetLeft(missile.Model, drake.HBox.Points[7].X);
            Canvas.SetTop(missile.Model, drake.HBox.Points[7].Y - 10);
            AObject.Objects.Add(missile);
            _ = MyCanvas.Children.Add(missile.Model);
            MaxTimeOfAttack = 15;
            IsAttack = "DrakeMissilesAttack";
        }
        public Drake(Canvas canvas) : base(CreateBossModel(150, 200, "pack://application:,,,/images/Drake.png"), "Drake",
            new List<Attack>
            {
                DrakeSphereAttack,
                DrakeLasersAttack,
                DrakeMissilesAttack
            }, 50, 12, 300)
        {
            HBox.Points = CreateDrakeHBox();
            MyCanvas = canvas;
            Lightning1.RenderTransform = ScaleTransform;
            Lightning2.RenderTransform = ScaleTransform;
        }
    }
    public class Level
    {
        public string LevelName { get; }
        public int NumOfEnemies { get; }
        public int Limit { get; }
        public Level(string levelName, int numOfEnemies, int limit)
        {
            LevelName = levelName;
            NumOfEnemies = numOfEnemies;
            Limit = limit;
        }
    }
    public class BossLevel : Level
    {

        public Boss Boss { get; set; }
        public BossLevel(string levelName, Boss boss) : base(levelName, 1, 1) { Boss = boss; }
    }
    public class Shop
    {
        public Canvas MyCanvas { get; set; }
        public Player Player { get; set; }
        public DispatcherTimer Timer { get; set; }
        public Label Title = new Label
        {
            Content = "SHOP",
            FontSize = 38,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White
        };
        public Button TripleGunButton = new Button
        {
            Content = "150",
            FontSize = 28,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.DeepPink,
            Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/TripleGun.png"))
            },
            BorderThickness = new Thickness(0),
            Height = 60,
            Width = 170,
            ClickMode = ClickMode.Press
        };
        public Button EnergyGunButton = new Button
        {
            Content = "75",
            FontSize = 28,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.DeepPink,
            Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/EnergyGun.png"))
            },
            BorderThickness = new Thickness(0),
            Height = 60,
            Width = 170,
            ClickMode = ClickMode.Press
        };
        public Button RepairButton = new Button
        {
            Content = "10",
            FontSize = 28,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.DeepPink,
            Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Repair.png"))
            },
            BorderThickness = new Thickness(0),
            Height = 70,
            Width = 70,
            ClickMode = ClickMode.Press
        };
        public Button ContinueButton = new Button
        {
            Content = "Continue",
            FontSize = 28,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.DeepPink,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Height = 60,
            Width = 170,
            ClickMode = ClickMode.Press
        };
        public void OpenShop(Canvas myCanvas, Player player, DispatcherTimer timer)
        {
            MyCanvas = myCanvas;
            Player = player;
            Timer = timer;
            _ = myCanvas.Children.Add(Title);
            _ = myCanvas.Children.Add(TripleGunButton);
            _ = myCanvas.Children.Add(EnergyGunButton);
            _ = myCanvas.Children.Add(ContinueButton);
            _ = myCanvas.Children.Add(RepairButton);
        }
        public Shop()
        {
            Canvas.SetLeft(Title, 210);
            Canvas.SetTop(Title, 60);
            Canvas.SetLeft(TripleGunButton, 100);
            Canvas.SetTop(TripleGunButton, 350);
            Canvas.SetLeft(EnergyGunButton, 100);
            Canvas.SetTop(EnergyGunButton, 200);
            Canvas.SetLeft(ContinueButton, 190);
            Canvas.SetTop(ContinueButton, 500);
            Canvas.SetLeft(RepairButton, 400);
            Canvas.SetTop(RepairButton, 200);
            TripleGunButton.Click += TripleGun_Click;
            EnergyGunButton.Click += EnergyGunButton_Click;
            ContinueButton.Click += ContinueButton_Click;
            RepairButton.Click += RepairButton_Click;
        }



        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Remove(Title);
            MyCanvas.Children.Remove(TripleGunButton);
            MyCanvas.Children.Remove(EnergyGunButton);
            MyCanvas.Children.Remove(ContinueButton);
            MyCanvas.Children.Remove(RepairButton);
            MyCanvas.Focus();
            Timer.Start();
        }
        private void TripleGun_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Score >= 150 && (string)TripleGunButton.Content != "SOLD")
            {
                Player.Score -= 150;
                Player.Weapon = new TripleGun();
                TripleGunButton.Foreground = Brushes.Red;
                TripleGunButton.Content = "SOLD";
            }
        }
        private void EnergyGunButton_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Score >= 75 && (string)EnergyGunButton.Content != "SOLD")
            {
                Player.Score -= 75;
                Player.Weapon = new EnergyGun();
                EnergyGunButton.Foreground = Brushes.Red;
                EnergyGunButton.Content = "SOLD";
            }
        }
        private void RepairButton_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Score >= 10 && Player.Health < Player.MaxHealth)
            {
                Player.Score -= 10;
                Player.Repair(10);
            }
        }
    }
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer GameTimer = new DispatcherTimer();
        private bool IsMoveLeft, IsMoveRight;
        private readonly List<Rectangle> ItemRemover = new List<Rectangle>();
        private readonly Random  Rand = new Random();
        private int EnemySpriteCounter = 0;
        private int EnemySpawnCounter = 100;
        private int EnemyCount = 0;
        private bool LevelStart = true;
        private readonly Player Player;
        private readonly Shop Shop = new Shop();
        private readonly Label LevelLabel = new Label
        {
            Name = "LevelLabel",
            Content = "Level 1",
            FontSize = 36,
            FontWeight = FontWeights.Bold,
            Foreground = Brushes.White
        };

        private readonly List<Level> Levels = new List<Level>();

        public MainWindow()
        {
            InitializeComponent();
            Player = new Player(15, 200, MyCanvas);
            CreateLevels();
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Tick += GameLoop;

            ImageBrush BackGround = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/darkPurple.png")),
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 0.15, 0.15),
                ViewportUnits = BrushMappingMode.RelativeToBoundingBox
            };
            MyCanvas.Background = BackGround;
            Canvas.SetLeft(Player.Model, 228);
            Canvas.SetTop(Player.Model, 624);
            _ = MyCanvas.Children.Add(Player.Model);
        }

        private Boss CreateBoss(string name)
        {
            if (name == "Gurlow")
            {
                return new Gurlow(MyCanvas);
            }
            else if (name == "Quarros")
            {
                return new Quarros(MyCanvas);
            }
            else if (name == "Drake")
            {
                return new Drake(MyCanvas);
            }
            else
            {
                throw new Exception();
            }
        }

        private void CreateLevels() // Adding new levels to the game
        {
            Levels.Add(new Level("Level 1", 3, 50));
            Levels.Add(new Level("Level 2", 10, 15));
            Levels.Add(new Level("Level 3", 30, 40));
            Levels.Add(new BossLevel("Gurlow", CreateBoss("Gurlow")));
            Levels.Add(new Level("Level 4", 20, 20));
            Levels.Add(new Level("Level 5", 30, 25));
            Levels.Add(new Level("Level 6", 15, 10));
            Levels.Add(new BossLevel("Quarros", CreateBoss("Quarros")));
            Levels.Add(new Level("Level 7", 30, 10));
            Levels.Add(new Level("Level 8", 50, 15));
            Levels.Add(new Level("Level 9", 20, 5));
            Levels.Add(new BossLevel("Drake", CreateBoss("Drake")));
        }
        
        private void GurlowAttackEvents()
        {
            if (Gurlow.IsAttack != null)
            {
                Gurlow.TimeOfAttack++;
                if (Gurlow.IsAttack == "GurlowLaserAttack") // Removes GurlowLasers at the end of attack
                {
                    if (Gurlow.TimeOfAttack == Gurlow.MaxTimeOfAttack)
                    {
                        foreach (var x in AObject.Objects.Where(obj => obj is GurlowLaser))
                        {
                            x.ToRemove = true;
                        }
                        Gurlow.IsAttack = null;
                        Gurlow.TimeOfAttack = 0;
                    }
                } // Removes GurlowLasers at the end of attack
            }
        }
        private void QuarrosAttackEvents(Quarros quarros)
        {
            if (quarros.Hp <= 75 && quarros.IsSheildActive == false) // Activates QUarros sheild when his HP reaches 75
            {
                quarros.Attacks.RemoveAt(1);
                quarros.IsSheildActive = true;
                quarros.SheildHp = 75;
                quarros.HorizontalSpeed = quarros.InSheildHorizontalSpeed * Math.Sign(quarros.HorizontalSpeed);
                quarros.AttackSpeed = quarros.IntSheildAttackSpeed;
                Canvas.SetLeft(quarros.SheildModel, Canvas.GetLeft(quarros.Model));
                Canvas.SetTop(quarros.SheildModel, Canvas.GetTop(quarros.Model));
                MyCanvas.Children.Add(quarros.SheildModel);
            } // Activates QUarros sheild when his HP reaches 75
            if (Quarros.IsAttack == "QuarrosBeamAttack") // Uses QuarrosBeamAttack two more times to make a triple shot
            {
                Gurlow.TimeOfAttack++;
                if (Quarros.TimeOfAttack == 5 || Quarros.TimeOfAttack == 10)
                {
                    Quarros.QuarrosAttack2();
                }
                if (Quarros.TimeOfAttack == Quarros.MaxTimeOfAttack)
                {
                    Quarros.IsAttack = null;
                    Quarros.TimeOfAttack = 0;
                }
            }// Uses QuarrosBeamAttack two more times to make a triple shot
        }
        private void DrakeAttackEvents(Drake drake)
        {
            if (Drake.IsAttack == "DrakeSphereAttack") // Adds a little lightning animation to DrakeSphereAttack
            {
                Drake.TimeOfAttack++;
                if (Drake.TimeOfAttack == 3 || Drake.TimeOfAttack == 6)
                {
                    drake.ScaleTransform.ScaleY *= -1;
                }
                else if (Drake.TimeOfAttack == Drake.MaxTimeOfAttack)
                {
                    drake.ScaleTransform.ScaleY *= -1;
                    MyCanvas.Children.Remove(Drake.Lightning1);
                    MyCanvas.Children.Remove(Drake.Lightning2);
                    Drake.TimeOfAttack = 0;
                    Drake.IsAttack = null;
                }
            } // Adds a little lightning animation to DrakeSphereAttack
            else if (Drake.IsAttack == "DrakeLasersAttack") // Uses DrakeLasersAttack three more times to make a quadro shot
            {
                Drake.TimeOfAttack++;
                if (Drake.TimeOfAttack == 5 || Drake.TimeOfAttack == 10)
                {
                    Drake.DrakeLasersAttack();
                }
                else if (Drake.TimeOfAttack == Drake.MaxTimeOfAttack)
                {
                    Drake.DrakeLasersAttack();
                    Drake.TimeOfAttack = 0;
                    Drake.IsAttack = null;
                }
            }// Uses DrakeLasersAttack three more times to make a quadro shot
            else if (Drake.IsAttack == "DrakeMissilesAttack") // Uses DrakeMissilesAttack three more times to make a quadro shot
            {
                Drake.TimeOfAttack++;
                if (Drake.TimeOfAttack == 5 || Drake.TimeOfAttack == 10)
                {
                    Drake.DrakeMissilesAttack();
                }
                else if (Drake.TimeOfAttack == Drake.MaxTimeOfAttack)
                {
                    Drake.DrakeMissilesAttack();
                    Drake.TimeOfAttack = 0;
                    Drake.IsAttack = null;
                }
            }// Uses DrakeMissilesAttack three more times to make a quadro shot
        }
        private void AttackTimer_Tick(object sender, EventArgs e)// Working with all bosses special attacks
        {
            if (Levels[0] is BossLevel bossLevel)
            {
                try
                {
                    ((Boss)AObject.Objects.FirstOrDefault(x => x is Boss)).AttackCount--;
                    if (((Boss)AObject.Objects.FirstOrDefault(x => x is Boss)).AttackCount == 0) // Uses random boss attack
                    {
                        ((Boss)AObject.Objects.FirstOrDefault(x => x is Boss)).AttackCount = ((Boss)AObject.Objects.FirstOrDefault(x => x is Boss)).AttackSpeed;
                        BossAttack(bossLevel.Boss);
                    } // Uses random boss attack
                    if (bossLevel.Boss is Gurlow gurlow)
                    {
                        GurlowAttackEvents();
                    }
                    else if (bossLevel.Boss is Quarros quarros)
                    {
                        QuarrosAttackEvents(quarros);
                    }
                    else if (bossLevel.Boss is Drake drake)
                    {
                        DrakeAttackEvents(drake);
                    }
                }
                catch (Exception) { }
            }
        }

        private void PlayerSheildMoving()
        {
            --Player.ShieldTimer;
            if (!MyCanvas.Children.Contains(Player.ShieldModel))
            {
                MyCanvas.Children.Add(Player.ShieldModel);
            }
            if (Player.ShieldTimer < 30 && Player.ShieldTimer % 4 == 0) // Makes sheild to blink right before ending
            {
                MyCanvas.Children.Remove(Player.ShieldModel);
            } // Makes sheild to blink right before ending
            Canvas.SetLeft(Player.ShieldModel, Canvas.GetLeft(Player.Model));
            Canvas.SetTop(Player.ShieldModel, Canvas.GetTop(Player.Model));
        }
        private void BossHBoxMoving(Boss boss)
        {
            for (int i = 0; i < boss.HBox.Points.Count(); i++)
            {
                boss.HBox.Points[i] = new Point(boss.HBox.Points[i].X + boss.HorizontalSpeed, boss.HBox.Points[i].Y + boss.VerticalSpeed);
            }
        }
        private void ObjectReachesBottom(AObject obj)
        {
            if (obj is AEnemy en) // Respawn an enemy at the top and deal some dmg
            {
                Canvas.SetTop(en.Model, -100);
                Player.TakeDamage(en.Damage * 2);
            } // Respawn an enemy at the top and deal some dmg
            else
            {
                obj.ToRemove = true;
            }
        }
        private void CheckBulletForHit_Boss(ABullet bullet, Boss boss)
        {
            int X = (int)Canvas.GetLeft(bullet.Model);
            int Y = (int)Canvas.GetTop(bullet.Model);
            for (int i = 0; i < boss.HBox.Points.Count() - 1; i++)
            {
                int HBox_X = (int)boss.HBox.Points[i].X;
                int HBox_X2 = (int)boss.HBox.Points[i + 1].X;
                int HBox_Y = (int)boss.HBox.Points[i].Y;
                int HBox_Y2 = (int)boss.HBox.Points[i + 1].Y;
                if (X <= HBox_X && X >= HBox_X2 && Y < Math.Max(HBox_Y, HBox_Y2))
                {
                    double Tg = (double)(HBox_X - X) / (HBox_Y - Y);
                    double HBox_Tg = (double)(HBox_X - HBox_X2) / (HBox_Y - HBox_Y2);
                    if (Tg <= HBox_Tg || Y <= Math.Min(HBox_Y, HBox_Y2))
                    {
                        boss.TakeDamage(bullet.Damage);
                        bullet.ToRemove = true;
                    }
                }
            }
        }
        private void CheckBulletForHit(ABullet bullet)
        {
            foreach (AEnemy y in AObject.Objects.Where(obj => obj is AEnemy))
            {
                if (y.IsDead)
                    continue;
                if (y is Boss boss)// Hitting Boss with bullet
                {
                    CheckBulletForHit_Boss(bullet, boss);
                }// Hitting Boss with bullet
                else if (bullet.HitBox.IntersectsWith(y.HitBox))// Hitting regular enemy with bullet
                {
                    y.TakeDamage(bullet.Damage);
                    bullet.ToRemove = true;
                }// Hitting regular enemy with bullet
            }
        }
        private void PlayerIntersectionWithObjects(AObject obj)
        {
            if (obj is AEnemy enem) // With enemy
            {
                Player.TakeDamage(enem.Damage);
                enem.IsDead = true;
                enem.ToRemove = true;
            } // With enemy
            else if (obj is AProjectile projectile) // With projectile
            {
                Player.TakeDamage(projectile.Damage);
                if (projectile.Destructible)
                {
                    projectile.Lifetime = 0;
                    projectile.ToRemove = true;
                }
            }// With projectile
            else if (obj is ABonus bonus) // With bonus
            {
                bonus.Effect(Player);
                bonus.IsTaken = true;
                bonus.ToRemove = true;
            } // With bonus
        }
        private void WorkWithObjects(AObject obj)
        {
            Canvas.SetTop(obj.Model, Canvas.GetTop(obj.Model) + obj.VerticalSpeed);
            Canvas.SetLeft(obj.Model, Canvas.GetLeft(obj.Model) + obj.HorizontalSpeed);
            if (obj is Boss enemy) // Moving the boss complex HBox
            {
                BossHBoxMoving(enemy);
            } // Moving the boss complex HBox

            if (Canvas.GetLeft(obj.Model) <= 0 || Canvas.GetLeft(obj.Model) >= Application.Current.MainWindow.Width - obj.Model.Width) // Horizontal ricochet
            {
                obj.Angle = -obj.Angle;
            } // Horizontal ricochet

            if (Canvas.GetTop(obj.Model) > Canvas.GetTop(Player.Model) + 30 || Canvas.GetTop(obj.Model) < -150) // Object passes through thу bottom
            {
                ObjectReachesBottom(obj);
            } // Object passes through thу bottom

            if (obj is HomingMissile missile && Canvas.GetTop(obj.Model) < Canvas.GetTop(Player.Model) - 100) // Make HommingMissiles to aim to the Player model
            {
                missile.Angle = Math.Atan((Canvas.GetLeft(obj.Model) - Canvas.GetLeft(Player.Model) - (Player.Model.Width / 2)) / (Canvas.GetTop(obj.Model) - Canvas.GetTop(Player.Model)));
            }// Make HommingMissiles to aim to the Player model
            else if (obj is PowerSphere sphere && Canvas.GetTop(obj.Model) > Canvas.GetTop(Player.Model) - 350)// Make PowerSPhere to explode at the middle of the screen
            {
                sphere.ToRemove = true;
            }// Make PowerSPhere to explode at the middle of the screen

            obj.HitBox = new Rect(Canvas.GetLeft(obj.Model), Canvas.GetTop(obj.Model), obj.Model.Width, obj.Model.Height);
            if (obj is ABullet bullet) // Checking every bullet for hitting an enemy
            {
                CheckBulletForHit(bullet);
            } // Checking every bullet for hitting an enemy
            if (Player.HBox.IntersectsWith(obj.HitBox)) // Player intersection with objects 
            {
                PlayerIntersectionWithObjects(obj);
            } // Player intersection with objects 
        }
        private void MovingGurlowLasers(Gurlow gurlow)
        {
            foreach (GurlowLaser x in AObject.Objects.OfType<GurlowLaser>())
            {
                try
                {
                    Canvas.SetLeft(x.Model, Canvas.GetLeft(AObject.Objects.FirstOrDefault(y => y is Boss).Model) + x.Side + gurlow.Model.Width / 2 - 4);
                    Canvas.SetTop(x.Model, gurlow.Model.Height);
                }
                catch (Exception) { }
            }
        }
        private void MovingQuarrosSheild(Quarros quarros)
        {
            if (quarros.IsSheildActive == true && quarros.SheildHp != 0)
            {
                Canvas.SetLeft(quarros.SheildModel, Canvas.GetLeft(quarros.Model));
                Canvas.SetTop(quarros.SheildModel, Canvas.GetTop(quarros.Model));
            }
        }
        private void MovingDrakeLightnings(Drake drake)
        {
            Canvas.SetLeft(Drake.Lightning1, drake.HBox.Points[2].X - Drake.Lightning1.Width);
            Canvas.SetTop(Drake.Lightning1, drake.HBox.Points[2].Y - 10);
            Canvas.SetLeft(Drake.Lightning2, drake.HBox.Points[12].X);
            Canvas.SetTop(Drake.Lightning2, drake.HBox.Points[12].Y - 10);
        }
        private void MovingBossesSpecialObjects(BossLevel bossLevel)
        {
            if (bossLevel.Boss is Gurlow gurlow) // Moving GurlowsLasers
            {
                MovingGurlowLasers(gurlow);
            } // Moving GurlowsLasers
            else if (bossLevel.Boss is Quarros quarros) // Moving Quarroses sheild
            {
                MovingQuarrosSheild(quarros);
            } // Moving Quarroses sheild
            else if (bossLevel.Boss is Drake drake) // Moving Drakes lighnings
            {
                MovingDrakeLightnings(drake);
            } // Moving Drakes lighnings
        }
        private ABonus CreateRandomBonus()
        {
            int ChanceOfBonus = Rand.Next(1, 50);
            if (ChanceOfBonus < 5)
            {
                switch (ChanceOfBonus)
                {
                    case 1:
                        return new RepairBonus();
                    case 2:
                        return new ExplosionBonus();
                    case 3:
                        return new SheildBonus();
                    case 4:
                        return new MoneyBonus();
                    default:
                        return new MoneyBonus();
                }
            }
            return null;
        }
        private void RemoveEnemie(AEnemy enemy)
        {
            Player.Score++;
            if (enemy is Boss boss) // Stopping boss attacks
            {
                boss.AttackTimer.Tick -= AttackTimer_Tick;
                boss.AttackTimer.Stop();
                ItemRemover.Add(boss.HealthBar);
                MyCanvas.Children.Remove(boss.HBox);
            } // Stopping boss attacks
            ABonus NewBonus = CreateRandomBonus();
            if (NewBonus != null) // Spawning bonuses
            {
                AObject.Objects.Add(NewBonus);
                Canvas.SetLeft(NewBonus.Model, Canvas.GetLeft(enemy.Model) + enemy.Model.Width / 2);
                Canvas.SetTop(NewBonus.Model, Canvas.GetTop(enemy.Model));
                _ = MyCanvas.Children.Add(NewBonus.Model);
            } // Spawning bonuses
        }
        private void ExplodePowerSphere(PowerSphere sphere)
        {
            List<AProjectile> projectiles = new List<AProjectile>
            {
                new Fireball(0),
                new Beam(30),
                new Beam(-30),
                new Fireball(60),
                new Fireball(-60)
            };
            foreach (var proj in projectiles)
            {
                Canvas.SetLeft(proj.Model, Canvas.GetLeft(sphere.Model) + 20);
                Canvas.SetTop(proj.Model, Canvas.GetTop(sphere.Model) + 20);
                AObject.Objects.Add(proj);
                _ = MyCanvas.Children.Add(proj.Model);
            }
        }
        private void RemoveAllObjects()// Removing of objects
        {
            for (int i = AObject.Objects.Count - 1; i >= 0; i--)
            {
                if (AObject.Objects[i].ToRemove)
                {
                    MyCanvas.Children.Remove(AObject.Objects[i].Model);
                    if (AObject.Objects[i] is AEnemy enemy) // Deleting enemies
                    {
                        RemoveEnemie(enemy);
                    } // Deleting enemies
                    else if (AObject.Objects[i] is PowerSphere sphere) // Making PowerSphere to explode into 5 new projectiles
                    {
                        ExplodePowerSphere(sphere);
                    }// Making PowerSphere to explode into 5 new projectiles
                    AObject.Objects.RemoveAt(i);
                }
            }
        }
        private void GetToTheNextLvl()
        {
            if (Levels.Count == 1) // Finishing the game
            {
                _ = MessageBox.Show("Congratz, you destroyed all alien ships and saved the world!", "Chad");
                Application.Current.Shutdown();
                Environment.Exit(0);
            } // Finishing the game
            if (Levels[0] is BossLevel Level) // Opens SHOP after every bossLevel
            {
                for (int i = AObject.Objects.Count() - 1; i >= 0; i--) // Remove excess objects
                {
                    MyCanvas.Children.Remove(AObject.Objects[i].Model);
                    AObject.Objects.RemoveAt(i);
                } // Remove excess objects
                GameTimer.Stop();
                Shop.OpenShop(MyCanvas, Player, GameTimer);
            } // Opens SHOP after every bossLevel
            Levels.RemoveAt(0);
            EnemyCount = 0;
            EnemySpawnCounter = 100;
            LevelLabel.Content = Levels[0].LevelName;
            LevelStart = true;
        }
        private void GameOver()
        {
            GameTimer.Stop();
            if (Levels[0] is BossLevel bosslevel)
            {
                bosslevel.Boss.AttackTimer.Stop();
            }
            Player.HealthText.Foreground = Brushes.Red;
            MessageBox.Show("Lol you died!" + Environment.NewLine + "So, that was the last hope of mankind, huh?", "Loser");
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
        private void GameLoop(object sender, EventArgs e) // The main loop
        {
            Player.HBox = new Rect(Canvas.GetLeft(Player.Model), Canvas.GetTop(Player.Model), Player.Model.Width, Player.Model.Height);
            EnemySpawnCounter--;
            if (ExplosionBonus.IsActive) // Remove the model of the ExplosionBonus
            {
                MyCanvas.Children.Remove(ExplosionBonus.ExplosionModel);
                ExplosionBonus.IsActive = false;
            } // Remove the model of the ExplosionBonus
            if (LevelStart) // Shows level name at the screen when the level starts
            {
                Canvas.SetLeft(LevelLabel, 200);
                Canvas.SetTop(LevelLabel, 300);
                MyCanvas.Children.Add(LevelLabel);
                LevelStart = false;
            } // Shows level name at the screen when the level starts
            if (EnemySpawnCounter < 0 && EnemyCount < Levels[0].NumOfEnemies)// Spawning Enemies
            {
                if (EnemyCount == 0) // Removes the level name off screen when the first enemy spawnes
                {
                    MyCanvas.Children.Remove(LevelLabel);
                }// Removes the level name off screen when the first enemy spawnes
                EnemyCount++;
                MakeEnemies();
                EnemySpawnCounter = Levels[0].Limit;
            } // Spawning Enemies
            if (IsMoveLeft && Canvas.GetLeft(Player.Model) > 0) // Moving left
            {
                Canvas.SetLeft(Player.Model, Canvas.GetLeft(Player.Model) - Player.Speed);
            } // Moving left
            if (IsMoveRight && Canvas.GetLeft(Player.Model) < Application.Current.MainWindow.Width - 80)// Moving right
            {
                Canvas.SetLeft(Player.Model, Canvas.GetLeft(Player.Model) + Player.Speed);
            } // Moving right
            if (Player.ShieldTimer != 0) // Working with player sheild after catching the SheildBonus
            {
                PlayerSheildMoving();
            } // Working with player sheild after catching the SheildBonus
            foreach (AObject x in AObject.Objects) // Working with every Object in the map
            {
                WorkWithObjects(x);
            } // Working with every Object in the map
            if (Levels[0] is BossLevel bossLevel)
            {
                MovingBossesSpecialObjects(bossLevel);
            }// Enteraction with bosses special objects
            RemoveAllObjects();
            foreach (Rectangle item in ItemRemover)// ItemRemover
            {
                MyCanvas.Children.Remove(item);
            } // ItemRemover
            if (EnemyCount == Levels[0].NumOfEnemies && AObject.Objects.Where(obj => obj is AEnemy).Count() == 0) // Getting to the next level
            {
                GetToTheNextLvl();
            } // Getting to the next level
            if (Player.Health <= 0) // Losing the game
            {
                GameOver();
            } // Losing the game
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                IsMoveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                IsMoveRight = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
            if (e.Key == Key.Left)
            {
                IsMoveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                IsMoveRight = false;
            }
            if (e.Key == Key.Space)
            {
                Player.Attack();
            }
        }
        private void StartGame_OnClick(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Remove(StartGame);
            _ = MyCanvas.Children.Add(Player.ScoreText);
            _ = MyCanvas.Children.Add(Player.HealthText);
            GameTimer.Start();
            _ = MyCanvas.Focus();
        }

        private void BossAttack(Boss boss)
        {
            int Attack = Rand.Next(0, boss.Attacks.Count);
            boss.Attacks[Attack]();
        }

        private void SpawnMinion()
        {
            EnemySpriteCounter = Rand.Next(1, 5);
            AEnemy NewEnemy;
            switch (EnemySpriteCounter)
            {
                case 1:
                    NewEnemy = new FastEnemy();
                    break;
                case 2:
                    NewEnemy = new StrongEnemy();
                    break;
                case 3:
                    NewEnemy = new DangerousEnemy();
                    break;
                case 4:
                    NewEnemy = new StrafingEnemy();
                    break;
                default:
                    NewEnemy = new DangerousEnemy();
                    break;
            }
            AObject.Objects.Add(NewEnemy);
            Canvas.SetTop(AObject.Objects.Last().Model, -100);
            Canvas.SetLeft(AObject.Objects.Last().Model, Rand.Next(60, (int)Application.Current.MainWindow.Width) - 60);
            _ = MyCanvas.Children.Add(AObject.Objects.Last().Model);
        }

        private void SpawnBoss(BossLevel level)
        {
            AObject.Objects.Add(level.Boss);
            Canvas.SetTop(AObject.Objects.Last().Model, 0);
            Canvas.SetLeft(AObject.Objects.Last().Model, 260 - level.Boss.Model.Width / 2);
            _ = MyCanvas.Children.Add(AObject.Objects.Last().Model);
            _ = MyCanvas.Children.Add(((Boss)AObject.Objects.Last()).HBox);
            ((Boss)AObject.Objects.Last()).HealthBar.Width = 540;
            _ = MyCanvas.Children.Add(((Boss)AObject.Objects.Last()).HealthBar);
            ((Boss)AObject.Objects.Last()).AttackTimer.Interval = TimeSpan.FromMilliseconds(20);
            ((Boss)AObject.Objects.Last()).AttackTimer.Tick += AttackTimer_Tick;
            ((Boss)AObject.Objects.Last()).AttackTimer.Start();
        }
        private void MakeEnemies()
        {
            if (Levels[0] is BossLevel level)
            {
                SpawnBoss(level);
            }
            else
            {
                SpawnMinion();
            }
        }
    }
}
