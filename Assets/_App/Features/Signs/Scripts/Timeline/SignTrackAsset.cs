using UnityEngine;
using UnityEngine.Playables;

namespace DigitalLove.Game.Signs
{
    [System.Serializable]
    public class SignTrackAsset : PlayableAsset
    {
        public SignId signId;
        [HideInInspector] public double startTime;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            ScriptPlayable<SignTrackBehaviour> scriptPlayable = ScriptPlayable<SignTrackBehaviour>.Create(graph);
            SignTrackBehaviour behaviour = scriptPlayable.GetBehaviour();
            behaviour.signId = signId;
            behaviour.startTime = startTime;
            return scriptPlayable;
        }
    }
}