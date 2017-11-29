using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_Simulation
{
    public class HitObjectHeap
    {
        public List<HitObject> list = new List<HitObject>();

        public void Add(HitObject value)
        {
            list.Add(value);
            
            int i = list.Count - 1;
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (list[parent] > list[i])
                {
                    Swap(parent, i);
                    i = parent;
                }
                else
                {
                    break;
                }
            }
        }

        public int Count { get => list.Count; }

        public HitObject RemoveOne()
        {
            HitObject root = list[0];
            
            list[0] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            
            int i = 0;
            int last = list.Count - 1;
            while (i < last)
            {
                int child = i * 2 + 1;
        
                if (child < last &&
                    list[child] > list[child + 1])
                    child = child + 1;
                
                if (child > last ||
                   list[i] <= list[child])
                    break;
                
                Swap(i, child);
                i = child;
            }

            return root;
        }

        private void Swap(int i, int j)
        {
            HitObject t = list[i];
            list[i] = list[j];
            list[j] = t;
        }
    }
}
