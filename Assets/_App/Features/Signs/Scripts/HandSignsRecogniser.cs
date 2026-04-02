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
        public float ActiveSecs => recognitionData.SecsToPerfect;

        public Action<float> recognised = (percentage) => { };
        public Action failed = () => { };

        public void ListenTo(SignId signId, float duration)
        {
            SignVisual visual = spawner.Spawn(signId, recognitionData);
            Listener listener = new() { signId = signId, launchTime = Time.time, duration = duration, visual = visual };
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
            IEnumerable<Listener> selection = GetRecognisableListeners(signId);
            List<Listener> toRemove = new();
            if (selection != null && selection.Count() > 0)
            {
                foreach (Listener listener in selection)
                {
                    OnRecognisedSignListenerFound(listener);
                    toRemove.Add(listener);
                }
            }
            foreach (Listener listener in toRemove)
            {
                listeners.Remove(listener);
            }
        }

        private IEnumerable<Listener> GetRecognisableListeners(SignId signId)
        {
            return listeners.Where(l =>
            {
                if (l.signId != signId)
                    return false;
                if (Time.time > recognitionData.GetFinalTime(l.launchTime))
                    return false;
                if (Time.time < recognitionData.GetStartTime(l.launchTime))
                    return false;
                return true;
            });
        }

        private void OnRecognisedSignListenerFound(Listener listener)
        {
            float percentage = recognitionData.GetPercentage(listener.launchTime);
            listener.visual.OnSuccess();
            recognised.Invoke(percentage);
        }

        private void Update()
        {
            // ? This is when reduces score
            if (listeners.Count <= 0)
                return;
            List<Listener> toRemove = new();
            foreach (Listener listener in listeners)
            {
                if (Time.time > recognitionData.GetFinalTime(listener.launchTime))
                {
                    toRemove.Add(listener);
                }
            }
            foreach (Listener listener in toRemove)
            {
                spawner.OnFailed();
                listener.visual.OnFailure();
                listeners.Remove(listener);
                failed.Invoke();
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
        public float launchTime;
        public float duration;
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
    }
}