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

        private List<HandSignListener> listeners = new();
        private List<HandSignListener> listenersInRecognition = new();
        private List<SignId> signsInRecognition = new();

        public HandId HandId => handId;
        public float ActiveSecs => recognitionData.SecsToPerfect;

        public Action<float> recognitionStarted = (percentage) => { };
        public Action<RecognitionEventArgs> recognitionComplete = (args) => { };
        public Action failed = () => { };

        public void ListenTo(SignId signId, float duration)
        {
            SignVisual visual = spawner.Spawn(signId, recognitionData, duration);
            HandSignListener listener = new() { signId = signId, launchTime = Time.time, duration = duration, visual = visual };
            listeners.Add(listener);
        }

        private void Start()
        {
            spawner.HideAll();
            foreach (SignIdSelectorPair pair in pairs)
            {
                void WhenSelected() => OnSignRecognised(pair.id);
                pair.selector.WhenSelected += WhenSelected;
                void WhenUnselected() => OnSignUnRecognised(pair.id);
                pair.selector.WhenUnselected += WhenUnselected;
            }
        }

        // ? Debug
        public void InvokeOnSignRecognised(SignId signId) => OnSignRecognised(signId);

        private void OnSignRecognised(SignId signId)
        {
            if (!signsInRecognition.Contains(signId))
                signsInRecognition.Add(signId);
            IEnumerable<HandSignListener> selection = GetRecognisableListeners(signId);
            if (selection != null && selection.Count() > 0)
            {
                foreach (HandSignListener listener in selection)
                {
                    OnRecognisedSignListenerFound(listener);
                }
            }
        }

        private IEnumerable<HandSignListener> GetRecognisableListeners(SignId signId)
        {
            return listeners.Where(l =>
            {
                if (l.signId != signId)
                    return false;
                if (Time.time < l.launchTime + recognitionData.InitialRecognitionSecs)
                    return false;
                if (Time.time > l.launchTime + recognitionData.GetFinalRecognitionSecs(l.duration))
                    return false;
                return true;
            });
        }

        private void OnRecognisedSignListenerFound(HandSignListener listener)
        {
            listener.percentage = recognitionData.GetAccuracyPercentage(listener.launchTime);
            listener.visual.OnRecognised();
            recognitionStarted.Invoke(listener.percentage);
            listenersInRecognition.Add(listener);
        }

        private void OnSignUnRecognised(SignId signId)
        {
            if (signsInRecognition.Contains(signId))
            {
                signsInRecognition.Remove(signId);
            }
        }

        private void Update()
        {
            IncreaseFrames();
            CheckRecognisedToGo();
            CheckNotRecognised();
        }

        private void IncreaseFrames()
        {
            foreach (SignId signInRecognition in signsInRecognition)
            {
                foreach (HandSignListener listener in listenersInRecognition)
                {
                    if (listener.signId == signInRecognition)
                        listener.frames++;
                }
            }
        }

        private void CheckRecognisedToGo()
        {
            List<HandSignListener> recognisedListenersToGo = GetRecognisedListenersToGo().ToList();
            foreach (HandSignListener listener in recognisedListenersToGo)
            {
                recognitionComplete.Invoke(new RecognitionEventArgs() { percentage = listener.percentage, frames = listener.frames });
                listener.visual.OnRecognisedFinalTimeReached();
                listeners.Remove(listener);
            }
        }

        private IEnumerable<HandSignListener> GetRecognisedListenersToGo()
        {
            return listeners.Where(l =>
            {
                if (l.HasBeenRecognised)
                {
                    if (Time.time > l.launchTime + recognitionData.GetFinalRecognitionSecs(l.duration))
                        return true;
                    if (!signsInRecognition.Contains(l.signId))
                        return true;
                }
                return false;
            });
        }

        private void CheckNotRecognised()
        {
            List<HandSignListener> notRecognised = listeners.Where(l => Time.time > l.launchTime + recognitionData.SecsToPerfect && !l.HasBeenRecognised).ToList();
            foreach (HandSignListener listener in notRecognised)
            {
                if (!listener.HasBeenRecognised)
                {
                    spawner.OnFailed();
                    listener.visual.OnNotRecognised();
                    failed.Invoke();
                }
                listeners.Remove(listener);
            }
        }
    }

    [Serializable]
    public class SignIdSelectorPair
    {
        public SignId id;
        public ActiveStateSelector selector;
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