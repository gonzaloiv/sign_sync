using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Tracks;
using DigitalLove.Timeline;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private PlayableDirectorWrapper director;

        [Inject] private MemoryDataClient memoryDataClient;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
        }

        public override void Enter()
        {
            TrackData trackData = memoryDataClient.Get<TrackData>();
            director.Play(onComplete: OnTrackComplete, asset: trackData.timelineAsset);
        }

        private void OnTrackComplete()
        {
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {

        }
    }
}