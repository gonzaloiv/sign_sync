using DigitalLove.FlowControl;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackState : MonoState
    {
        [SerializeField] private MonoState trackCompleteState;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;

        [Header("Debug")]
        [SerializeField] private DebugBool forceTrackComplete;

        public override void Enter()
        {
            trackSelector.CurrentBehaviour.complete += ToTrackCompleteState;
            statsCounter.defeated += ToTrackCompleteState;

            trackSelector.CurrentBehaviour.Play();
            statsCounter.Restart();

            if (forceTrackComplete.Value)
                ToTrackCompleteState();
        }

        private void ToTrackCompleteState()
        {
            parent.SetCurrentState(trackCompleteState.RouteId);
        }

        public override void Exit()
        {
            trackSelector.CurrentBehaviour.complete -= ToTrackCompleteState;
            statsCounter.defeated -= ToTrackCompleteState;
        }
    }
}