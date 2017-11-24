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

        public int Line { get => line; }
        public int Time { get => time; }
        public char Type { get => type; }

        public HitObject(int line, int time /*, char type */)
        {
            this.line = line;
            this.time = time;
            // this.type = type;
        }
    }
}
