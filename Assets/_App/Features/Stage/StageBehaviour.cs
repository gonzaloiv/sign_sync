using DigitalLove.VFX;
using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class StageBehaviour : MonoBehaviour
    {
        [SerializeField] private MaterialScroller grid;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Play(int bpm)
        {
            SetActive(true);
            grid.SetSpeed(bpm / 1000f);
        }

        public void Stop()
        {
            grid.SetSpeed(0);
        }
    }
}