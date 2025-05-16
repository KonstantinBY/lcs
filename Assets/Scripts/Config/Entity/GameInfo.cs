namespace sl.Entity
{
    public class GameInfo
    {
        public int score;
        public int scoreMax;
        public BallEntity[] balls = new BallEntity[0];
        
        public GameInfo()
        {
        }

        public GameInfo(int score, int scoreMax, BallEntity[] balls)
        {
            this.score = score;
            this.scoreMax = scoreMax;
            this.balls = balls;
        }
    }
}