using UnityEngine;

namespace DigitalLove.Game.Signs
{
    [CreateAssetMenu(fileName = "SignData", menuName = "DigitalLove/Game/SignData")]
    public class SignData : ScriptableObject
    {
        public string id;
        public GameObject prefab;
    }
}