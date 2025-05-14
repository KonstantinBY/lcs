using Unity.Mathematics;
using UnityEngine;

namespace ToonPeople
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public LevelData levelData;
        [SerializeField] private PlayerState playerState;

        public void reduceComfort(float value)
        {
            playerState.comfort = math.max(0, playerState.comfort - value);
        }

        public void addComfort(int value)
        {
            playerState.comfort = math.min(100, playerState.comfort + value);
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public int level = 1;
    }

    [System.Serializable]
    public class PlayerState
    {
        public float comfort;
        public float stress;
    }
}