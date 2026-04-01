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

        [Header("High Score")]
        [SerializeField] private TextMeshProUGUI highScoreLabel;
        [SerializeField] private GameObject highScorePanel;


        [Header("Idle-Hover")]
        [SerializeField] private AudioSource hoverAudioSource;
        [SerializeField] private float initialHoverAudioSourceTime = 12f;
        [SerializeField] private GameObject glitch;
        [SerializeField] private AudioSource idleAudioSource;

        private TrackData trackData;
        private Action<string> clicked;

        public void Show(TrackData trackData, Cookie cookie, Action<string> clicked)
        {
            this.trackData = trackData;
            this.clicked = clicked;

            gameObject.SetActive(true);
            ShowTrackData(trackData);
            ShowHighScore(cookie);
            ShowIdle();
        }

        private void ShowTrackData(TrackData trackData)
        {
            titleLabel.text = trackData.title;
            artistLabel.text = trackData.artist;
            genreLabel.text = trackData.genre;
            moodLabel.text = $"{trackData.mood[0]}, {trackData.mood[1]}";
        }

        private void ShowHighScore(Cookie cookie)
        {
            if (cookie != null)
            {
                highScorePanel.SetActive(true);
                highScoreLabel.text = $"{cookie.metadata}";
            }
            else
            {
                highScorePanel.SetActive(false);
            }
        }

        private void ShowIdle()
        {
            glitch.SetActive(false);
            idleAudioSource.Play();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverAudioSource.clip = trackData.clip;
            hoverAudioSource.time = initialHoverAudioSourceTime;
            hoverAudioSource.Play();
            glitch.SetActive(true);
            idleAudioSource.Pause();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverAudioSource.Stop();
            ShowIdle();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked(trackData.id);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}