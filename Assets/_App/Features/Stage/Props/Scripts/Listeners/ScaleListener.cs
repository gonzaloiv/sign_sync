using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class ScaleListener : MonoBehaviour, IBandListener
    {
        [SerializeField] private int bandIndex;
        [SerializeField] private float multiplier;
        [SerializeField] private float offset;
        [SerializeField] private Transform body;

        public int BandIndex => bandIndex;

        public void OnUpdate(float value)
        {
            float axisScaleValue = value * multiplier + offset;
            body.localScale = new Vector3(axisScaleValue, axisScaleValue, axisScaleValue);
        }
    }
}