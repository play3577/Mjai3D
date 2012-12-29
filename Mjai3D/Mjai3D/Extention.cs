using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    static class ListExtention
    {
        public static void Pop<T>(this List<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static List<T> Removed<T>(this List<T> list, T value)
        {
            var list_ = list.ToList();
            list_.Remove(value);
            return list_;
        }
    }
}
