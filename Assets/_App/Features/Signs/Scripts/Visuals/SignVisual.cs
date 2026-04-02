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

        public void Show(Transform origin, Transform destination, RecognitionData recognitionData)
        {
            time = 0;
            this.origin = origin;
            transform.position = origin.position;
            this.destination = destination;
            this.recognitionData = recognitionData;
            colorHelper.SetRecognitionData(recognitionData);
            body.SetActive(true);
        }

        public void Hide()
        {
            body.SetActive(false);
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
            Hide();
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            if (time < recognitionData.AnimationSecs)
            {
                transform.position = Vector3.Lerp(origin.position, destination.position, time / recognitionData.AnimationSecs);
                time += Time.deltaTime;
            }
            else
            {
                Hide();
            }
        }
    }
}