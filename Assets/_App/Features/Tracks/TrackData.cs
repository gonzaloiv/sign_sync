using DigitalLove.Casual.Levels;
using UnityEngine;

namespace DigitalLove.Game.Tracks
{
    [CreateAssetMenu(fileName = "TrackData", menuName = "DigitalLove/Game/TrackData")]
    public class TrackData : LevelData
    {
        public string title;
        public int bpm;
        public string artist;
        public string genre;
        public string[] mood;
        public AudioClip clip;
    }
}
