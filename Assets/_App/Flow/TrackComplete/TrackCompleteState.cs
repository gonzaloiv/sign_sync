using System.Collections;
using DigitalLove.DataAccess;
using DigitalLove.FlowControl;
using DigitalLove.Game.Stage;
using DigitalLove.Game.Stats;
using DigitalLove.Game.Tracks;
using DigitalLove.Game.VFX;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackCompleteState : MonoState
    {
        [SerializeField] private MonoState trackSelectionState;
        [SerializeField] private StageBehaviour stage;
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private StatsCounter statsCounter;
        [SerializeField] private AudioSource failed;
        [SerializeField] private PassthroughStyler passthroughStyler;
        [SerializeField] private PassthroughStyle menuStyle;

        [Inject] private MemoryDataClient memoryDataClient;
        [Inject] private UnityPlayerDataClient unityPlayerDataClient;

        private PlayerData playerData;

        public override void Enter()
        {
            playerData = memoryDataClient.Get<PlayerData>();
            if (!statsCounter.HasHealthBeenDepleted)
            {
                OnTrackComplete();
            }
            else
            {
                failed.Play();
                trackSelector.CurrentBehaviour.Stop();
            }
            StartTrackCompleteRoutine();
        }

        private void StartTrackCompleteRoutine()
        {
            IEnumerator InitRoutine()
            {
                stage.Stop();
                passthroughStyler.SetStyle(menuStyle);
                yield return new WaitForSeconds(3);
                ToLevelSelection();
            }
            StartCoroutine(InitRoutine());
        }

        private void ToLevelSelection()
        {
            stage.SetActive(false);
            parent.SetCurrentState(trackSelectionState.RouteId);
        }

        private void OnTrackComplete()
        {
            Debug.LogWarning($"Level completed with a score of: {statsCounter.Score}");
            bool isHighestScore = CheckScore();
            if (isHighestScore)
            {
                SaveScore();
                // TODO: Show highest score panel before replay button
            }
            else
            {
                // TODO: Show replay button
            }
        }

        private bool CheckScore()
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

        private async void SaveScore()
        {
            await unityPlayerDataClient.Put(playerData);
        }

        public override void Exit()
        {

        }
    }
}
