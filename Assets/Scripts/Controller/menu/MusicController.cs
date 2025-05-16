using sl.Config;
using UnityEngine;
using UnityEngine.UI;

namespace sl.controller
{
    public class MusicController : MonoBehaviour
    {
        public SpriteSwapper spriteSwapper;
        public ColorSwapper colorSwapper;
        
        private SaveService saveService;
        private AudioSource musicSource;
        private Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            saveService = SaveService.instance;

            float musicValue = saveService.gameConfig.musicValue;

            musicSource = GameObject.FindWithTag("bg_music").GetComponent<AudioSource>();
            musicSource.volume = musicValue;
            slider.value = musicValue;
            
            spriteSwapper.setSprite(musicValue > 0);
            colorSwapper.setColor(musicValue > 0);
            
            
            if (musicValue != saveService.gameConfig.musicValue)
            {
                saveService.gameConfig.musicValue = musicValue;
                saveService.saveConfig();
            }
            // Debug.Log("isMusicOff = " + isMusicOff);
        }

        public void updateSound()
        {
            float newValue = slider.value;
            musicSource.volume = newValue;
            
            if (spriteSwapper.isEnabled != (newValue > 0))
            {
                spriteSwapper.setSprite(newValue > 0);
                colorSwapper.setColor(newValue > 0);
            }

            if (newValue !=  saveService.gameConfig.musicValue)
            {
                saveService.gameConfig.musicValue = newValue;
                saveService.saveConfig();
            }
        }
    }
}