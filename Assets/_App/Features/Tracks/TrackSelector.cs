using DigitalLove.DataAccess;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Tracks
{
    public class TrackSelector : MonoBehaviour
    {
        [SerializeField] private TrackData[] tracks;

        [Inject] private MemoryDataClient memoryDataClient;

        public TrackData GetTrack()
        {
            return tracks[0];
        }
    }
}