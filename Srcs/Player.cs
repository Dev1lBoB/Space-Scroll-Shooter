using Spice_Scroll_Shooter.Srcs.Weapons;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter
{
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
}
