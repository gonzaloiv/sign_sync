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
        [SerializeField] private SignVisualVibrator vibrator;

        private float time;
        private Transform origin;
        private Transform destination;
        private RecognitionData recognitionData;
        private float duration;
        private bool hasBeenRecognised;

        public bool IsActive => body.activeInHierarchy;

        public void Show(Transform origin, Transform destination, RecognitionData recognitionData, float duration)
        {
            time = 0;
            hasBeenRecognised = false;
            this.origin = origin;
            this.destination = destination;
            this.recognitionData = recognitionData;
            this.duration = duration;

            transform.position = origin.position;
            colorHelper.Show(recognitionData, duration);
            vibrator.SetIdle();
            body.SetActive(true);
        }

        public void Hide()
        {
            body.SetActive(false);
            colorHelper.Hide();
        }

        public void OnRecognised()
        {
            hasBeenRecognised = true;
            successPS.Play();
            colorHelper.SetSuccessColor();
            scalePunch.Animate();
            fadeColors.SetHighligthColor();
            vibrator.Vibrate();
        }

        public void OnRecognisedFinalTimeReached()
        {
            colorHelper.DissolveOut(Hide);
        }

        public void OnNotRecognised()
        {
            failurePS.Play();
            scalePunch.Animate();
            colorHelper.DissolveOut(Hide);
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            if (!hasBeenRecognised && time < recognitionData.GetFinalRecognitionSecs(duration))
            {
                transform.position = Vector3.Lerp(origin.position, destination.position, time / recognitionData.TotalAnimationSecs);
                time += Time.deltaTime;
            }
        }
    }
}