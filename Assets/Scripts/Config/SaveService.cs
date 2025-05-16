using System;
using System.Collections.Generic;
using sl.Entity;
using UnityEngine;

namespace sl.Config
{
    public class SaveService
    {
        private static readonly string SAVE_GAME_NAME = "game";
        // private static readonly string SAVE_LVL_NAME = "levels";
        private static readonly string SAVE_CONFIG_NAME = "config";
        private static readonly string SAVE_PATH = "s_01.es3";
        
        internal GameConfig gameConfig;
        internal GameInfo gameInfo;
        private Dictionary<int, LevelInfo> levelsInfo = new Dictionary<int, LevelInfo>();
        
        private ES3Settings es3Settings;
        
        internal static SaveService instance = new SaveService();

        private SaveService()
        {
            ES3.Init();
            es3Settings = new ES3Settings();
            es3Settings.path = SAVE_PATH;

            loadConfig();
            loadGameInfo();
            // Debug.Log("isSoundOff" + gameConfig.isSoundOff);
        }
        
        internal void deleteFile()
        {
            ES3.DeleteFile(SAVE_PATH);
        }
        
        public void saveGameInfo(int score, List<BallEntity> balls) 
        {
            if (balls == null)
            {
                return;
            }
            
            gameInfo.score = score;
            gameInfo.balls = balls.ToArray();

            if (gameInfo.scoreMax < score)
            {
                gameInfo.scoreMax = score;
            }
            
            ES3.Save(SAVE_GAME_NAME, gameInfo, es3Settings);

            loadGameInfo();
        }
        
        internal void loadGameInfo()
        {
            GameInfo defaultGameInfo = new GameInfo();
            gameInfo = ES3.Load(SAVE_GAME_NAME, defaultGameInfo, es3Settings);
            
            // Debug.Log("gameInfo: " +
            //           "Score=" + gameInfo.score 
            //           + "; ScoreMax=" + gameInfo.scoreMax 
            //           + "; Balls=" + gameInfo.balls.Length);
        }

        // internal Dictionary<int, LevelInfo> loadLevelsInfo()
        // {
        //     Dictionary<int, LevelInfo> defaultLevels = new Dictionary<int, LevelInfo>();
        //     return ES3.Load(SAVE_LVL_NAME, defaultLevels, es3Settings);
        // }

        public void saveConfig()
        {
            if (gameConfig == null || es3Settings == null)
            {
                Debug.Log($"Save not init: {gameConfig != null} {es3Settings != null}");
                return;
            }
            
            if (string.IsNullOrEmpty(gameConfig.language))
            {
                Debug.LogError("Language not set. Will be setup default language: en");
                gameConfig.language = "en";
            }
            
            Debug.Log("Config Save: " +
                      "isAdsStop = " + gameConfig.isAdsStop +
                      "; isSoundOff = " + gameConfig.soundValue +
                      "; isMusicOff = " + gameConfig.musicValue +
                      "; language = " + gameConfig.language
                      );

            // Fix some issues with encoding from jslib
            // gameConfig.language = "" + gameConfig.language;


            try
            {
                Debug.Log($"saveConfig: 11111  {gameConfig != null} {es3Settings != null}");
                ES3.Save(SAVE_CONFIG_NAME, gameConfig, es3Settings);
                // loadConfig();
            }
            catch (Exception e)
            {
                Debug.LogError("Error Save Config: " + e.Message);
            }
            
            Debug.Log("saveConfig: 2222");
        }
        
        private void loadConfig()
        {
            GameConfig config = new GameConfig();
            gameConfig = ES3.Load(SAVE_CONFIG_NAME, config, es3Settings);
            
            // Debug.Log("Config Saved: isAdsStop = " + config.isAdsStop +
            //           "; isSoundOff = " + config.isSoundOff +
            //           "; isMusicOff = " + config.isMusicOff
            //           );
        }
    }
}