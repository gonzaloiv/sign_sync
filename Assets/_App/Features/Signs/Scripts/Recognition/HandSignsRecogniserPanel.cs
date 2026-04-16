using DigitalLove.Game.UI;
using DigitalLove.Global;
using TMPro;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandSignsRecogniserPanel : MonoBehaviour
    {
        [SerializeField] private AutoClearLabel autoClearLabel;
        [SerializeField] private HandSignsRecogniser recogniser;
        [SerializeField] private ColorValue highlightColor;

        private void OnEnable()
        {
            recogniser.recognitionStarted += OnHandSignRecognised;
        }

        private void OnHandSignRecognised(float percentage)
        {
            int formatedPercentage = (int)(percentage * 100);
            if (percentage > 0.75f)
            {
                autoClearLabel.Show($"{formatedPercentage}%", highlightColor.value);
            }
            else
            {
                autoClearLabel.Show($"{formatedPercentage}%");
            }
        }

        private void OnDisable()
        {
            recogniser.recognitionStarted -= OnHandSignRecognised;
        }
    }
}