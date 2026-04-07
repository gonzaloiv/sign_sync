using System.Linq;
using UnityEngine;

namespace DigitalLove.Game.Tracks
{
    public class TrackSelector : MonoBehaviour
    {
        [SerializeField] private TrackData[] data;
        [SerializeField] private TrackBehaviour[] behaviours;

        private TrackBehaviour currentBehaviour;
        public TrackBehaviour CurrentBehaviour => currentBehaviour;

        private TrackData currentData;
        public TrackData CurrentData => currentData;

        public TrackData[] TracksData => data;

        public void SetCurrent(string id)
        {
            currentData = data.FirstOrDefault(d => string.Equals(d.id, id));
            currentBehaviour = behaviours.FirstOrDefault(b => string.Equals(currentData.id, b.Id));
        }
    }
}