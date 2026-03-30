using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class MaterialColorHelper : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private ColorValue inactive;
        [SerializeField] private ColorValue active;
        [SerializeField] private ColorValue perfect;
        [SerializeField] private ColorValue success;
        [SerializeField] private string key;

        private float time;
        private RecognitionData recognitionData;

        public bool IsActive => gameObject.activeInHierarchy;

        public void Fade(RecognitionData recognitionData)
        {
            time = 0;
            this.recognitionData = recognitionData;
            rend.material.SetColor(key, inactive.value);
        }

        private void Update()
        {
            if (recognitionData == null)
                return;
            Color color = inactive.value;
            if (time < recognitionData.PrePerfectSecs)
            {
                float percentage = time / recognitionData.PrePerfectSecs;
                color = Color.Lerp(inactive.value, active.value, percentage);
            }
            else if (time < recognitionData.FinalSecs)
            {
                float percentage = time / recognitionData.FinalSecs;
                color = Color.Lerp(inactive.value, perfect.value, percentage);
            }
            rend.material.SetColor(key, color);
            time += Time.deltaTime;
        }

        private void OnDisable() => recognitionData = null;

        public void SetSuccessColor()
        {
            recognitionData = null;
            rend.material.SetColor(key, success.value);
        }
    }
}