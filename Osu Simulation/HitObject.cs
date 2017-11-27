using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_Simulation
{
    class HitObject
    {
        private int line;
        private int time;
        private char type;

        private int x = -20;
        private int y = -20;

        public int Line { get => line; }
        public int Time { get => time; }
        public char Type { get => type; }
        public int X { get => x; }
        public int Y { get => y; set => y = value; }

        public HitObject(int line, int time /*, char type */)
        {
            this.line = line;
            this.time = time;
            x = line * 70 + 80;
            // this.type = type;
        }
    }
}
