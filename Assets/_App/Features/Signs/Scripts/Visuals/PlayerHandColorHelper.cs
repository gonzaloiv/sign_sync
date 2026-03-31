using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class PlayerHandColorHelper : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private ColorValue idle;
        [SerializeField] private ColorValue success;
        [SerializeField] private string key;
        [SerializeField] private float successSecs;

        private void Start()
        {
            rend.material.SetColor(key, idle.value);
        }

        public void SetSuccessColor()
        {
            rend.material.SetColor(key, success.value);
            this.InvokeAfterSecs(successSecs, () => rend.material.SetColor(key, idle.value));
        }
    }
}