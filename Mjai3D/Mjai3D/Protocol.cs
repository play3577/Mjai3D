using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    static class Protocol
    {
        public static object none()
        {
            return new { type = "none" };
        }

        public static object join(string name, string room)
        {
            return new { type = "join", name = name, room = room };
        }

        public static object hora(int actor, int target, Pai pai)
        {
            return new { type = "hora", actor = actor, target = target, pai = pai.ToString() };
        }

        public static object dahai(int actor, Pai pai, bool tsumogiri)
        {
            return new { type = "dahai", actor = actor, pai = pai.ToString(), tsumogiri = tsumogiri };
        }

        public static object reach(int actor)
        {
            return new { type = "reach", actor = actor };
        }

        public static object pon(int actor, int target, Pai pai, List<Pai> consumed)
        {
            return naki("pon", actor, target, pai, consumed);
        }

        public static object kan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            return naki("kan", actor, target, pai, consumed);
        }

        public static object chi(int actor, int target, Pai pai, List<Pai> consumed)
        {
            return naki("chi", actor, target, pai, consumed);
        }

        static object naki(string type, int actor, int target, Pai pai, List<Pai> consumed)
        {
            consumed.Sort();
            return new { type = type, actor = actor, target = target, pai = pai.ToString(), consumed = consumed.Select(p => p.ToString()).ToList() };
        }
    }
}
