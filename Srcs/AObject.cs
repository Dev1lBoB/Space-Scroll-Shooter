using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spice_Scroll_Shooter
{
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
}
