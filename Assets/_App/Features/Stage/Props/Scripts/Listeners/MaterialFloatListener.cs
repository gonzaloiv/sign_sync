using UnityEngine;

namespace DigitalLove.Game.Stage
{
    public class MaterialFloatListener : MonoBehaviour, IBandListener
    {
        [SerializeField] private int bandIndex;
        [SerializeField] private string key;
        [SerializeField] private float offset;
        [SerializeField] private float multiplier;
        [SerializeField] private Renderer rend;

        public int BandIndex => bandIndex;

        public void OnUpdate(float value)
        {
            rend.material.SetFloat(key, (value * multiplier) + offset);
        }
    }
}
