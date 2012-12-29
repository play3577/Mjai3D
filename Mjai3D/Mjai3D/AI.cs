using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mjai3D
{
    public interface AI
    {
        object onStartGame(int id, List<string> names);
        object onStartKyoku(Pai bakaze, int kyoku, int honba, int kyotaku, int oya, Pai doraMarker, List<List<Pai>> tehais);
        object onTsumo(int actor, Pai pai);
        object onDahai(int actor, Pai pai, bool tsumogiri);
        object onReach(int actor);
        object onReachAccepted(int actor, List<int> deltas, List<int> scores);
        object onPon(int actor, int target, Pai pai, List<Pai> consumed);
        object onChi(int actor, int target, Pai pai, List<Pai> consumed);
        object onKan(int actor, int target, Pai pai, List<Pai> consumed);
        object onHora(int actor, int target, Pai pai, List<Pai> horaTehais, List<YakuN> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores);
        object onRyukyoku(string reason, List<List<Pai>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores);
        object onEndKyoku();
        void onError(string message);
    }
}
