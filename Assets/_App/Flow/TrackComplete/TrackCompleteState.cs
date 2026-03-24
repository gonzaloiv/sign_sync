using System.Collections;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackCompleteState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private StageBehaviour stage;

        public override void Enter()
        {
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