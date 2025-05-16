 namespace sl.Config
{
    public class GameConfig
    {
        public bool isAdsStop;
        public float soundValue = 1;
        public float musicValue = 1;
        public string language = "en";

        public GameConfig()
        {
        }

        public GameConfig(bool isAdsStop, float soundValue, float musicValue, string language)
        {
            this.isAdsStop = isAdsStop;
            this.soundValue = soundValue;
            this.musicValue = musicValue;
            this.language = language;
        }
    }
}