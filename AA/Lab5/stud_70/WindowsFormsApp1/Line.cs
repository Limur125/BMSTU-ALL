using ComputerGraphic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using WindowsFormsApp1;
using System.Collections.Concurrent;

namespace WindowsFormsApp1
{
    enum Condition
    {
        Run,
        Empty,
        Finish
    };
    abstract class LineBase
    {
        protected Scene scene;
        protected ConcurrentQueue<Query> myQueue;
        protected Stopwatch start;
        protected LineBase nextLine;
        public LineBase(Scene scene, Stopwatch start)
        {
            this.start = start;
            this.scene = scene;
        }
        public void RunLine()
        {
            Condition result = Condition.Run;
            while (result != Condition.Finish)
                result = ProcessElem();
        }
        virtual protected Condition ProcessElem()
        {
            Query element = PopElem();
            if (element != null)
            {
                if (element.last)
                    return FinishLine(element);
                Query q = Action(element);
                nextLine.PushElem(q);
            }
            else
            {
                return Condition.Empty;
            }
            return Condition.Run;
        }
        abstract protected Query Action(Query arg);
        protected Condition FinishLine(Query arg)
        {
            nextLine.PushElem(arg);
            return Condition.Finish;
        }
        protected Query PopElem()
        {
            myQueue.TryDequeue(out Query query);
            return query;
        }
        protected void PushElem(Query arg)
        {
            myQueue.Enqueue(arg);
        }
    }
    class LineTraceRay : LineBase
    {
        public LineTraceRay(LineCastShadow lineCastShadow, List<Query> elements, Scene scene, Stopwatch start) : base(scene, start)
        {
            myQueue = new ConcurrentQueue<Query>(elements);
            nextLine = lineCastShadow;
        }

        override protected Query Action(Query arg)
        {
            arg.times[0] = new TimeStamp($"On line TraceRay element {arg.id} starts at", start.ElapsedTicks);
            arg.trace = scene.TraceRay(arg.x, arg.y);
            arg.times[1] = new TimeStamp($"On line TraceRay element {arg.id} finishes at", start.ElapsedTicks);
            return arg;
        }
    }
    class LineCastShadow : LineBase
    {
        public LineCastShadow(LineRenderSmoke lineRenderSmoke, Scene scene, Stopwatch start) : base(scene, start)
        {
            myQueue = new ConcurrentQueue<Query>();
            nextLine = lineRenderSmoke;
        }
        override protected Query Action(Query arg)
        {
            arg.times[2] = new TimeStamp($"On line CastShadow element {arg.id} starts at", start.ElapsedTicks);
            scene.CastShadow(arg.trace);
            arg.times[3] = new TimeStamp($"On line CastShadow element {arg.id} finishes at", start.ElapsedTicks);
            return arg;
        }
    }
    class LineRenderSmoke : LineBase
    {
        public LineRenderSmoke(LineSetPixel lineSetPixel, Scene scene, Stopwatch start) : base(scene, start)
        {
            myQueue = new ConcurrentQueue<Query>();
            nextLine = lineSetPixel;
        }
        override protected Query Action(Query arg)
        {
            arg.times[4] = new TimeStamp($"On line RenderSmoke element {arg.id} starts at", start.ElapsedTicks);
            scene.RenderSmoke(arg.trace);
            arg.times[5] = new TimeStamp($"On line RenderSmoke element {arg.id} finishes at", start.ElapsedTicks);
            return arg;
        }
    }
    class LineSetPixel : LineBase
    {
        public List<Query> result;
        public LineSetPixel(Scene scene, Stopwatch start) : base(scene, start) 
        {
            myQueue = new ConcurrentQueue<Query>();
            result = new List<Query>();
        }
        override protected Query Action(Query arg)
        {
            arg.times[6] = new TimeStamp($"On line SetPixel element {arg.id} starts at", start.ElapsedTicks);
            scene.SetPixel(arg.x, arg.y, arg.trace.Color);
            arg.times[7] = new TimeStamp($"On line SetPixel element {arg.id} finishes at", start.ElapsedTicks);
            return arg;
        }
        override protected Condition ProcessElem()
        {
            Query element = PopElem();
            if (element != null)
            {
                if (element.last)
                    return Condition.Finish;
                Action(element);
                result.Add(element);
            }
            else
            {
                return Condition.Empty;
            }
            return Condition.Run;
        }
    }

}
// Console.WriteLine($"On line TraceRay element {++counter} starts at {DateTime.Now.Subtract(start)}");
