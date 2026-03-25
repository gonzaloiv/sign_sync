using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "SignData", menuName = "DigitalLove/Game/HandSignData")]
    public class HandSignData : ScriptableObject
    {
        public HandId handId;
        public SignId signId;
    }
}