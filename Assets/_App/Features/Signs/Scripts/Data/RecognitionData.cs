using System;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "RecognitionData", menuName = "DigitalLove/Game/RecognitionData")]
    public class RecognitionData : ScriptableObject
    {
        [SerializeField] private float activeSecs = 1;
        [SerializeField] private float recognitionRange = 0.25f;
        [SerializeField] private float perfectRange = 0.1f;

        public float TotalAnimationSecs => activeSecs * 2;
        public float SecsToPerfect => activeSecs;
        public float InitialRecognitionSecs => SecsToPerfect - recognitionRange;
        public float FinalRecognitionSecs => SecsToPerfect + recognitionRange;

        public float GetAccuracyPercentage(float launchTime)
        {
            float recognisedTime = Time.time;
            float perfectTime = launchTime + activeSecs;
            float deviation = Math.Abs(recognisedTime - perfectTime);
            return 1f - deviation / recognitionRange;
        }

        public bool IsInRecognitionRange(float time)
        {
            return time > SecsToPerfect - recognitionRange && time < FinalRecognitionSecs;
        }

        public bool IsInperfectRange(float time)
        {
            return time > SecsToPerfect - perfectRange && time < SecsToPerfect + perfectRange;
        }
    }
}