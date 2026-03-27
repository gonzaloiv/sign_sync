using System;
using DigitalLove.Timeline;
using UnityEngine;

namespace DigitalLove.Game.Tracks
{
    public class TrackBehaviour : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private PlayableDirectorWrapper director;
        [SerializeField] private AudioSource audioSource;

        public string Id => id;
        public AudioSource AudioSource => audioSource;

        public Action complete = () => { };

        public void Play()
        {
            gameObject.SetActive(true);
            director.Reset();
            director.Play(onComplete: OnComplete);
        }

        private void OnComplete()
        {
            complete.Invoke();
        }

        public void Stop()
        {
            director.Stop();
            gameObject.SetActive(false);
        }
    }
}