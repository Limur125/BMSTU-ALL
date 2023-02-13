using System.Numerics;

namespace ComputerGraphic
{
    internal abstract class SceneObject
    {
        public abstract Trace Intersection(Ray ray, LightSource l);
        //public abstract Color? TraceRay(Ray ray, LightSource l, Smoke s);
        public abstract bool CastShadow(Trace t, Vector3 l);
        public virtual void Add(SceneObject o) { }
        public virtual void Remove(SceneObject o) { }
        public abstract void Move(float dx, float dy, float dz);
        public abstract void Rotate(float ax, float ay, float az);
    }
}
