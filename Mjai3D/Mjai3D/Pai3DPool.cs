using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    static class Pai3DPool
    {
        static int[] count;
        static List<Pai3D>[] cache;

        public static void Initialize() {
            count = new int[35];
            cache = new List<Pai3D>[35];
            for (int i = 0; i < 35; i++)
            {
                count[i] = 0;
                cache[i] = new List<Pai3D>();
                for (int j = 0; j < 30; j++)
                {
                    cache[i].Add(new Pai3D(Pai.FromInt34(i)));
                }
            }
        }

        public static void Clear() {
            for(int i = 0; i < 35; i++) count[i] = 0;
        }

        public static Pai3D Get(Pai pai) {
            int x = pai.ToInt34();
            if (count[x] >= cache[x].Count)
            {
                Pai3D p = new Pai3D(pai);
                count[x]++;
                cache[x].Add(p);
                return p;
            }
            else
            {
                return cache[x][count[x]++];
            }
        }
    }
}
