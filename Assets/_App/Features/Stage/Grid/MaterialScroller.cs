using UnityEngine;

namespace DigitalLove.FX
{
    public class MaterialScroller : MonoBehaviour
    {
        [SerializeField] private Renderer rend;
        [SerializeField] private string textureName = "_BaseMap";
        [SerializeField] private Vector2 direction;
        [SerializeField] private float speed;

        private void Update()
        {
            if (rend != null)
            {
                Vector2 current = rend.material.GetTextureOffset(textureName);
                current += direction * speed * Time.deltaTime;
                rend.material.SetTextureOffset(textureName, current);
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}
