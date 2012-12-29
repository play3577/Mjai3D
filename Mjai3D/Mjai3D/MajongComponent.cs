using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    public enum FuroType
    {
        Chi, Pon, Ankan, Daiminkan, Kakan
    }

    public struct Furo
    {
        public FuroType type;
        public int actor;
        public int target;
        public Pai pai;
        public List<Pai> consumed;

        public Furo(FuroType type, int actor, int target, Pai pai, List<Pai> consumed)
        {
            this.type = type;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }

    public struct Sutehai
    {
        public Pai pai;
        public bool tsumogiri;
        public Sutehai(Pai pai, bool tsumogiri)
        {
            this.pai = pai;
            this.tsumogiri = tsumogiri;
        }
    }

    class MajongComponent : Component
    {

        #region Properties

        public int Id
        {
            get;
            private set;
        }

        public List<Pai>[] Tehais
        {
            get;
            private set;
        }

        public List<Sutehai>[] Kawas
        {
            get;
            private set;
        }

        public List<Furo>[] Furos
        {
            get;
            private set;
        }

        public List<int> Scores
        {
            get;
            private set;
        }

        public List<int> Reaches
        {
            get;
            private set;
        }

        public List<string> Names
        {
            get;
            private set;
        }

        public Pai Bakaze
        {
            get;
            private set;
        }

        public int Kyoku
        {
            get;
            private set;
        }

        public int Honba
        {
            get;
            private set;
        }

        public int Kyotaku
        {
            get;
            private set;
        }

        public int Oya
        {
            get;
            private set;
        }

        public Pai DoraMarker
        {
            get;
            private set;
        }

        #endregion

        public MajongComponent()
        {
            Id = 0;
            Tehais = new List<Pai>[4];
            Kawas = new List<Sutehai>[4];
            Furos = new List<Furo>[4];
            for (int i = 0; i < 4; i++)
            {
                Tehais[i] = new List<Pai>();
                Kawas[i] = new List<Sutehai>();
                Furos[i] = new List<Furo>();
            }
            Scores = new List<int>(Enumerable.Repeat(25000, 4));
            Reaches = new List<int>(Enumerable.Repeat(-1, 4));
            Names = new List<string>(Enumerable.Repeat("", 4));
            Bakaze = Pais.Ton;
            Kyoku = 0;
            Honba = 0;
            Oya = 0;
            DoraMarker = Pais.Man1;

        }

        public void onStartGame(int id, List<string> names)
        {
            Id = id;
            Names = names.ToList();
        }

        public void onStartKyoku(Pai bakaze, int kyoku, int honba, int kyotaku, int oya, Pai doraMarker, List<List<Pai>> tehais)
        {
            Bakaze = bakaze;
            Kyoku = kyoku;
            Honba = honba;
            Kyotaku = kyotaku;
            Oya = oya;
            DoraMarker = doraMarker;

            Reaches = new List<int>(Enumerable.Repeat(-1, 4));
            for (int i = 0; i < 4; i++)
            {
                Tehais[i] = tehais[i].ToList();
                Furos[i] = new List<Furo>();
                Kawas[i] = new List<Sutehai>();
            }

        }

        public void onTsumo(int actor, Pai pai)
        {
            Console.WriteLine("onTsumo: actor = {0}, pai = {1}", actor.ToString(), pai.ToString());
            Console.WriteLine(Tehais[Id].Count);
            Tehais[actor].Add(pai);
            Console.WriteLine(Tehais[Id].Count);
        }

        public void onDahai(int actor, Pai pai, bool tsumogiri)
        {
            Kawas[actor].Add(new Sutehai(pai, tsumogiri));

            if (actor == Id)
            {
                Tehais[Id].Remove(pai);
                Tehais[Id].Sort();
            }
            else
                Tehais[actor].Remove(Pais.Unknown);
        }

        public void onReach(int actor)
        {
        }

        public void onReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            Scores = scores.ToList();
            Reaches[actor] = Kawas[actor].Count - 1; // CAN BE BUG. もしリーチ宣言牌が鳴かれたらchiとreach_acceptedどっちが先？ これは、reach_acceptedが先の想定。
            Kyotaku++;
        }

        public void onPon(int actor, int target, Pai pai, List<Pai> consumed)
        {
            Kawas[target].Pop();

            foreach (Pai c in consumed)
            {
                if (actor == Id)
                    Tehais[Id].Remove(c);
                else
                    Tehais[actor].Remove(Pais.Unknown);
            }
            
            Furos[actor].Add(new Furo(FuroType.Pon, actor, target, pai, consumed));
        }

        public void onChi(int actor, int target, Pai pai, List<Pai> consumed)
        {
            Kawas[target].Pop();

            foreach (Pai c in consumed)
            {
                if (actor == Id)
                    Tehais[Id].Remove(c);
                else
                    Tehais[actor].Remove(Pais.Unknown);
            }

            Furos[actor].Add(new Furo(FuroType.Chi, actor, target, pai, consumed));
        }

        public void onKan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            Kawas[target].Pop();

            foreach (Pai c in consumed)
            {
                if (actor == Id)
                    Tehais[Id].Remove(c);
                else
                    Tehais[actor].Remove(Pais.Unknown);
            }

            Furos[actor].Add(new Furo(FuroType.Pon, actor, target, pai, consumed));
        }

        public void onHora(int actor, int target, Pai pai, List<Pai> horaTehais, List<YakuN> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            Scores = scores;
        }

        public void onRyukyoku(string reason, List<List<Pai>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            Scores = scores;
        }

        public void onEndKyoku()
        {
            
        }

        public void onError(string message)
        {
            
        }
    }
}
