using Spice_Scroll_Shooter.Srcs.Weapons;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Spice_Scroll_Shooter
{
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
}
