using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandVisualsSpawner : MonoBehaviour
    {
        [SerializeField] private HandId handId;
        [SerializeField] private List<SignIdVisualPair> pairs;

        [SerializeField] private Transform origin;
        [SerializeField] private Transform destination;

        public HandId HandId => handId;

        public SignVisual Spawn(SignId signId, float preloadSecs)
        {
            SignIdVisualPair pair = pairs.FirstOrDefault(p => p.id == signId && !p.visual.IsActive);
            if (pair == null)
            {
                SignVisual visual = Instantiate(pairs.FirstOrDefault(p => p.id == signId).visual, transform);
                visual.Hide(instant: true);
                pair = new SignIdVisualPair() { id = signId, visual = visual };
                pairs.Add(pair);
            }
            pair.visual.Show(origin, destination, preloadSecs);
            return pair.visual;
        }

        public void HideAll()
        {
            foreach (SignIdVisualPair pair in pairs)
            {
                pair.visual.Hide(instant: true);
            }
        }
    }

    [Serializable]
    public class SignIdVisualPair
    {
        public SignId id;
        public SignVisual visual;
    }
}