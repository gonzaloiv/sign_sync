using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "RecognitionData", menuName = "DigitalLove/Game/RecognitionData")]
    public class RecognitionData : ScriptableObject
    {
        public float activeSecs = 1;
        public float perfectRange = 0.125f;

        public float PrePerfectSecs => activeSecs - perfectRange;
        public float FinalSecs => activeSecs + perfectRange;
    }
}