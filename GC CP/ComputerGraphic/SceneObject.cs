using System.Numerics;

namespace ComputerGraphic
{
    [Serializable]
    internal abstract class SceneObject
    {
        protected SceneObject(string name)
        {
            this.name = name;
        }
        public string name;
        public abstract Trace Intersection(Ray ray, LightSource l);
        //public abstract Color? TraceRay(Ray ray, LightSource l, Smoke s);
        public abstract bool CastShadow(Trace t, Vector3 l);
        public virtual void Add(SceneObject o) { }
        public virtual void Remove(string name) { }
        public abstract void Move(float dx, float dy, float dz);
        public abstract void Rotate(float ax, float ay, float az);
        public abstract void RotateC(float ax, float ay, float az);

    }
}
