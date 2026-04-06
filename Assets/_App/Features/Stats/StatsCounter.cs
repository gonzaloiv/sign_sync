using System;
using DigitalLove.Game.Signs;
using UnityEngine;

namespace DigitalLove.Game.Stats
{
    public class StatsCounter : MonoBehaviour
    {
        [SerializeField] private HandSignsRecogniser[] recognisers;
        [SerializeField] private SuccessData successData;
        [SerializeField] private FailData failData;

        [Header("Debug")]
        [SerializeField] private Stats stats;

        public int Score => stats.score;
        public bool HasHealthBeenDepleted => stats.health < 0;
        public float HealthPercentage => stats.HealthPercentage;

        public Action defeated = () => { };

        public void Restart() => stats.Reset();

        private void OnEnable()
        {
            foreach (HandSignsRecogniser recogniser in recognisers)
            {
                recogniser.recognitionComplete += OnHandSignRecognitionComplete;
                recogniser.failed += OnFailed;
            }
        }

        private void OnHandSignRecognitionComplete(RecognitionEventArgs args)
        {
            int score = (int)(successData.score * args.percentage * args.frames);
            stats.IncreaseScore(score);
            stats.IncreaseHealth(successData.health * args.percentage);
        }

        private void OnFailed()
        {
            stats.DecreaseHealth(failData.health);
            if (HasHealthBeenDepleted)
                defeated.Invoke();
        }

        private void OnDisable()
        {
            foreach (HandSignsRecogniser recogniser in recognisers)
            {
                recogniser.recognitionComplete -= OnHandSignRecognitionComplete;
                recogniser.failed -= OnFailed;
            }
        }

        // ? Debug
        public void ForceHighestScoreStats()
        {
            stats.score = 9999999;
            stats.health = Stats.InitialHealth;
        }
    }

    [Serializable]
    public class SuccessData
    {
        public int score;
        public float health;
    }

    [Serializable]
    public class FailData
    {
        public float health;
    }
}
