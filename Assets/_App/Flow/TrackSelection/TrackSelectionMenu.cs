using System;
using System.Collections.Generic;
using DigitalLove.DataAccess;
using DigitalLove.Game.Tracks;
using DigitalLove.UI.Behaviours;
using Reflex.Attributes;
using UnityEngine;

namespace DigitalLove.Game.Flow
{
    public class TrackSelectionMenu : MonoBehaviour
    {
        [SerializeField] private TrackSelector trackSelector;
        [SerializeField] private List<TrackPanel> panels;
        [SerializeField] private SubtitlesLikeFollow subtitlesLikeFollow;

        [Inject] private MemoryDataClient memoryDataClient;

        public Action<string> trackSelected = (string id) => { };

        public void Show()
        {
            gameObject.SetActive(true);
            HideAll();
            PlayerData playerData = memoryDataClient.Get<PlayerData>();
            TrackData[] tracksData = trackSelector.TracksData;
            for (int i = 0; i < tracksData.Length; i++)
            {
                if (i >= panels.Count)
                {
                    TrackPanel trackPanel = Instantiate(panels[0], panels[0].transform.parent);
                    panels.Add(trackPanel);
                }
                Cookie cookie = playerData.GetCookieById(tracksData[i].id);
                panels[i].Show(tracksData[i], cookie, OnTrackPanelClick);
            }
            subtitlesLikeFollow.ShowInCameraView();
        }

        public void OnTrackPanelClick(string id)
        {
            trackSelected.Invoke(id);
        }

        public void HideAll()
        {
            foreach (TrackPanel panel in panels)
            {
                panel.Hide();
            }
        }

        public void Hide()
        {
            HideAll();
            gameObject.SetActive(false);
        }
    }
}