using System;
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
        [SerializeField] private Image healthImage;

        private void Update()
        {
            scoreLabel.text = statsCounter.Score.ToString();
            healthImage.fillAmount = statsCounter.HealthPercentage;
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}