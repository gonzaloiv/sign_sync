using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalLove.Game.UI
{
    public class AutoClearLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private float secs;

        private float showTime;

        public UnityEvent showed;

        private void Awake()
        {
            label.text = string.Empty;
        }

        public void Show(string text)
        {
            label.text = text;
            showTime = Time.time;
            showed.Invoke();
        }

        private void Update()
        {
            if (secs + showTime < Time.time)
                label.text = string.Empty;
        }
    }
}