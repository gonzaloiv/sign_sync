using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;

        [Header("FX")]
        [SerializeField] private SpectrumVisualizer spectrumVisualizer;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
            if (Application.isEditor)
                Camera.main.clearFlags = CameraClearFlags.Skybox;
        }

        public override void Enter()
        {
            trackSelector.CurrentBehaviour.complete += ToTrackCompleteState;
            statsCounter.defeated += ToTrackCompleteState;

            trackSelector.CurrentBehaviour.Play();
            statsCounter.Restart();
            if (spectrumVisualizer != null)
                spectrumVisualizer.AudioSource = trackSelector.CurrentBehaviour.AudioSource;
        }

        private void ToTrackCompleteState()
        {
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {
            trackSelector.CurrentBehaviour.complete -= ToTrackCompleteState;
            statsCounter.defeated -= ToTrackCompleteState;

            if (spectrumVisualizer != null)
                spectrumVisualizer.AudioSource = null;
        }
    }
}