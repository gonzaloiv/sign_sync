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

        [Header("FX")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] glitchClips;

        public HandId HandId => handId;

        public SignVisual Spawn(SignId signId, RecognitionData recognitionData, float duration)
        {
            SignIdVisualPair pair = pairs.FirstOrDefault(p => p.id == signId && !p.visual.IsActive);
            if (pair == null)
            {
                SignVisual visual = Instantiate(pairs.FirstOrDefault(p => p.id == signId).visual, transform);
                visual.Hide();
                pair = new SignIdVisualPair() { id = signId, visual = visual };
                pairs.Add(pair);
            }
            pair.visual.Show(origin, destination, recognitionData, duration);
            return pair.visual;
        }

        public void HideAll()
        {
            foreach (SignIdVisualPair pair in pairs)
            {
                pair.visual.Hide();
            }
        }

        public void OnFailed()
        {
            audioSource.clip = glitchClips[UnityEngine.Random.Range(0, glitchClips.Length)];
            audioSource.Play();
        }
    }

    [Serializable]
    public class SignIdVisualPair
    {
        public SignId id;
        public SignVisual visual;
    }
}