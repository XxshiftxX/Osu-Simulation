using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_Simulation
{
    public class HitObject : IComparable
    {
        private int line;
        private int time;
        private char type;

        private int x = -20;
        private int y = -20;

        public int Line { get => line; }
        public char Type { get => type; }
        public int StartTime;
        public int X { get => x; }
        public int Y { get => y; set => y = value; }

        public HitObject(int line, int time /*, char type */)
        {
            this.line = line;
            x = line * 70 + 80;
            StartTime = time;
            // this.type = type;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(HitObject h1, HitObject h2)
            => h1.StartTime < h2.StartTime;

        public static bool operator >(HitObject h1, HitObject h2)
            => h1.StartTime > h2.StartTime;

        public static bool operator <=(HitObject h1, HitObject h2)
            => h1.StartTime <= h2.StartTime;

        public static bool operator >=(HitObject h1, HitObject h2)
            => h1.StartTime >= h2.StartTime;
    }

    public class Short : HitObject
    {
        public int Time;
        public Short(int line, int time) : base(line, time) {
            Time = time;
        }
    }

    public class Long : HitObject
    {
        public bool IsPressing { get; private set; }
        public int Start;
        public int End;

        public Long(int line, int start, int end) : base(line, start)
        {

        }


        public void StartPressing()
        {
            IsPressing = true;
        }

        public void EndPressing()
        {
            IsPressing = false;
        }
    }
}
