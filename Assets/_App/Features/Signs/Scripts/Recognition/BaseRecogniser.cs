using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public abstract class BaseRecogniser : MonoBehaviour
    {
        [SerializeField] protected RecognitionData recognitionData;

        public float ActiveSecs => recognitionData.SecsToPerfect;

        public abstract void ListenTo(SignId signId, float duration);
    }
}