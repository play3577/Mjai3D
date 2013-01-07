using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using Wistery.Majong;

namespace Mjai3D
{
    public class XnaGuiAI : AI
    {
        Game1 window;

        List<Component> components;
        MajongComponent majong;

        int id { get { return majong.Id; } }
        bool reach { get { return majong.Reaches[majong.Id] != -1; } }
        List<Pai> tehai { get { return majong.Tehais[majong.Id]; } }
        bool menzen { get { return !majong.Furos[majong.Id].Any(); } }
        int score { get { return majong.Scores[majong.Id]; } }

        public XnaGuiAI(Game1 window)
        {
            components = new List<Component>();
            this.window = window;
            components.Add(window);
            majong = new MajongComponent();
            components.Add(majong);
        }

        public object onStartGame(int id, List<string> names)
        {
            foreach(var component in components)
                component.onStartGame(id, names);
            return Protocol.none();
        }

        public object onStartKyoku(Pai bakaze, int kyoku, int honba, int kyotaku, int oya, Pai doraMarker, List<List<Pai>> tehais)
        {
            foreach (var component in components)
                component.onStartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);

            return Protocol.none();
        }

        public object onTsumo(int actor, Pai pai)
        {
            foreach (var component in components)
                component.onTsumo(actor, pai);

            if (actor != id)
            {
                return Protocol.none();
            }
            else if (reach)
            {
                return Protocol.dahai(id, pai, true);
            }
            else
            {
                List<SelectionType> alternatives = new List<SelectionType>();

                if (menzen && Algorithm.shanten(tehai) <= 0)
                {
                    alternatives.Add(SelectionType.Reach);
                }
                if (Algorithm.shanten(tehai) == -1)
                {
                    alternatives.Add(SelectionType.Tsumo);
                }
                if (Algorithm.canKan(tehai))
                {
                    alternatives.Add(SelectionType.Kan);
                }

                return window.Select1(alternatives).match<object>(
                    (dahai, tsumogiri) =>
                    {
                        if (!tehai.Contains(dahai)) // 消極的対処
                            return Protocol.dahai(id, pai, true);
                        else
                            return Protocol.dahai(id, dahai, tsumogiri);
                    },
                    (kanPai) =>
                    {
                        throw new NotImplementedException("XnaGuiAI: Ankan is not yet implemented.");
                    },
                    (kanPai) => {
                        throw new NotImplementedException("XnaGuiAI: Kakan is not yet implemented.");
                    },
                    () =>
                    {
                        if (score >= 1000)
                            return Protocol.reach(id);
                        else
                            return Protocol.dahai(id, pai, true);
                    },
                    () => 
                        Protocol.hora(id, id, pai)
                );
            }
        }

        public object onDahai(int actor, Pai pai, bool tsumogiri)
        {
            foreach (var component in components)
                component.onDahai(actor, pai, tsumogiri);

            if (actor == id)
            {
                tehai.Remove(pai);
                return Protocol.none();
            }
            else
            {
                List<SelectionType> alternatives = new List<SelectionType>();

                if ((actor + 1) % 4 == id && Algorithm.canChi(tehai, pai))
                    alternatives.Add(SelectionType.Chi);
                if (Algorithm.canPon(tehai, pai))
                {
                    Console.WriteLine(string.Join(", ", tehai.Select(p => p.ToString()).ToArray()));
                    alternatives.Add(SelectionType.Pon);
                }
                if (Algorithm.canKan(tehai, pai))
                    alternatives.Add(SelectionType.Kan);
                if (Algorithm.canRon(tehai, pai))
                    alternatives.Add(SelectionType.Ron);

                if (!alternatives.Any())
                {
                    return Protocol.none();
                }
                else
                {
                    alternatives.Add(SelectionType.Pass);

                    return window.Select2(alternatives).match<object>(
                        (pai1, pai2) =>
                            Protocol.chi(id, actor, pai, new List<Pai> { pai1, pai2 })
                        ,
                        (pai1, pai2) =>
                            Protocol.pon(id, actor, pai, new List<Pai> { pai1, pai2 })
                        ,
                        () =>
                            Protocol.kan(id, actor, pai, tehai.FindAll(p => p.RemoveRed() == pai.RemoveRed()))
                        ,
                        () =>
                            Protocol.hora(id, actor, pai)
                        ,
                        () =>
                            Protocol.none()
                    );
                }
            }
        }

        public object onReach(int actor)
        {
            foreach (var component in components)
                component.onReach(actor);

            if (actor != id)
            {
                return Protocol.none();
            }
            else
            {
                object res;
                while (true)
                {
                    res = window.Select1(new List<SelectionType>()).match<object>(
                        (pai, tsumogiri) =>
                        {
                            if (Algorithm.shanten(tehai.Removed(pai)) != 0)
                                return null;
                            else
                                return Protocol.dahai(id, pai, tsumogiri);
                        },
                        (pai) =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        (pai) =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        () =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        () =>
                        {
                            Debug.Assert(false);
                            return null;
                        }
                    );
                    if (res != null) break;
                }
                return res;
            }
        }

        public object onReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            foreach (var component in components)
                component.onReachAccepted(actor, deltas, scores);

            return Protocol.none();
        }

        object onNaki(int actor, int target, Pai pai, List<Pai> consumed)
        {
            if (actor != id)
            {
                return Protocol.none();
            }
            else
            {
                object res;
                while (true)
                {
                    res = window.Select1(new List<SelectionType>()).match<object>(
                        (pai2, tsumogiri) =>
                            Protocol.dahai(id, pai2, false) // TODO: 喰い替えチェック
                        ,
                        (pai2) =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        (pai2) =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        () =>
                        {
                            Debug.Assert(false);
                            return null;
                        },
                        () =>
                        {
                            Debug.Assert(false);
                            return null;
                        }
                    );
                    if (res != null) break;
                }
                return res;
            }
        }

        public object onPon(int actor, int target, Pai pai, List<Pai> consumed)
        {
            foreach (var component in components)
                component.onPon(actor, target, pai, consumed);

            return onNaki(actor, target, pai, consumed);
        }

        public object onChi(int actor, int target, Pai pai, List<Pai> consumed)
        {
            foreach (var component in components)
                component.onChi(actor, target, pai, consumed);

            return onNaki(actor, target, pai, consumed);
        }

        public object onKan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            foreach (var component in components)
                component.onKan(actor, target, pai, consumed);

            return onNaki(actor, target, pai, consumed);
        }

        public object onHora(int actor, int target, Pai pai, List<Pai> horaTehais, List<YakuN> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            foreach (var component in components)
                component.onHora(actor, target, pai, horaTehais, yakus, fu, fan, horaPoints, deltas, scores);

            return Protocol.none();
        }

        public object onRyukyoku(string reason, List<List<Pai>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            foreach(var component in components)
                component.onRyukyoku(reason, tehais, tenpais, deltas, scores);
            
            return Protocol.none();
        }

        public object onEndKyoku()
        {
            foreach (var component in components)
                component.onEndKyoku();

            return Protocol.none();
        }

        public void onError(string message)
        {
            window.onError(message);
        }
    }
}
