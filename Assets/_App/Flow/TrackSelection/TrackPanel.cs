using System;
using DigitalLove.DataAccess;
using DigitalLove.Game.Tracks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DigitalLove.Game.Flow
{
    public class TrackPanel : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI artistLabel;
        [SerializeField] private TextMeshProUGUI genreLabel;
        [SerializeField] private TextMeshProUGUI moodLabel;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private float initialAudioSourceTime = 12f;
        [SerializeField] private GameObject glitch;


        private TrackData trackData;
        private Action<string> clicked;

        public void Show(TrackData trackData, Cookie cookie, Action<string> clicked)
        {
            this.trackData = trackData;
            this.clicked = clicked;
            titleLabel.text = trackData.title;
            artistLabel.text = trackData.artist;
            genreLabel.text = trackData.genre;
            moodLabel.text = $"{trackData.mood[0]}, {trackData.mood[1]}";
            glitch.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            audioSource.clip = trackData.clip;
            audioSource.time = initialAudioSourceTime;
            audioSource.Play();
            glitch.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            glitch.SetActive(false);
            audioSource.Stop();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked(trackData.id);
        }
    }
}