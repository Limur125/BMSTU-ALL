using System.Numerics;

namespace ComputerGraphic
{
    [Serializable]
    internal class LightSource
    {
        public LightSource(Vector3 pos) { position = pos; }
        public LightSource(Vector3 pos, float intensity) { position = pos; intens = intensity; }
        private Vector3 position;
        public float intens = 1;
        public Vector3 Position { get => position; private set => position = value; }
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
        }
        public void Move(float dx, float dy, float dz)
        {
            Vector3 o = new(dx, dy, dz);
            position += o;
        }
    }
}
