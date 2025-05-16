namespace sl.Entity
{
    public class LevelInfo
    {
        public int sceneNumber;
        public int score;
        public int stars;
        
        public LevelInfo()
        {
        }

        public LevelInfo(int sceneNumber, int score, int stars)
        {
            this.sceneNumber = sceneNumber;
            this.score = score;
            this.stars = stars;
        }
    }
}