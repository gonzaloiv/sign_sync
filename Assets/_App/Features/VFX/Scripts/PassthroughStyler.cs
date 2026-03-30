using UnityEngine;

namespace DigitalLove.Game.VFX
{
    public class PassthroughStyler : MonoBehaviour
    {
        [SerializeField] private OVRPassthroughLayer ovrPassthroughLayer;

        public void SetStyle(PassthroughStyle style)
        {
            ovrPassthroughLayer.colorMapEditorType = style.colorMapEditorType;
            ovrPassthroughLayer.colorMapEditorSaturation = style.saturation;
            ovrPassthroughLayer.colorMapEditorGradient = style.gradient;
            ovrPassthroughLayer.SetStyleDirty();
        }
    }
}