namespace sl.Config
{
    public class GameContext
    {
        internal static readonly GameContext instance = new GameContext();

        internal static int adsLvlCount;
        private static readonly int showAdsNumber = 5;

        internal int currentLvl;

        private GameContext()
        {
        }

        internal bool nextAds()
        {
            if (adsLvlCount++ >= showAdsNumber)
            {
                adsLvlCount = 0;
                return true;
            }

            return false;
        }
    }
}