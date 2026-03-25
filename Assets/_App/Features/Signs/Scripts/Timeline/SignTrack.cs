using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace DigitalLove.Game.Signs
{
    [TrackClipType(typeof(SignTrackAsset))]
    [TrackBindingType(typeof(HandSignsRecogniser))]
    public class SignTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                SignTrackAsset asset = clip.asset as SignTrackAsset;
                if (asset != null)
                    asset.startTime = clip.start;
            }
            return ScriptPlayable<SignTrackMixerBehaviour>.Create(graph, inputCount);
        }
    }
}