using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    public enum Suit
    {
        Manzu = 0, Pinzu = 1, Sozu = 2, Jihai = 3
    }

    public struct Pai : IComparable
    {

        public Suit Suit;
        public int Num;
        public bool Red;

        public Pai(Suit suit, int num, bool red = false)
        {
            Suit = suit;
            Num = num;
            Red = red;
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Suit == Suit.Jihai) sb.Append("-ESWNPFC?"[Num]);
            else
            {
                sb.Append(Num);
                sb.Append("mps"[(int)Suit]);
                if (Red) sb.Append("r");
            }
            return sb.ToString();
        }

        public int ToInt34()
        {
            return (int)Suit * 9 + Num - 1;
        }

        public Pai AddRed()
        {
            return new Pai(Suit, Num, true);
        }

        public Pai RemoveRed()
        {
            return new Pai(Suit, Num, false);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Pai pai = (Pai)obj;
            if (Suit != pai.Suit) return Suit - pai.Suit;
            if (Num != pai.Num) return Num - pai.Num;
            if (Red != pai.Red) return Red ? 1 : -1;
            return 0;
        }

        public static Pai operator +(Pai pai, int x)
        {
            int sup = (pai.Suit == Suit.Jihai) ? 7 : 9;
            if (pai.Num + x > sup) throw new ArgumentException();
            return new Pai(pai.Suit, pai.Num + x, pai.Red);
        }

        public static Pai operator -(Pai pai, int x)
        {
            if (pai.Num - x <= 0) throw new ArgumentException();
            return new Pai(pai.Suit, pai.Num - x, pai.Red);
        }

        public static bool operator ==(Pai p1, Pai p2)
        {
            if (p1 == null || p2 == null) return false;
            return p1.Suit == p2.Suit && p1.Num == p2.Num && p1.Red == p2.Red;
        }

        public static bool operator !=(Pai p1, Pai p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Pai pai = (Pai)obj;
            if (pai == null) return false;
            return Suit == pai.Suit && Num == pai.Num && Red == pai.Red;
        }

        public override int GetHashCode()
        {
            return (int)Suit * 10 + Num + (Red ? 100 : 0);
        }

        public static bool operator <(Pai p1, Pai p2)
        {
            if (p1.Suit != p2.Suit) return p1.Suit < p2.Suit;
            if (p1.Num != p2.Num) return p1.Num < p2.Num;
            return !p1.Red;
        }

        public static bool operator >(Pai p1, Pai p2)
        {
            if (p1.Suit != p2.Suit) return p1.Suit > p2.Suit;
            if (p1.Num != p2.Num) return p1.Num > p2.Num;
            return p1.Red;
        }

        public static Pai FromInt37(int n)
        {
            Suit suit = (Suit)(n / 10);
            int num = n % 10;

            if (num == 0) // red
            {
                return new Pai(suit, 5, true);
            }
            else
            {
                return new Pai(suit, num);
            }
        }

        public static Pai FromString(string s)
        {
            int len = s.Length;
            if (len == 1)
            {
                return new Pai(Suit.Jihai, "-ESWNPFC?".IndexOf(s[0]));
            }
            else if (len == 2)
            {
                return new Pai((Suit)"mps".IndexOf(s[1]), (s[0] - '0'));
            }
            else if (len == 3 && s[0] == '5' && s[2] == 'r')
            {
                return new Pai((Suit)"mps".IndexOf(s[1]), 5, true);
            }
            else
            {
                throw new ArgumentException();
            }
        }


        internal static Pai FromInt34(int x)
        {
            if (x < 0 || x >= 35) throw new ArgumentException();
            return new Pai((Suit)(x / 9), x % 9 + 1);
        }
    }
}
