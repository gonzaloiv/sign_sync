using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class HandSignsEditorHelper : MonoBehaviour
    {
        [SerializeField] private HandSignsRecogniser[] recognisers;

        [Button]
        public void LeftRock() => recognisers.GetByHandId(HandId.Left).InvokeOnSignRecognised(SignId.Rock);

        [Button]
        public void LeftPaper() => recognisers.GetByHandId(HandId.Left).InvokeOnSignRecognised(SignId.Paper);

        [Button]
        public void RightRock() => recognisers.GetByHandId(HandId.Right).InvokeOnSignRecognised(SignId.Rock);

        [Button]
        public void RightPaper() => recognisers.GetByHandId(HandId.Right).InvokeOnSignRecognised(SignId.Paper);
    }
}