using System.Collections;
using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisualColorHelper : MonoBehaviour
    {
        [SerializeField] private Renderer rend;

        [Header("Colors")]
        [SerializeField] private string colorKey;
        [SerializeField] private ColorValue inactive;
        [SerializeField] private ColorValue active;
        [SerializeField] private ColorValue inRecognitionRange;
        [SerializeField] private ColorValue success;

        [Header("Cutoff")]
        [SerializeField] private Dissolver dissolver;
        [SerializeField] private float cutoffHeight;
        [SerializeField] private float dissolveCutoffHeight;

        private float time;
        private RecognitionData recognitionData;

        public bool IsActive => gameObject.activeInHierarchy;

        public void SetRecognitionData(RecognitionData recognitionData)
        {
            time = 0;
            this.recognitionData = recognitionData;
            dissolver.Dissolve(dissolveCutoffHeight, cutoffHeight);
            rend.material.SetColor(colorKey, inactive.value);
        }

        private void Update()
        {
            if (recognitionData == null || time > recognitionData.TotalAnimationSecs)
                return;
            Color color = inactive.value;
            if (recognitionData.IsInRecognitionRange(time))
            {
                color = inRecognitionRange.value;
            }
            else if (time < recognitionData.SecsToPerfect) // ? PRE-RECOGNITION
            {
                float percentage = time / recognitionData.SecsToPerfect;
                color = Color.Lerp(inactive.value, active.value, percentage);
            }
            else if (time < recognitionData.TotalAnimationSecs) // ? POST-RECOGNITION
            {
                float percentage = time / recognitionData.TotalAnimationSecs;
                color = Color.Lerp(inRecognitionRange.value, inactive.value, percentage);
                dissolver.Dissolve(cutoffHeight, dissolveCutoffHeight, recognitionData.TotalAnimationSecs - recognitionData.SecsToPerfect);
            }
            rend.material.SetColor(colorKey, color);
            time += Time.deltaTime;
        }

        private void OnDisable() => recognitionData = null;

        public void SetSuccessColor()
        {
            recognitionData = null;
            rend.material.SetColor(colorKey, success.value);
        }
    }
}