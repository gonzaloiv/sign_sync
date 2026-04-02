using System.Collections;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class SignVisualDissolver : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private string cutoffKey;
        [SerializeField] private float dissolveSecs;

        private bool isDissolving;
        public bool IsDissolving { get { return isDissolving; } set { isDissolving = value; } }

        public void Dissolve(float initial, float final, float secs)
        {
            if (isDissolving || final == rend.material.GetFloat(cutoffKey))
                return;
            isDissolving = true;
            float timer = 0;
            rend.material.SetFloat(cutoffKey, initial);
            IEnumerator DissolveRoutine()
            {
                while (timer < secs)
                {
                    float value = Mathf.Lerp(initial, final, timer / secs);
                    rend.material.SetFloat(cutoffKey, value);
                    timer += Time.deltaTime;
                    yield return null;
                }
                rend.material.SetFloat(cutoffKey, final);
                isDissolving = false;
            }
            StartCoroutine(DissolveRoutine());
        }

        public void Dissolve(float initial, float final)
        {
            Dissolve(initial, final, dissolveSecs);
        }
    }
}
