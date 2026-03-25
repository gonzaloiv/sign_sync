using System.Linq;
using DigitalLove.Global;
using UnityEngine;

namespace DigitalLove.Game.Signs
{
    public class VisualsSpawner : MonoBehaviour
    {
        [SerializeField] private HandVisualsSpawner[] spawners;

        [Header("Debug")]
        [SerializeField] private HandSignData toSpawn;

        private void Awake()
        {
            foreach (HandVisualsSpawner spawner in spawners)
            {
                spawner.HideAll();
            }
        }

        [Button]
        public void SpawnToSpawn() => Spawn(toSpawn);

        public void Spawn(HandSignData signData)
        {
            GetByHandId(signData.handId).Spawn(signData.signId, HandSignsRecogniser.PreloadSecs);
        }

        public HandVisualsSpawner GetByHandId(HandId handId) => spawners.FirstOrDefault(s => s.HandId == handId);
    }
}