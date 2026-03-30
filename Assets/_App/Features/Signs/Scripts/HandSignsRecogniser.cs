using System;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandSignsRecogniser : MonoBehaviour
    {
        [SerializeField] private RecognitionData recognitionData;
        [SerializeField] private HandId handId;
        [SerializeField] private HandVisualsSpawner spawner;
        [SerializeField] private SignIdSelectorPair[] pairs;

        private List<Listener> listeners = new();

        public HandId HandId => handId;
        public float ActiveSecs => recognitionData.activeSecs;

        public Action<RecognitionLevel> recognised = (state) => { };
        public Action<FailType> failed = (type) => { };

        public void ListenTo(SignId signId)
        {
            SignVisual visual = spawner.Spawn(signId, recognitionData);
            Listener listener = new() { signId = signId, startTime = Time.time, visual = visual };
            listeners.Add(listener);
        }

        private void Start()
        {
            spawner.HideAll();
            foreach (SignIdSelectorPair pair in pairs)
            {
                void WhenSelected() => OnSignRecognised(pair.id);
                pair.selector.WhenSelected += WhenSelected;
            }
        }

        // ? Debug
        public void InvokeOnSignRecognised(SignId signId) => OnSignRecognised(signId);

        private void OnSignRecognised(SignId signId)
        {
            // Debug.LogWarning($"Sign Recognised {signId} in hand {handId}");
            Listener listener = listeners.FirstOrDefault(l => l.signId == signId);
            if (listener != null && listener.GetFinalTime(recognitionData.activeSecs) >= Time.time)
                OnRecognisedSignListenerFound(listener);
        }

        private void OnRecognisedSignListenerFound(Listener listener)
        {
            float recognisedTime = Time.time - listener.startTime;
            RecognitionLevel state = Mathf.Abs(recognitionData.activeSecs - recognisedTime) < recognitionData.perfectRange ?
                RecognitionLevel.Perfect : RecognitionLevel.Good;
            listener.visual.Hide(instant: false);
            listeners.Remove(listener);
            recognised.Invoke(state);
        }

        private void Update()
        {
            // ? This is when reduces score
            if (listeners.Count <= 0)
                return;
            List<Listener> toRemove = new();
            foreach (Listener listener in listeners)
            {
                if (listener.GetFinalTime(recognitionData.activeSecs) < Time.time)
                {
                    toRemove.Add(listener);
                }
            }
            foreach (Listener listener in toRemove)
            {
                spawner.OnFailed();
                listeners.Remove(listener);
                failed.Invoke(FailType.NotRecognised);
            }
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
    }

    public static class HandSignsRecogniserExtensions
    {
        public static HandSignsRecogniser GetByHandId(this IEnumerable<HandSignsRecogniser> recognisers, HandId handId)
        {
            foreach (HandSignsRecogniser recogniser in recognisers)
            {
                if (recogniser.HandId == handId)
                    return recogniser;
            }
            return null;
        }

        public static float GetFinalTime(this Listener listener, float activeSecs)
        {
            return listener.startTime + activeSecs * 2;
        }
    }
}