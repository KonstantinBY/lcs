using sl.Config;
using UnityEngine;
using UnityEngine.UI;

namespace sl.controller
{
    // This class handles updating the sound UI widgets depending on the player's selection.
    public class SoundController : MonoBehaviour
    {
        public SpriteSwapper spriteSwapper;
        public ColorSwapper colorSwapper;
        
        private SaveService saveService;
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            saveService = SaveService.instance;
            
            float soundValue = saveService.gameConfig.soundValue;
            AudioListener.volume = soundValue;
            slider.value = soundValue;
            
            spriteSwapper.setSprite(soundValue > 0);
            colorSwapper.setColor(soundValue > 0);
            
            if (soundValue !=  saveService.gameConfig.soundValue)
            {
                saveService.gameConfig.soundValue = soundValue;
                saveService.saveConfig();
            }
            // Debug.Log("sound_off = " + isSoundOff);
        }

        public void updateSound()
        {
            float newValue = slider.value;
            AudioListener.volume = newValue;

            if (spriteSwapper.isEnabled != (newValue > 0))
            {
                spriteSwapper.setSprite(newValue > 0);
                colorSwapper.setColor(newValue > 0);
            }
            
            if (newValue !=  saveService.gameConfig.soundValue)
            {
                saveService.gameConfig.soundValue = newValue;
                saveService.saveConfig();
            }
        }
    }
}