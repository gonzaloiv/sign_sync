using System.Collections;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackCompleteState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private StageBehaviour stage;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;
        [SerializeField] private AudioSource failed;
        [SerializeField] private PassthroughStyler passthroughStyler;
        [SerializeField] private PassthroughStyle menuStyle;

        public override void Enter()
        {
            stage.Stop();
            passthroughStyler.SetStyle(menuStyle);
            if (!statsCounter.HasHealthBeenDepleted)
            {
                Debug.LogWarning($"Level completed with a score of: {statsCounter.Score}");
            }
            else
            {
                failed.Play();
                trackSelector.CurrentBehaviour.Stop();
                Debug.LogWarning($"Level failed");
            }
            IEnumerator InitRoutine()
            {
                yield return new WaitForSeconds(3);
                parent.SetCurrentState(nextState.RouteId);
            }
            StartCoroutine(InitRoutine());
        }

        public override void Exit()
        {
            stage.SetActive(false);
        }
    }
}