using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace DigitalLove.Game.Signs
{
    [TrackClipType(typeof(SignTrackAsset))]
    [TrackBindingType(typeof(BaseRecogniser))]
    public class SignTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                SignTrackAsset asset = clip.asset as SignTrackAsset;
                if (asset != null)
                {
                    asset.startTime = clip.start;
                    asset.finalTime = clip.end;
                }
            }
            return ScriptPlayable<SignTrackMixerBehaviour>.Create(graph, inputCount);
        }
    }
}