using Klak.Motion;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisualVibrator : MonoBehaviour
    {
        [SerializeField] private BrownianMotion brownianMotion;

        public void SetIdle()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            brownianMotion.enabled = false;
        }

        public void Vibrate()
        {
            brownianMotion.enabled = true;
        }
    }
}
