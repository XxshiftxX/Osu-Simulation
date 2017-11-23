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
            Regex regex = new Regex("(/d+),(/d+),(/d+),(/d+),(/d+),(/d+):(/d+):(/d+):(/d+):");
            string allString = File.ReadAllText(filePath);
            System.Diagnostics.Debug.WriteLine(allString);
            System.Diagnostics.Debug.WriteLine(regex.Matches(allString)[0]);
        }
    }
}
