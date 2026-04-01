using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalLove.Game.Signs
{
    public class HandSignsDebug : MonoBehaviour
    {
        [SerializeField] private HandSignsRecogniser[] recognisers;
        [SerializeField] private HandSignReference[] references;

        private void Update()
        {
            foreach (HandSignReference reference in references)
            {
                if (reference.inputAction.action.WasPerformedThisFrame())
                {
                    recognisers.GetByHandId(reference.handId).InvokeOnSignRecognised(reference.signId);
                    Debug.Log($"InvokeOnSignRecognised {reference.signId} in hand {reference.handId}");
                }
            }
        }
    }

    [Serializable]
    public class HandSignReference
    {
        public SignId signId;
        public HandId handId;
        public InputActionReference inputAction;
    }
}