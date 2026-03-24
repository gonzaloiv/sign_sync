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
        [SerializeField] private TrackSelector trackSelector;

        [Inject] private MemoryDataClient memoryDataClient;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
        }

        public override void Enter()
        {
            trackSelector.CurrentBehaviour.complete += OnTrackComplete;
            trackSelector.CurrentBehaviour.Play();
        }

        private void OnTrackComplete()
        {
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {
            trackSelector.CurrentBehaviour.complete -= OnTrackComplete;
        }
    }
}