using System.Collections;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
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

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
            stage.SetActive(false);
        }

        public override void Enter()
        {
            origin.Setup();
            IEnumerator InitRoutine()
            {
                init.Play();
                yield return new WaitForSeconds(1);
                passthroughStyler.SetStyle(playStyle);
                stage.Play(trackSelector.CurrentData.bpm, trackSelector.CurrentBehaviour.AudioSource);
                parent.SetCurrentState(nextState.RouteId);
            }
            StartCoroutine(InitRoutine());
        }

        public override void Exit()
        {

        }
    }
}