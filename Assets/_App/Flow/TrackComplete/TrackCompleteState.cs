using DigitalLove.Casual.Analytics;
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
        [SerializeField] private ProgressionEventsHelper progressionEventsHelper;

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

            if (forceNewHighScore.Value) // ? Debug
                statsCounter.ForceHighestScoreStats();

            playerData = memoryDataClient.Get<PlayerData>();
            StopTrack();
            DoEnter();
        }

        private void StopTrack()
        {
            stage.Stop();
            passthroughStyler.SetStyle(menuStyle);
            trackSelector.CurrentBehaviour.Stop();
        }

        private void OnCountdownComplete() => parent.SetCurrentState(trackSelectionState.RouteId);

        private void OnReplayButtonClick() => parent.SetCurrentState(replayState.RouteId);

        private void DoEnter()
        {
            if (statsCounter.HasHealthBeenDepleted)
            {
                OnFailed();
            }
            else
            {
                OnComplete();
            }
        }

        private void OnFailed()
        {
            progressionEventsHelper.SendLevelFailedEvent("health_depleted", trackSelector.CurrentData.id);
            failed.Play();
            trackCompletePanel.Show();
        }

        private async void OnComplete()
        {
            progressionEventsHelper.SendLevelCompleteEvent(trackSelector.CurrentData.id, score: statsCounter.Score);
            bool isHighestScore = IsNewHighScore();
            if (isHighestScore)
            {
                trackCompletePanel.ShowWithNewHighScore(statsCounter.Score);
                await unityPlayerDataClient.Put(playerData);
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
