using System.Numerics;

namespace ComputerGraphic
{
    internal class Composite : SceneObject
    {
        public Composite(string name) : base(name) { }
        public List<SceneObject> Objects { get; } = new();
        public override Trace Intersection(Ray ray, LightSource l)
        {
            Trace close = Objects[0].Intersection(ray, l);
            foreach (var obj in Objects)
            {
                Trace t = obj.Intersection(ray, l);
                if (t < close)
                    close = t;
            }
            return close;
        }
        public Trace TraceRay(Ray ray, LightSource l)
        {
            Trace close = Objects[0].Intersection(ray, l);
            foreach (var obj in Objects)
            {
                Trace t = obj.Intersection(ray, l);
                if (t < close)
                    close = t;
            }
            return close;
        }
        public override void Move(float dx, float dy, float dz)
        {
            foreach (var obj in Objects)
                obj.Move(dx, dy, dz);
        }
        public void Move(float dx, float dy, float dz, string name)
        {
            Objects.Find((SceneObject o) => o.name == name)?.Move(dx, dy, dz);
        }
        public override void Rotate(float ax, float ay, float az)
        {
            foreach (var obj in Objects)
                obj.Rotate(ax, ay, az);
        }
        public void Rotate(float ax, float ay, float az, string name)
        {
            Objects.Find((SceneObject o) => o.name == name)?.RotateC(ax, ay, az);
        }
        public override void Add(SceneObject o)
        {
            Objects.Add(o);
        }
        public override void RotateC(float ax, float ay, float az) { }
        public override void Remove(string name)
        {
            Objects.RemoveAll((SceneObject o) => o.name == name);
        }
        public override bool CastShadow(Trace t, Vector3 l)
        {
            bool f = false;
            foreach (var obj in Objects)
            {
                f = obj.CastShadow(t, l);
                if (f)
                    return f;
            }
            return f;
        }
    }
}
