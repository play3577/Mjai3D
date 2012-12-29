using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Mjai3D
{
    static class Algorithm
    {

        private static Dictionary<int, List<Tuple<int, int>>> SHANTEN_TBL;

        public static void Initialize()
        {
            SHANTEN_TBL = new Dictionary<int, List<Tuple<int, int>>>();

            foreach (string line in File.ReadAllLines("../shanten.txt").Skip(1))
            {
                int[] tmp = line.Split('\t').Select(Int32.Parse).ToArray<int>();

                var key = tmp[0];
                var v = tmp[1];

                SHANTEN_TBL[key] = new List<Tuple<int, int>>(){ 
                    Tuple.Create(v/1000%10, v/100%10), 
                    Tuple.Create(v/10%10, v%10)
                };
            }
        }

        static int calcHash(int[] t)
        {
            return t.Aggregate(0, ((acc, x) => acc * 10 + x));
        }

        static void removeKoritsu(int[] t)
        {
            for (int i = 0; i < 9; i++)
            {
                if ((i - 2 < 0 || t[i - 2] == 0) &&
                   (i - 1 < 0 || t[i - 1] == 0) &&
                   (t[i] == 1) &&
                   (i + 1 > 8 || t[i + 1] == 0) &&
                   (i + 2 > 8 || t[i + 2] == 0))
                {
                    t[i] = 0;
                }
            }
        }

        static int pickMenta(int[] t, int needMentsu)
        {
            var mts = new List<List<Tuple<int, int>>>();
            for (int i = 0; i < 3; i++)
            {
                int[] tt = t.Skip(i * 9).Take(9).ToArray<int>();
                removeKoritsu(tt);
                var h = calcHash(tt);
                mts.Add((h == 0) ? new List<Tuple<int, int>>() { Tuple.Create(0, 0), Tuple.Create(0, 0) } : SHANTEN_TBL[h]);
            }

            var mJihai = t.Skip(27).Take(7).Count(_ => _ >= 3);
            var tJihai = t.Skip(27).Take(7).Count(_ => _ == 2);

            int ans = Int32.MaxValue;

            for (int i = 0; i < (1 << 3); i++)
            {
                var mts_ = Enumerable.Range(0, 3).Select(j => mts[j][i >> j & 1]);
                var m = mts_.Select(_ => _.Item1).Sum() + mJihai;
                var a = Math.Min(mts_.Select(_ => _.Item2).Sum() + tJihai, needMentsu - m);
                ans = Math.Min(ans, 8 - 2 * m - a);
            }
            return ans;
        }

        static int normalShanten(int[] t, int needMentsu)
        {
            int ans = 10;
            for (int i = 0; i < 34; i++)
            {
                if (t[i] >= 2)
                {
                    t[i] -= 2;
                    ans = Math.Min(ans, pickMenta(t, needMentsu) - 1);
                    t[i] += 2;
                }
            }
            return Math.Min(ans, pickMenta(t, needMentsu)) - (4 - needMentsu) * 2;
        }

        static int kokushiShanten(int[] t)
        {
            var yaochu = new List<int>() { 0, 8, 9, 17, 18, 26 }.Concat(Enumerable.Range(27, 7));
            var kind = yaochu.Count(_ => t[_] >= 1);
            return 13 - (kind + (yaochu.Any(_ => t[_] >= 2) ? 1 : 0));
        }

        static int chitoiShanten(int[] t)
        {
            var toitsu = t.Count(_ => _ >= 2);
            var kind = t.Count(_ => _ >= 1);
            return 6 - toitsu + Math.Max(7 - kind, 0);
        }

        public static int shanten(IEnumerable<Pai> tehai)
        {
            return shanten(tehai.Select(p => p.ToInt34()));
        }

        private static int shanten(IEnumerable<int> tehai)
        {
            int[] t = new int[34];
            for (int i = 0; i < 34; i++) t[i] = 0;
            foreach (int pai in tehai)
            {
                t[pai]++;
            }
            int needMentsu = tehai.Count() / 3;
            if (needMentsu == 4)
            {
                var tmp = new List<int>() { normalShanten(t, needMentsu), kokushiShanten(t), chitoiShanten(t) };
                return tmp.Min();
            }
            else
                return normalShanten(t, needMentsu);
        }

        public static bool canChi(IEnumerable<Pai> tehai, Pai pai)
        {
            if (pai.Suit == Suit.Jihai) return false;

            tehai = tehai.Select(p => p.RemoveRed());
            pai = pai.RemoveRed();

            if (pai.Num >= 3 && tehai.Contains(pai - 2) && tehai.Contains(pai - 1)) return true;
            if (pai.Num >= 2 && pai.Num <= 8 && tehai.Contains(pai - 1) && tehai.Contains(pai + 1)) return true;
            if (pai.Num <= 7 && tehai.Contains(pai + 1) && tehai.Contains(pai + 2)) return true;
            return false;
        }

        public static bool canPon(IEnumerable<Pai> tehai, Pai pai)
        {
            tehai = tehai.Select(p => p.RemoveRed());
            pai = pai.RemoveRed();
            return tehai.Count(p => p == pai) >= 2;
        }

        public static bool canKan(IEnumerable<Pai> tehai, Pai pai)
        {
            tehai = tehai.Select(p => p.RemoveRed());
            pai = pai.RemoveRed();
            return tehai.Count(p => p == pai) >= 3;
        }

        public static bool canKan(IEnumerable<Pai> tehai)
        {
            Dictionary<Pai, int> cnt = new Dictionary<Pai, int>();
            foreach (var pai in tehai)
                cnt[pai] = (cnt.ContainsKey(pai) ? cnt[pai] : 0) + 1;
            return cnt.Any(item => item.Value >= 4);
        }

        public static bool canRon(IEnumerable<Pai> tehai, Pai pai)
        {
            List<Pai> tmp = tehai.ToList();
            tmp.Add(pai);
            return shanten(tmp) == -1;
        }

        public static IEnumerable<List<Pai>> allChiCandidates(IEnumerable<Pai> tehai, Pai pai)
        {
            List<List<Pai>> ans = new List<List<Pai>>();
            foreach (var p1 in tehai)
            {
                foreach (var p2 in tehai)
                {
                    List<Pai> tmp = new List<Pai>() { pai, p1, p2 };
                    tmp.Sort();
                    if (tmp[0].Num <= 7 && tmp[1].Num <= 8 && tmp[0] + 1 == tmp[1] && tmp[1] + 1 == tmp[2])
                    {
                        ans.Add((p1 < p2) ? new List<Pai>() { p1, p2 } : new List<Pai>() { p2, p1 });
                    }
                }
            }
            return ans.Distinct();
        }

    }
}
