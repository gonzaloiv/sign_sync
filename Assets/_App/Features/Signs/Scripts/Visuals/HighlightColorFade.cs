using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HighlightColorFade : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private Color idle;
        [SerializeField] private Color highlight;
        [SerializeField] private string key;
        [SerializeField] private float fadeSecs;

        private void Start()
        {
            rend.material.SetColor(key, idle);
        }

        public void SetHighligthColor()
        {
            rend.material.SetColor(key, highlight);
            this.InvokeAfterSecs(fadeSecs, () => rend.material.SetColor(key, idle));
        }
    }
}