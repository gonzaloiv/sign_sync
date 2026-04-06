using DigitalLove.Casual.Analytics;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using DigitalLove.Global;
using DigitalLove.XR;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackInitState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private OVROriginBehaviour origin;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StageBehaviour stage;
        [SerializeField] private AudioSource init;
        [SerializeField] private PassthroughStyler passthroughStyler;
        [SerializeField] private PassthroughStyle playStyle;
        [SerializeField] private ProgressionEventsHelper progressionEventsHelper;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
            stage.SetActive(false);
        }

        public override void Enter()
        {
            origin.Setup();
            init.Play();
            this.InvokeAfterSecs(1, ToTrackState);
        }

        private void ToTrackState()
        {
            passthroughStyler.SetStyle(playStyle);
            stage.Play(trackSelector.CurrentData.bpm, trackSelector.CurrentBehaviour.AudioSource);
            progressionEventsHelper.SendLevelStartedEvent(trackSelector.CurrentData.id);
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {

        }
    }
}