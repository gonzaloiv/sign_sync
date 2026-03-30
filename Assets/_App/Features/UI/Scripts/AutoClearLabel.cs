using TMPro;
using UnityEngine;

namespace DigitalLove.Game.UI
{
    public class AutoClearLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private float secs;

        private float showTime;

        private void Awake()
        {
            label.text = string.Empty;
        }

        public void Show(string text)
        {
            label.text = text;
            showTime = Time.time;
        }

        private void Update()
        {
            if (secs + showTime < Time.time)
                label.text = string.Empty;
        }
    }
}