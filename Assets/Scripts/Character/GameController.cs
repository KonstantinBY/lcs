using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ToonPeople
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] internal LevelData levelData;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI currentEventNumberText;
        public TextMeshProUGUI totalEventsText;
            
        [SerializeField] internal int eventsAmountOnFirstsLevel = 5;
        [SerializeField] internal int eventsAmountIncrease = 3;
        internal int currentEventNumber = 0;
        internal int currentTotalEventNumber = 0;
        
        [SerializeField] private PlayerState playerState;
        
        [SerializeField] private Slider comfortSlider;
        [SerializeField] private Slider stressSlider;
        
        
        private TextMeshProUGUI comfortText;
        private TextMeshProUGUI stressText;

        private void Start()
        {
            comfortText = comfortSlider.GetComponentInChildren<TextMeshProUGUI>();
            stressText = stressSlider.GetComponentInChildren<TextMeshProUGUI>();
            levelText.text = levelData.level.ToString();
        }

        private void LateUpdate()
        {
            comfortText.text = ((int) playerState.comfort).ToString();
            stressText.text = ((int) playerState.stress).ToString();
            
            comfortSlider.value = playerState.comfort;
            stressSlider.value = playerState.stress;

            currentEventNumberText.text = currentEventNumber.ToString();
            totalEventsText.text = currentTotalEventNumber.ToString();
        }

        public void reduceComfort(float value)
        {
            playerState.comfort = math.max(0, playerState.comfort - value);
        }

        public void addComfort(int value)
        {
            playerState.comfort = math.min(100, playerState.comfort + value);
        }

        public void nextLevel()
        {
            currentEventNumber = 0;
            ++levelData.level;
            levelText.text = levelData.level.ToString();
        }
    }

    [Serializable]
    public class LevelData
    {
        public int level = 1;
    }

    [Serializable]
    public class PlayerState
    {
        public float comfort;
        public float stress;
    }
}