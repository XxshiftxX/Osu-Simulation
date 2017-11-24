using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Osu_Simulation
{
    partial class Game1
    {
        Stack<HitObject> HitObjects = new Stack<HitObject>();

        private void ReadOsuFile(string filePath)
        {
            string allString = File.ReadAllText(filePath);
            Regex regex = new Regex(@"(\d+),(\d+),(\d+),(\d+),(\d+),(\d+):(\d+):(\d+):(\d+):");
            MatchCollection matches = regex.Matches(allString);
            HitObjects.Clear();
            
            for(int i = matches.Count - 1; i >= 0; i--)
            {
                int line = 0, time;
                switch(int.Parse(matches[i].Groups[1].Value))
                {
                    case 64:
                        line = 0;
                        break;
                    case 192:
                        line = 1;
                        break;
                    case 320:
                        line = 2;
                        break;
                    case 448:
                        line = 3;
                        break;
                }
                time = int.Parse(matches[i].Groups[3].Value);

                HitObjects.Push(new HitObject(line, time));
            }
        }
    }
}
