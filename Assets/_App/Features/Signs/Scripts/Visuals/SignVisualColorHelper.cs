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
        [SerializeField] private ColorValue perfect;
        [SerializeField] private ColorValue success;

        [Header("Cutoff")]
        [SerializeField] private string cutoffKey;
        [SerializeField] private float cutoffHeight;
        [SerializeField] private float dissolveCutoffHeight;
        [SerializeField] private float dissolveSecs;

        private float time;
        private RecognitionData recognitionData;
        private bool isDissolving;

        public bool IsActive => gameObject.activeInHierarchy;

        public void SetRecognitionData(RecognitionData recognitionData)
        {
            time = 0;
            this.recognitionData = recognitionData;
            SetCutoffHeight(dissolveCutoffHeight, cutoffHeight, dissolveSecs);
            rend.material.SetColor(colorKey, inactive.value);
            isDissolving = false;
        }

        private void SetCutoffHeight(float initial, float final, float secs)
        {
            float timer = 0;
            rend.material.SetFloat(cutoffKey, initial);
            IEnumerator DissolveRoutine()
            {
                while (timer < dissolveSecs)
                {
                    float value = Mathf.Lerp(initial, final, timer / dissolveSecs);
                    rend.material.SetFloat(cutoffKey, value);
                    timer += Time.deltaTime;
                    yield return null;
                }
                rend.material.SetFloat(cutoffKey, final);
            }
            StartCoroutine(DissolveRoutine());
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            Color color = inactive.value;
            if (time < recognitionData.ActiveSecs)
            {
                float percentage = time / recognitionData.ActiveSecs;
                color = Color.Lerp(inactive.value, active.value, percentage);
            }
            else if (time < recognitionData.AnimationSecs)
            {
                float percentage = time / recognitionData.AnimationSecs;
                color = Color.Lerp(inactive.value, perfect.value, percentage);
            }
            if (!isDissolving && time >= recognitionData.ActiveSecs)
                Dissolve();
            rend.material.SetColor(colorKey, color);
            time += Time.deltaTime;
        }

        private void Dissolve()
        {
            isDissolving = true;
            SetCutoffHeight(cutoffHeight, dissolveCutoffHeight, recognitionData.AnimationSecs - recognitionData.ActiveSecs);
        }

        private void OnDisable() => recognitionData = null;

        public void SetSuccessColor()
        {
            recognitionData = null;
            rend.material.SetColor(colorKey, success.value);
        }

    }
}