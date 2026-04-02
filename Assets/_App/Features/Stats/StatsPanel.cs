using System;
using DigitalLove.VFX;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalLove.Game.Stats
{
    [Serializable]
    public class StatsPanel : MonoBehaviour
    {
        [SerializeField] private StatsCounter statsCounter;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private ScalePunch scalePunch;
        [SerializeField] private Image healthImage;

        private int previousScore;

        private void Update()
        {
            if (statsCounter.Score != previousScore)
            {
                scalePunch.Animate();
                previousScore = statsCounter.Score;
                scoreLabel.text = statsCounter.Score.ToString();
            }
            healthImage.fillAmount = statsCounter.HealthPercentage;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}