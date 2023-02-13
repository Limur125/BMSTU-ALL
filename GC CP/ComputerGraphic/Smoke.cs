using System.Numerics;

namespace ComputerGraphic
{
    [Serializable]
    internal class Smoke
    {
        private List<Particle> particles;
        public Color ParticleColor
        {
            set
            {
                foreach(var particle in particles)
                    particle.color = value;
            }
        }
        public Smoke()
        {
            particles = new();
        }
        public void Add(Particle p)
        {
            particles.Add(p);
        }
        public void Update(double dtime, Vector3 smokerPosition)
        { 
            List<int> l = new();
            int listCount = particles.Count;
            for (int i = 0; i < listCount; i++)
            {
                bool f = particles[i].Update(dtime, smokerPosition);
                if (f)
                    l.Add(i);
            }

            for (int j = l.Count - 1; j > 0; j--)
                particles.RemoveAt(l[j]);
        }
        public Color Intersection(Vector3 p1, Vector3 p2, Color c)
        {
            foreach (var particle in particles)
                c = particle.Intersection(p1, p2, c);
            return c;
        }
        public Color CastShadow(Vector3 t, Vector3 l, Color tc)
        {
            float m = 1;
            foreach (var particle in particles)
                m *= particle.CastShadow(t, l, tc);
            if (m < 0.3f)
                m = 0.3f;
            return Color.FromArgb((int)(tc.R * m), (int)(tc.G * m), (int)(tc.B * m));
        }
        public void EnableShadowedParticles(Vector3 t, Vector3 l)
        {
            foreach (Particle particle in particles)
                particle.Enable(t, l);
        }
        public void Move(float dx, float dy, float dz)
        {
            foreach (var particle in particles)
                particle.Move(dx, dy, dz);
        }
        public void Rotate(float ax, float ay, float az)
        {
            foreach (var particle in particles)
                particle.Rotate(ax, ay, az);
        }

    }
}
