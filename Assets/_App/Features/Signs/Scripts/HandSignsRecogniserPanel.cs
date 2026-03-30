using DigitalLove.Game.UI;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandSignsRecogniserPanel : MonoBehaviour
    {
        [SerializeField] private AutoClearLabel label;
        [SerializeField] private HandSignsRecogniser recogniser;

        private void OnEnable()
        {
            recogniser.recognised += OnHandSignRecognised;
        }

        private void OnHandSignRecognised(RecognitionLevel recognitionState)
        {
            label.Show($"{recognitionState}");
        }

        private void OnDisable()
        {
            recogniser.recognised -= OnHandSignRecognised;
        }
    }
}