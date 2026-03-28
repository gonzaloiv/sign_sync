using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Listener> listeners = new();

        public HandId HandId => handId;

        public Action<RecognitionState> recognised = (state) => { };
        public Action<FailType> failed = (type) => { };

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

        // ? Debug
        public void InvokeOnSignRecognised(SignId signId) => OnSignRecognised(signId);

        private void OnSignRecognised(SignId signId)
        {
            Debug.LogWarning($"Sign Recognised {signId} in hand {handId}");
            Listener listener = listeners.FirstOrDefault(l => l.signId == signId);
            if (listener != null && listener.FinalTime >= Time.time)
                OnRecognisedSignListenerFound(listener);
        }

        private void OnRecognisedSignListenerFound(Listener listener)
        {
            float recognisedTime = Time.time - listener.startTime;
            RecognitionState state = Mathf.Abs(PreloadSecs - recognisedTime) < PerfectRange ?
                RecognitionState.Perfect : RecognitionState.Good;
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
                if (listener.FinalTime < Time.time)
                {
                    toRemove.Add(listener);
                }
            }
            foreach (Listener listener in toRemove)
            {
                spawner.OnFailed(listener.visual);
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

        public float FinalTime => startTime + HandSignsRecogniser.PreloadSecs * 2;
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
    }
}