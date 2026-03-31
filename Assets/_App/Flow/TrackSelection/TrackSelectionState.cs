using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using DigitalLove.Global;
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
        [SerializeField] private TrackSelectionMenu menu;

        [Header("Debug")]
        [SerializeField] private DebugBool autoSelectTrack;
        [SerializeField] private TrackData trackToSelect;

        [Inject] private MemoryDataClient memoryDataClient;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
            menu.Hide();
        }

        public override void Enter()
        {
            if (autoSelectTrack.Value && trackToSelect != null)
            {
                OnTrackSelected(trackToSelect.id);
            }
            else
            {
                menu.trackSelected += OnTrackSelected;

                passthroughStyler.SetStyle(menuStyle);
                menu.Show();
            }
        }

        private void OnTrackSelected(string obj)
        {
            trackSelector.SetCurrent();
            memoryDataClient.Put(trackSelector.CurrentData);
            parent.SetCurrentState(nextState.RouteId);
        }

        public override void Exit()
        {
            menu.trackSelected -= OnTrackSelected;

            menu.Hide();
        }
    }
}