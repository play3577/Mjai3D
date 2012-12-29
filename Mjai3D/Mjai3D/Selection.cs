using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    public enum SelectionType
    {
        Pon, Chi, Kan, Ron, Reach, Tsumo, Pass
    }

    public interface Selection1
    {
        T match<T>(Func<Pai, bool, T> fDahai,
                   Func<Pai, T> fAnkan,
                   Func<Pai, T> fKakan,
                   Func<T> fReach,
                   Func<T> fTsumo);
    }

    public interface Selection2
    {
        T match<T>(Func<Pai, Pai, T> fChi,
                   Func<Pai, Pai, T> fPon,
                   Func<T> fDaiminkan,
                   Func<T> fRon,
                   Func<T> fPass);
    }

    public class Dahai : Selection1
    {
        Pai pai;
        bool tsumogiri;
        public Dahai(Pai pai, bool tsumogiri = false)
        {
            this.pai = pai;
            this.tsumogiri = tsumogiri;
        }

        public T match<T>(Func<Pai, bool, T> fDahai,
                          Func<Pai, T> fAnkan,
                          Func<Pai, T> fKakan,
                          Func<T> fReach,
                          Func<T> fTsumo)
        {
            return fDahai(pai, tsumogiri);
        }
    }

    public class Ankan : Selection1
    {
        Pai pai;
        public Ankan(Pai pai)
        {
            this.pai = pai;
        }

        public T match<T>(Func<Pai, bool, T> fDahai,
                          Func<Pai, T> fAnkan,
                          Func<Pai, T> fKakan,
                          Func<T> fReach,
                          Func<T> fTsumo)
        {
            return fAnkan(pai);
        }
    }

    public class Kakan : Selection1
    {
        Pai pai;
        public Kakan(Pai pai)
        {
            this.pai = pai;
        }

        public T match<T>(Func<Pai, bool, T> fDahai,
                          Func<Pai, T> fAnkan,
                          Func<Pai, T> fKakan,
                          Func<T> fReach,
                          Func<T> fTsumo)
        {
            return fKakan(pai);
        }
    }

    public class Reach : Selection1
    {
        public Reach() { }

        public T match<T>(Func<Pai, bool, T> fDahai,
                          Func<Pai, T> fAnkan,
                          Func<Pai, T> fKakan,
                          Func<T> fReach,
                          Func<T> fTsumo)
        {
            return fReach();
        }
    }

    public class Tsumo : Selection1
    {
        public Tsumo() { }

        public T match<T>(Func<Pai, bool, T> fDahai,
                          Func<Pai, T> fAnkan,
                          Func<Pai, T> fKakan,
                          Func<T> fReach,
                          Func<T> fTsumo)
        {
            return fTsumo();
        }
    }

    public class Chi : Selection2
    {
        Pai pai1;
        Pai pai2;

        public Chi(Pai pai1, Pai pai2)
        {
            this.pai1 = pai1;
            this.pai2 = pai2;
        }

        public T match<T>(Func<Pai, Pai, T> fChi,
                          Func<Pai, Pai, T> fPon,
                          Func<T> fDaiminkan,
                          Func<T> fRon,
                          Func<T> fPass)
        {
            return fChi(pai1, pai2);
        }
    }

    public class Pon : Selection2
    {
        Pai pai1;
        Pai pai2;

        public Pon(Pai pai1, Pai pai2)
        {
            this.pai1 = pai1;
            this.pai2 = pai2;
        }

        public T match<T>(Func<Pai, Pai, T> fChi,
                          Func<Pai, Pai, T> fPon,
                          Func<T> fDaiminkan,
                          Func<T> fRon,
                          Func<T> fPass)
        {
            return fPon(pai1, pai2);
        }
    }

    public class Daiminkan : Selection2
    {
        public Daiminkan() { }

        public T match<T>(Func<Pai, Pai, T> fChi,
                          Func<Pai, Pai, T> fPon,
                          Func<T> fDaiminkan,
                          Func<T> fRon,
                          Func<T> fPass)
        {
            return fDaiminkan();
        }
    }

    public class Ron : Selection2
    {
        public Ron() { }

        public T match<T>(Func<Pai, Pai, T> fChi,
                          Func<Pai, Pai, T> fPon,
                          Func<T> fDaiminkan,
                          Func<T> fRon,
                          Func<T> fPass)
        {
            return fRon();
        }
    }

    public class Pass : Selection2
    {
        public Pass() { }

        public T match<T>(Func<Pai, Pai, T> fChi,
                          Func<Pai, Pai, T> fPon,
                          Func<T> fDaiminkan,
                          Func<T> fRon,
                          Func<T> fPass)
        {
            return fPass();
        }
    }
}
