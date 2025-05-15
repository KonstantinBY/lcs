using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ToonPeople
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public LevelData levelData;
        [SerializeField] private PlayerState playerState;
        
        [SerializeField] private Slider comfortSlider;
        [SerializeField] private Slider stressSlider;
        
        private TextMeshProUGUI comfortText;
        private TextMeshProUGUI stressText;

        private void Start()
        {
            comfortText = comfortSlider.GetComponentInChildren<TextMeshProUGUI>();
            stressText = stressSlider.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            comfortText.text = ((int) playerState.comfort).ToString();
            stressText.text = ((int) playerState.stress).ToString();
            
            comfortSlider.value = playerState.comfort;
            stressSlider.value = playerState.stress;
        }

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