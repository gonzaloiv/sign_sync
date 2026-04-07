using System;
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
        [SerializeField] private SignVisualDissolver dissolver;
        [SerializeField] private float cutoffHeight;
        [SerializeField] private float dissolveCutoffHeight;

        [Header("Trail")]
        [SerializeField] private TrailRenderer trailRenderer;

        private float time;
        private RecognitionData recognitionData;
        private float duration;
        private bool isSynced;

        public bool IsActive => gameObject.activeInHierarchy;

        public void Show(RecognitionData recognitionData, float duration)
        {
            time = 0;
            this.recognitionData = recognitionData;
            this.duration = duration;
            dissolver.Dissolve(dissolveCutoffHeight, cutoffHeight);
            rend.material.SetColor(colorKey, inactive.value);
            this.InvokeAfterSecs(0.1f, () => ShowTrail(duration));
            isSynced = false;

        }

        private void ShowTrail(float duration)
        {
            trailRenderer.Clear();
            trailRenderer.time = duration;
            trailRenderer.material.color = inactive.value;
            trailRenderer.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (recognitionData == null || time > recognitionData.TotalAnimationSecs || isSynced)
                return;
            Color color = rend.material.GetColor(colorKey);
            if (recognitionData.IsInRecognitionRange(time))
            {
                // float percentage = (time - recognitionData.InitialRecognitionSecs) / recognitionData.GetFinalRecognitionSecs();
                // color = Color.Lerp(active.value, inRecognitionRange.value, percentage);
                color = inRecognitionRange.value;
            }
            if (recognitionData.IsInperfectRange(time))
            {
                color = success.value;
            }
            else if (time < recognitionData.InitialRecognitionSecs) // ? PRE-RECOGNITION
            {
                float percentage = time / recognitionData.SecsToPerfect;
                color = Color.Lerp(inactive.value, active.value, percentage);
            }
            rend.material.SetColor(colorKey, color);
            trailRenderer.material.color = color;
            time += Time.deltaTime;
        }

        public void Hide()
        {
            recognitionData = null;
            HideTrail();
        }

        private void HideTrail()
        {
            trailRenderer.time = 0;
            trailRenderer.Clear();
            trailRenderer.gameObject.SetActive(false);
        }

        public void SetSuccessColor()
        {
            rend.material.SetColor(colorKey, success.value);
            trailRenderer.material.color = success.value;
            isSynced = true;
        }

        public void DissolveOut(Action onComplete)
        {
            float animationSecs = 0.5f;
            float timer = 0;
            dissolver.Dissolve(cutoffHeight, dissolveCutoffHeight, animationSecs);
            HideTrail();
            IEnumerator DissolveRoutine()
            {
                while (timer < animationSecs)
                {
                    float percentage = time / recognitionData.TotalAnimationSecs;
                    Color color = Color.Lerp(inRecognitionRange.value, inactive.value, percentage);
                    timer += Time.deltaTime;
                    rend.material.SetColor(colorKey, color);
                    yield return null;
                }
                onComplete.Invoke();
            }
            StartCoroutine(DissolveRoutine());
        }
    }
}