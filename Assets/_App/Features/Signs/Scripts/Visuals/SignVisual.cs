using DigitalLove.FX;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisual : MonoBehaviour
    {
        [SerializeField] private MaterialColorHelper material;
        [SerializeField] private GameObject body;
        [SerializeField] private ScalePunch scalePunch;
        [SerializeField] private ParticleSystem ps;

        private float time;
        private Transform origin;
        private Transform destination;
        private RecognitionData recognitionData;

        public bool IsActive => body.activeInHierarchy;
        private float TotalActiveSecs => recognitionData.activeSecs * 2;

        public void Show(Transform origin, Transform destination, RecognitionData recognitionData)
        {
            time = 0;
            this.origin = origin;
            transform.position = origin.position;
            this.destination = destination;
            this.recognitionData = recognitionData;
            material?.Fade(recognitionData);
            body.SetActive(true);
        }

        public void Hide(bool instant)
        {
            if (instant)
            {
                body.SetActive(false);
            }
            else
            {
                ps.Play();
                material.SetSuccessColor();
                scalePunch.Animate(() => body.SetActive(false));
            }
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            if (time < TotalActiveSecs)
            {
                transform.position = Vector3.Lerp(origin.position, destination.position, time / TotalActiveSecs);
                time += Time.deltaTime;
            }
            else
            {
                Hide(instant: true);
            }
        }
    }
}