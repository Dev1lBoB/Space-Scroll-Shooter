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
    public class Drake : ABoss
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
            Canvas.SetLeft(Sphere.Model, ((ABoss)AObject.Objects.FirstOrDefault(obj => obj is Drake)).HBox.Points[7].X - 32);
            Canvas.SetTop(Sphere.Model, ((ABoss)AObject.Objects.FirstOrDefault(obj => obj is Drake)).HBox.Points[7].Y - 10);
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
}
