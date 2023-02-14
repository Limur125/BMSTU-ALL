using System.Numerics;

namespace ComputerGraphic
{
    internal class Ray
    {
        public Ray(Vector3 direct, Vector3 start)
        {
            Direct = direct;
            Start = start;
        }

        public Vector3 Direct { get; set; }
        public Vector3 Start { get; set; }
    }
}
