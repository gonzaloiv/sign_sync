using UnityEngine;
using static OVRPassthroughLayer;

namespace DigitalLove.Game.VFX
{
    [CreateAssetMenu(fileName = "PassthroughStyle", menuName = "DigitalLove/Game/PassthroughStyle")]
    public class PassthroughStyle : ScriptableObject
    {
        public ColorMapEditorType colorMapEditorType;
        public Gradient gradient;
        public float saturation;
    }
}