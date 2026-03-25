using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandFade : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private Color origin;
        [SerializeField] private Color destination;
        [SerializeField] private string opacityKey = "_Opacity";

        private float time;
        private int sign;
        private float secs;

        public bool IsActive => gameObject.activeInHierarchy;

        public void Fade(float secs)
        {
            time = 0;
            sign = 1;
            this.secs = secs;
            rend.material.SetFloat(opacityKey, origin.a);
        }

        private void Update()
        {
            Color color = Color.Lerp(origin, destination, time / secs);
            if (time > secs && sign == 1)
                sign = -1;
            rend.material.SetFloat(opacityKey, color.a);
            time += Time.deltaTime * sign;
        }
    }
}