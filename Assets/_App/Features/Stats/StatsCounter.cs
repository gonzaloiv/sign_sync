using System;
using System.Linq;
using DigitalLove.Game.Signs;
using UnityEngine;

namespace DigitalLove.Game.Stats
{
    public class StatsCounter : MonoBehaviour
    {
        [SerializeField] private HandSignsRecogniser[] recognisers;
        [SerializeField] private RecognitionData[] recognitionValues;
        [SerializeField] private FailData[] failValues;

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
                recogniser.recognised += OnHandSignRecognised;
                recogniser.failed += OnFailed;
            }
        }

        private void OnHandSignRecognised(RecognitionState recognitionState)
        {
            RecognitionData recognitionData = recognitionValues.FirstOrDefault(p => p.recognitionState == recognitionState);
            stats.IncreaseScore(recognitionData.score);
            stats.IncreaseHealth(recognitionData.health);
        }

        private void OnFailed(FailType failType)
        {
            FailData failData = failValues.FirstOrDefault(p => p.failType == failType);
            stats.DecreaseHealth(failData.health);
            if (HasHealthBeenDepleted)
                defeated.Invoke();
        }

        private void OnDisable()
        {
            foreach (HandSignsRecogniser recogniser in recognisers)
            {
                recogniser.recognised -= OnHandSignRecognised;
                recogniser.failed -= OnFailed;
            }
        }
    }

    [Serializable]
    public class RecognitionData
    {
        public RecognitionState recognitionState;
        public int score;
        public float health;
    }

    [Serializable]
    public class FailData
    {
        public FailType failType;
        public float health;
    }
}
