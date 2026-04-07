using System;
using System.Linq;
using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class DebugRecogniser : BaseRecogniser
    {
        [SerializeField] private SignIdGOPair[] pairs;

        public override void ListenTo(SignId signId, float duration)
        {
            SignIdGOPair pair = pairs.FirstOrDefault(p => p.id == signId);
            this.InvokeAfterSecs(recognitionData.InitialRecognitionSecs, () => pair.go.SetActive(true));
            this.InvokeAfterSecs(recognitionData.FinalRecognitionSecs, () => pair.go.SetActive(false));
        }
    }

    [Serializable]
    public class SignIdGOPair
    {
        public SignId id;
        public GameObject go;
    }
}