using System;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "RecognitionData", menuName = "DigitalLove/Game/RecognitionData")]
    public class RecognitionData : ScriptableObject
    {
        [SerializeField] private float activeSecs = 1;
        [SerializeField] private float recognitionRange = 0.25f;

        public float TotalAnimationSecs => activeSecs * 2;
        public float SecsToPerfect => activeSecs;

        public float GetPercentage(float launchTime)
        {
            float recognisedTime = Time.time;
            float perfectTime = launchTime + activeSecs;
            float deviation = Math.Abs(recognisedTime - perfectTime);
            return 1f - deviation / recognitionRange;
        }

        public float GetFinalTime(float launchTime)
        {
            return launchTime + SecsToPerfect + recognitionRange;
        }

        public float GetStartTime(float launchTime)
        {
            return launchTime + SecsToPerfect - recognitionRange;
        }

        public bool IsInRecognitionRange(float time)
        {
            Debug.LogWarning($"time = {time}");
            Debug.LogWarning($"SecsToPerfect + recognitionRange = {SecsToPerfect + recognitionRange}");
            Debug.LogWarning($"SecsToPerfect - recognitionRange = {SecsToPerfect - recognitionRange}");
            return time < SecsToPerfect + recognitionRange && time > SecsToPerfect - recognitionRange;
        }
    }
}