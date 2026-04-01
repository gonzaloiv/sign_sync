using System;
using System.Collections.Generic;

namespace DigitalLove.Game.Stage
{
    [Serializable]
    public class Band
    {
        public float value;
        public List<IBandListener> listeners = new List<IBandListener>();

        public void AddListener(IBandListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(IBandListener listener)
        {
            listeners.Remove(listener);
        }

        public void Update(float value)
        {
            this.value = value;
            foreach (IBandListener listener in listeners)
            {
                listener.OnUpdate(value);
            }
        }
    }
}