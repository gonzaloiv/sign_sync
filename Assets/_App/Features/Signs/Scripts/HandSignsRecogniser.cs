using System;
using System.Collections.Generic;
using System.Linq;
using DigitalLove.Game.UI;
using Oculus.Interaction;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandSignsRecogniser : MonoBehaviour
    {
        public const float PreloadSecs = 1f;
        public const float PerfectRange = 0.2f;

        [SerializeField] private HandId handId;
        [SerializeField] private HandVisualsSpawner spawner;
        [SerializeField] private SignIdSelectorPair[] pairs;
        [SerializeField] private AutoClearLabel label;

        private List<Listener> listeners = new();

        public void Clear() => listeners.Clear();

        public void ListenTo(SignId signId)
        {
            SignVisual visual = spawner.Spawn(signId, PreloadSecs);
            Listener listener = new() { signId = signId, startTime = Time.time, visual = visual };
            listeners.Add(listener);
        }

        private void Start()
        {
            foreach (SignIdSelectorPair pair in pairs)
            {
                void WhenSelected() => OnSignRecognised(pair.id);
                pair.selector.WhenSelected += WhenSelected;
            }
        }

        private void OnSignRecognised(SignId signId)
        {
            // Debug.LogWarning($"Sign Recognised {signId} in hand {handId}");
            Listener listener = listeners.FirstOrDefault(l => l.signId == signId);
            if (listener != null && listener.FinalTime > Time.time)
            {
                float perfectTime = listener.startTime + PreloadSecs;
                float recognisedTime = Time.time - listener.startTime;
                RecognitionState state = Mathf.Abs(PreloadSecs - recognisedTime) < PerfectRange ? RecognitionState.Perfect : RecognitionState.Good;
                label.Show($"{state}");
                listener.visual.Hide(instant: false);
            }
        }

        private void Update()
        {
            listeners = listeners.Where(l => l.FinalTime > Time.time).ToList();
        }
    }

    [Serializable]
    public class SignIdSelectorPair
    {
        public SignId id;
        public ActiveStateSelector selector;
    }

    public class Listener
    {
        public SignId signId;
        public float startTime;
        public SignVisual visual;

        public float FinalTime => startTime + HandSignsRecogniser.PreloadSecs * 2;
    }
}