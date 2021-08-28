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
using Spice_Scroll_Shooter.Srcs.Enemies;
using Spice_Scroll_Shooter.Srcs.Bonuses;
using Spice_Scroll_Shooter.Srcs.Projectiles;
using Spice_Scroll_Shooter.Srcs.Bullets;
using Spice_Scroll_Shooter.Srcs.Enemies.Bosses;

namespace Spice_Scroll_Shooter
{
    public delegate void Attack();
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

        private ABoss CreateBoss(string name)
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
            Levels.Add(new Level("Level 1", 30, 50));
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
                    ((ABoss)AObject.Objects.FirstOrDefault(x => x is ABoss)).AttackCount--;
                    if (((ABoss)AObject.Objects.FirstOrDefault(x => x is ABoss)).AttackCount == 0) // Uses random boss attack
                    {
                        ((ABoss)AObject.Objects.FirstOrDefault(x => x is ABoss)).AttackCount = ((ABoss)AObject.Objects.FirstOrDefault(x => x is ABoss)).AttackSpeed;
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
        private void BossHBoxMoving(ABoss boss)
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
        private void CheckBulletForHit_Boss(ABullet bullet, ABoss boss)
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
                if (y is ABoss boss)// Hitting Boss with bullet
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
            if (obj is ABoss enemy) // Moving the boss complex HBox
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
                    Canvas.SetLeft(x.Model, Canvas.GetLeft(AObject.Objects.FirstOrDefault(y => y is ABoss).Model) + x.Side + gurlow.Model.Width / 2 - 4);
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
            if (enemy is ABoss boss) // Stopping boss attacks
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

        private void BossAttack(ABoss boss)
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
            _ = MyCanvas.Children.Add(((ABoss)AObject.Objects.Last()).HBox);
            ((ABoss)AObject.Objects.Last()).HealthBar.Width = 540;
            _ = MyCanvas.Children.Add(((ABoss)AObject.Objects.Last()).HealthBar);
            ((ABoss)AObject.Objects.Last()).AttackTimer.Interval = TimeSpan.FromMilliseconds(20);
            ((ABoss)AObject.Objects.Last()).AttackTimer.Tick += AttackTimer_Tick;
            ((ABoss)AObject.Objects.Last()).AttackTimer.Start();
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
