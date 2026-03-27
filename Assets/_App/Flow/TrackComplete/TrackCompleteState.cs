using System.Collections;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
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

        public override void Enter()
        {
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
                yield return new WaitForSeconds(1);
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