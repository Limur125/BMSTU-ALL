using System.Numerics;
using System;

namespace ComputerGraphic
{
    internal class LightSource
    {
        public LightSource(Vector3 pos) { position = pos; }
        public LightSource(Vector3 pos, Vector3 d) { position = pos; direction = d; }

        private Vector3 position;
        private Vector3 direction = Vector3.Zero;
        public Vector3 Position { get => position; private set => position = value; }
        public Vector3 Direction { get => direction; private set => direction = value; }
        public void Rotate(float ax, float ay, float az)
        {
            double x, y, z;
            y = (position.Y * Math.Cos(ax)) - (position.Z * Math.Sin(ax));
            z = (position.Y * Math.Sin(ax)) + (position.Z * Math.Cos(ax));
            position.Y = (float)y;
            position.Z = (float)z;
            x = (position.X * Math.Cos(az)) - (position.Y * Math.Sin(az));
            y = (position.X * Math.Sin(az)) + (position.Y * Math.Cos(az));
            position.X = (float)x;
            position.Y = (float)y;
            z = (position.Z * Math.Cos(ay)) - (position.X * Math.Sin(ay));
            x = (position.Z * Math.Sin(ay)) + (position.X * Math.Cos(ay));
            position.Z = (float)z;
            position.X = (float)x;
            y = (direction.Y * Math.Cos(ax)) - (direction.Z * Math.Sin(ax));
            z = (direction.Y * Math.Sin(ax)) + (direction.Z * Math.Cos(ax));
            direction.Y = (float)y;
            direction.Z = (float)z;
            x = (direction.X * Math.Cos(az)) - (direction.Y * Math.Sin(az));
            y = (direction.X * Math.Sin(az)) + (direction.Y * Math.Cos(az));
            direction.X = (float)x;
            direction.Y = (float)y;
            z = (direction.Z * Math.Cos(ay)) - (direction.X * Math.Sin(ay));
            x = (direction.Z * Math.Sin(ay)) + (direction.X * Math.Cos(ay));
            direction.Z = (float)z;
            direction.X = (float)x;
        }
        public void Move(float dx, float dy, float dz)
        {
            Vector3 o = new Vector3(dx, dy, dz);
            position += o;
        }
    }
}
