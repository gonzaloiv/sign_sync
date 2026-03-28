using DigitalLove.FX;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisual : MonoBehaviour
    {
        [SerializeField] private HandFade fade;
        [SerializeField] private GameObject body;
        [SerializeField] private ScalePunch scalePunch;
        [SerializeField] private ParticleSystem ps;

        private float time;
        private Transform origin;
        private Transform destination;
        private float secsToBeat;

        public bool IsActive => body.activeInHierarchy;
        private float TotalActiveSecs => secsToBeat * 2;

        public void Show(Transform origin, Transform destination, float secsToBeat)
        {
            time = 0;
            this.origin = origin;
            transform.position = origin.position;
            this.destination = destination;
            this.secsToBeat = secsToBeat;
            fade?.Fade(secsToBeat);
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
                scalePunch.Animate(() => body.SetActive(false));
            }
        }

        private void Update()
        {
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