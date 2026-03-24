using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Tracks;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackSelectionState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private TrackSelector trackSelector;

        [Inject] private MemoryDataClient memoryDataClient;

        public override void Enter()
        {
            trackSelector.SetCurrent();
            memoryDataClient.Put(trackSelector.CurrentData);
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {

        }
    }
}