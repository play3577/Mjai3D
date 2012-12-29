using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    public enum Yaku
    {
        TENHO, CHIHO, KOKUSHIMUSO, DAISANGEN, SUANKO,
        TSUISO, RYUISO, CHINROTO, DAISUSHI, SHOSUSHI,
        SUKANTSU, CHURENPOTON, DORA, URADORA, AKADORA,
        REACH, IPPATSU, MENZENCHIN_TSUMOHO, TANYAOCHU,
        PINFU, IPEKO, BAKAZE, JIKAZE, RINSHANKAIHO,
        CHANKAN, HAITEIRAOYUE, HOTEIRAOYUI, SANSHOKUDOJUN, IKKITSUKAN,
        HONCHANTAIYAO, CHITOITSU, TOITOIHO, SANANKO, HONROTO,
        SANSHOKUDOKO, SANKANTSU, SHOSANGEN, DOUBLE_REACH, HONISO,
        JUNCHANTAIYAO, RYANPEKO, CHINISO
    }

    public struct YakuN
    {
        public Yaku yaku;
        public int n;

        public YakuN(Yaku yaku, int n)
        {
            this.yaku = yaku;
            this.n = n;
        }
    }

    public static class YakuUtil
    {
        public static Yaku FromString(string s)
        {
            // TODO implementation
            return Yaku.TENHO;
        }
    }
}
