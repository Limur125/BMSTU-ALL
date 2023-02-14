using ComputerGraphic;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    class Query
    {
        public readonly bool last;
        public readonly int x, y;
        public Trace trace;
        public readonly int id;
        public TimeStamp[] times = new TimeStamp[8];
        public Query(int id, int x, int y, bool isLast = false)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            last = isLast;
        }
    }
    class TimeStamp
    {
        public string message;
        public long ts;
        public TimeStamp(string message, long ts)
        {
            this.message = message;
            this.ts = ts;
        }
    }
}


