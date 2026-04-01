using DigitalLove.VFX;
using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class StageBehaviour : MonoBehaviour
    {
        [SerializeField] private MaterialScroller grid;
        [SerializeField] private ReactiveProps reactiveProps;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Play(int bpm, AudioSource audioSource)
        {
            SetActive(true);
            grid.SetSpeed(bpm / 1000f);
            reactiveProps.AudioSource = audioSource;
            reactiveProps.Show();

        }

        public void Stop()
        {
            grid.SetSpeed(0);
            reactiveProps.Hide();
        }
    }
}