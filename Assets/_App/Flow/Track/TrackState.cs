using DigitalLove.FlowControl;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using DigitalLove.Global;
using DigitalLove.XR;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackState : MonoState
    {
        [SerializeField] private MonoState trackCompleteState;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;
        [SerializeField] private OVRSessionHelper ovrSessionHelper;

        [Header("Debug")]
        [SerializeField] private DebugBool forceTrackComplete;

        public override void Enter()
        {
            AddListeners();

            trackSelector.CurrentBehaviour.Play();
            statsCounter.Restart();

            if (forceTrackComplete.Value)
                ToTrackCompleteState();
        }

        private void ToTrackCompleteState()
        {
            parent.SetCurrentState(trackCompleteState.RouteId);
        }

        private void OnSessionPaused()
        {
            trackSelector.CurrentBehaviour.Pause();
        }

        private void OnSessionUnpaused()
        {
            trackSelector.CurrentBehaviour.Play();
        }

        private void AddListeners()
        {
            trackSelector.CurrentBehaviour.complete += ToTrackCompleteState;
            statsCounter.defeated += ToTrackCompleteState;
            ovrSessionHelper.hasPaused += OnSessionPaused;
            ovrSessionHelper.hasUnpaused += OnSessionUnpaused;
        }

        public override void Exit()
        {
            trackSelector.CurrentBehaviour.complete -= ToTrackCompleteState;
            statsCounter.defeated -= ToTrackCompleteState;
            ovrSessionHelper.hasPaused -= OnSessionPaused;
            ovrSessionHelper.hasUnpaused -= OnSessionUnpaused;
        }
    }
}