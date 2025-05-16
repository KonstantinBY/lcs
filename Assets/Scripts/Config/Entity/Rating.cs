using System.Collections.Generic;

namespace sl.Entity
{
    public class Rating
    {
        public int userRank;
        public List<RatingEntry> ratingEntries = new List<RatingEntry>();

        public int score;
        public string name;
    }
}