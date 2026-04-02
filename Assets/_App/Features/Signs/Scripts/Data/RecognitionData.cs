using System;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "RecognitionData", menuName = "DigitalLove/Game/RecognitionData")]
    public class RecognitionData : ScriptableObject
    {
        [SerializeField] private float activeSecs = 1;
        [SerializeField] private float recognitionRange = 0.25f;

        public float AnimationSecs => activeSecs * 2;
        public float ActiveSecs => activeSecs;

        public float GetPercentage(float launchTime)
        {
            float recognisedTime = Time.time;
            float perfectTime = launchTime + activeSecs;
            float deviation = Math.Abs(recognisedTime - perfectTime);
            return 1f - deviation / recognitionRange;
        }

        public float GetFinalTime(float launchTime)
        {
            return launchTime + ActiveSecs + recognitionRange;
        }

        public float GetStartTime(float launchTime)
        {
            return launchTime + ActiveSecs - recognitionRange;
        }
    }
}