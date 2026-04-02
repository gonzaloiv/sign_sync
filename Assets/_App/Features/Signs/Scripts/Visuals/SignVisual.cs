using DigitalLove.VFX;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisual : MonoBehaviour
    {
        [SerializeField] private SignVisualColorHelper colorHelper;
        [SerializeField] private HighlightColorFade[] fadeColors;
        [SerializeField] private GameObject body;
        [SerializeField] private ScalePunch scalePunch;
        [SerializeField] private ParticleSystem successPS;
        [SerializeField] private ParticleSystem failurePS;

        private float time;
        private Transform origin;
        private Transform destination;
        private RecognitionData recognitionData;

        public bool IsActive => body.activeInHierarchy;

        public void Show(Transform origin, Transform destination, RecognitionData recognitionData, float duration)
        {
            time = 0;
            this.origin = origin;
            transform.position = origin.position;
            this.destination = destination;
            this.recognitionData = recognitionData;
            colorHelper.Show(recognitionData, duration);
            body.SetActive(true);
        }

        public void Hide()
        {
            body.SetActive(false);
            colorHelper.Hide();
        }

        public void OnSuccess()
        {
            successPS.Play();
            colorHelper.SetSuccessColor();
            scalePunch.Animate(Hide);
            if (fadeColors != null && fadeColors.Length > 0)
            {
                foreach (HighlightColorFade fadeColor in fadeColors)
                {
                    fadeColor.SetHighligthColor();
                }
            }
        }

        public void OnFailure()
        {
            failurePS.Play();
            scalePunch.Animate();
            colorHelper.ShowFailure(Hide);
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            if (time < recognitionData.FinalRecognitionSecs)
            {
                transform.position = Vector3.Lerp(origin.position, destination.position, time / recognitionData.TotalAnimationSecs);
                time += Time.deltaTime;
            }
        }
    }
}