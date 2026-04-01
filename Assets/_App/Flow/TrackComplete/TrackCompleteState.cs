using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using DigitalLove.Global;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackCompleteState : MonoState
    {
        [SerializeField] private MonoState trackSelectionState;
        [SerializeField] private MonoState replayState;

        [SerializeField] private StageBehaviour stage;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;
        [SerializeField] private TrackCompletePanel trackCompletePanel;

        [Header("FX")]
        [SerializeField] private AudioSource failed;
        [SerializeField] private PassthroughStyler passthroughStyler;
        [SerializeField] private PassthroughStyle menuStyle;

        [Header("Debug")]
        [SerializeField] private DebugBool forceNewHighScore;

        [Inject] private MemoryDataClient memoryDataClient;
        [Inject] private UnityPlayerDataClient unityPlayerDataClient;

        private PlayerData playerData;

        public override void Init(StateMachine parent)
        {
            base.Init(parent);
            trackCompletePanel.Hide();
        }

        public override void Enter()
        {
            trackCompletePanel.countdownComplete += OnCountdownComplete;
            trackCompletePanel.replayButtonClick += OnReplayButtonClick;

            playerData = memoryDataClient.Get<PlayerData>();
            stage.Stop();
            passthroughStyler.SetStyle(menuStyle);
            trackSelector.CurrentBehaviour.Stop();

            DoEnter();
        }

        private void OnCountdownComplete() => parent.SetCurrentState(trackSelectionState.RouteId);

        private void OnReplayButtonClick() => parent.SetCurrentState(replayState.RouteId);

        private async void DoEnter()
        {
            bool isHighestScore = IsNewHighScore();
            if (isHighestScore)
            {
                trackCompletePanel.ShowWithNewHighScore(statsCounter.Score);
                await unityPlayerDataClient.Put(playerData);
            }
            else if (forceNewHighScore.Value) // ! Debug
            {
                trackCompletePanel.ShowWithNewHighScore(999);
            }
            else if (statsCounter.HasHealthBeenDepleted)
            {
                failed.Play();
                trackCompletePanel.Show();
            }
            else
            {
                trackCompletePanel.Show();
            }
        }

        private bool IsNewHighScore()
        {
            TrackData trackData = trackSelector.CurrentData;
            Cookie current = playerData.GetCookieById(trackData.id);
            if (current == null) // ? First time complete
            {
                current = new(trackData.id) { metadata = statsCounter.Score.ToString() };
                playerData.AddCookie(current);
                return true;
            }
            else if (int.Parse(current.metadata) < statsCounter.Score)
            {
                current.metadata = statsCounter.Score.ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Exit()
        {
            trackCompletePanel.countdownComplete -= OnCountdownComplete;
            trackCompletePanel.replayButtonClick -= OnReplayButtonClick;

            stage.SetActive(false);
            trackCompletePanel.Hide();
        }
    }
}
