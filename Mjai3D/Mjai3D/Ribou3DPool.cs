using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    class Ribou3DPool
    {

        static int count;
        static List<Ribou3D> cache;

        public static void Initialize()
        {
            count = 0;
            cache = new List<Ribou3D>();
            for (int i = 0; i < 4; i++)
            {
                cache.Add(new Ribou3D());
            }
        }

        public static void Clear()
        {
            count = 0;
        }

        public static Ribou3D Get()
        {
            if (count >= cache.Count)
            {
                Ribou3D r = new Ribou3D();
                count++;
                cache.Add(r);
                return r;
            }
            else
            {
                return cache[count++];
            }
        }
    }
}
