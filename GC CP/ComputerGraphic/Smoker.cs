using System.Numerics;

namespace ComputerGraphic
{
    [Serializable]
    internal class Smoker
    {
        private Vector3 position;
        private int intensity;
        private float particleSize;
        private Color particleColor;
        private Smoke smoke;
        public Smoker(Smoke smoke, Vector3 position, int intensity, float particleSize, Color particleColor)
        {
            this.smoke = smoke;
            this.position = position;
            this.intensity = intensity;
            this.particleSize = particleSize;
            this.particleColor = particleColor;
        }
        public void Run(TimeSpan t)
        {
            double dtime = 1.0 / intensity;
            for (double i = 0; i < t.TotalSeconds; i += dtime)
            {
                Particle p = new(position, particleSize, 1000 * 20, particleColor);
                smoke.Add(p);
                smoke.Update(dtime, position);
            }
        }
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
