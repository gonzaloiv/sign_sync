using UnityEngine;
using UnityEngine.Timeline;

namespace DigitalLove.Game.Tracks
{
    [CreateAssetMenu(fileName = "TrackData", menuName = "DigitalLove/Game/TrackData")]
    public class TrackData : ScriptableObject
    {
        public string id;
        public TimelineAsset timelineAsset;
        public AudioClip audioClip;
        public int bpm;
    }
}
