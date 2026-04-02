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

        public bool IsActive => gameObject.activeInHierarchy;

        public void Show(RecognitionData recognitionData, float duration)
        {
            time = 0;
            this.recognitionData = recognitionData;
            this.duration = duration;
            dissolver.Dissolve(dissolveCutoffHeight, cutoffHeight);
            rend.material.SetColor(colorKey, inactive.value);
            ShowTrail(duration);
        }

        private void ShowTrail(float duration)
        {
            trailRenderer.Clear();
            trailRenderer.time = duration;
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.material.color = inactive.value;
        }

        private void Update()
        {
            if (recognitionData == null || time > recognitionData.TotalAnimationSecs)
                return;
            Color color = inactive.value;
            if (recognitionData.IsInRecognitionRange(time, duration))
            {
                float percentage = time - recognitionData.InitialRecognitionSecs / recognitionData.GetFinalRecognitionSecs(duration);
                color = Color.Lerp(active.value, inRecognitionRange.value, percentage);
            }
            else if (time < recognitionData.SecsToPerfect) // ? PRE-RECOGNITION
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
            trailRenderer.gameObject.SetActive(false);
            trailRenderer.time = 0;
        }

        public void SetSuccessColor()
        {
            rend.material.SetColor(colorKey, success.value);
            trailRenderer.material.color = success.value;
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