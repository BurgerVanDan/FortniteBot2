using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FortniteBot.Models
{
    public class FortniteClasses
    {
    }

    public class RecentMatch
    {
        public int id { get; set; }
        public string accountId { get; set; }
        public string playlist { get; set; }
        public int kills { get; set; }
        public int minutesPlayed { get; set; }
        public int top1 { get; set; }
        public int top5 { get; set; }
        public int top6 { get; set; }
        public int top10 { get; set; }
        public int top12 { get; set; }
        public int top25 { get; set; }
        public int matches { get; set; }
        public int top3 { get; set; }
        public DateTime dateCollected { get; set; }
        public int score { get; set; }
        public int platform { get; set; }
        public double trnRating { get; set; }
        public double trnRatingChange { get; set; }
    }

    public class RootObject
    {
        public string accountId { get; set; }
        public int platformId { get; set; }
        public string platformName { get; set; }
        public string platformNameLong { get; set; }
        public string epicUserHandle { get; set; }
        public List<RecentMatch> recentMatches { get; set; }
    }
}