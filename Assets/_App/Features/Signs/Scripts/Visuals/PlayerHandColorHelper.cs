using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class PlayerHandColorHelper : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private Color idle;
        [SerializeField] private Color success;
        [SerializeField] private string key;
        [SerializeField] private float successSecs;

        private void Start()
        {
            rend.material.SetColor(key, idle);
        }

        public void SetSuccessColor()
        {
            rend.material.SetColor(key, success);
            this.InvokeAfterSecs(successSecs, () => rend.material.SetColor(key, idle));
        }
    }
}