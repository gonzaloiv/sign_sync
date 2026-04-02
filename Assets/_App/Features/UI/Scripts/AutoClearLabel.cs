using TMPro;
using UnityEngine;

namespace DigitalLove.Game.UI
{
    public class AutoClearLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private float secs;

        private Color initialColor;
        private float showTime;

        private void Awake()
        {
            initialColor = label.color;
            label.text = string.Empty;
        }

        public void Show(string text)
        {
            label.text = text;
            showTime = Time.time;
        }

        public void Show(string text, Color highlightColor)
        {
            Show(text);
            label.color = highlightColor;
        }

        private void Update()
        {
            if (secs + showTime < Time.time)
            {
                label.color = initialColor;
                label.text = string.Empty;
            }
        }
    }
}