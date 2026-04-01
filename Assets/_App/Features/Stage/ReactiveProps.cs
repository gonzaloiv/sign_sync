using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class ReactiveProps : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioSource AudioSource { set { audioSource = value; } }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Update()
        {

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
