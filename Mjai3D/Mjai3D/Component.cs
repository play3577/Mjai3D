using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    interface Component
    {
        void onStartGame(int id, List<string> names);
        void onStartKyoku(Pai bakaze, int kyoku, int honba, int kyotaku, int oya, Pai doraMarker, List<List<Pai>> tehais);
        void onTsumo(int actor, Pai pai);
        void onDahai(int actor, Pai pai, bool tsumogiri);
        void onReach(int actor);
        void onReachAccepted(int actor, List<int> deltas, List<int> scores);
        void onPon(int actor, int target, Pai pai, List<Pai> consumed);
        void onChi(int actor, int target, Pai pai, List<Pai> consumed);
        void onKan(int actor, int target, Pai pai, List<Pai> consumed);
        void onHora(int actor, int target, Pai pai, List<Pai> horaTehais, List<YakuN> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores);
        void onRyukyoku(string reason, List<List<Pai>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores);
        void onEndKyoku();
        void onError(string message);
    }
}
