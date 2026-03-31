using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "RecognitionData", menuName = "DigitalLove/Game/RecognitionData")]
    public class RecognitionData : ScriptableObject
    {
        [SerializeField] private float activeSecs = 1;
        [SerializeField] private float goodRange = 0.25f;
        [SerializeField] private float perfectRange = 0.125f;

        public float PrePerfectSecs => activeSecs - perfectRange;
        public float FinalSecs => activeSecs + perfectRange;
        public float AnimationSecs => activeSecs * 2;
        public float ActiveSecs => activeSecs;

        public bool IsInPerfectRange(float time)
        {
            return Mathf.Abs(activeSecs - time) < perfectRange;
        }

        public float GetFinalTime(float startTime)
        {
            return startTime + ActiveSecs + goodRange;
        }
    }
}