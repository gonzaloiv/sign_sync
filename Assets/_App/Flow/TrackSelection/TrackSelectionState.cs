using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackSelectionState : MonoState
    {
        [SerializeField] private MonoState nextState;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private PassthroughStyler passthroughStyler;
        [SerializeField] private PassthroughStyle menuStyle;

        [Inject] private MemoryDataClient memoryDataClient;

        public override void Enter()
        {
            trackSelector.SetCurrent();
            memoryDataClient.Put(trackSelector.CurrentData);
            parent.SetCurrentState(nextState.RouteId);
            passthroughStyler.SetStyle(menuStyle);
        }

        public override void Exit()
        {

        }
    }
}