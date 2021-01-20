using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Riot_SoloRankStatus
    {
        public int Wins { get; private set; }
        public int Losses { get; private set; }
        /// <summary>
        /// 경기수
        /// </summary>
        public int Maches { get; private set; }
        /// <summary>
        /// 승률
        /// </summary>
        public double Per { get; private set; }
        /// <summary>
        /// 티어 단게
        /// </summary>
        public string Rank { get; private set; }
        /// <summary>
        /// 티어
        /// </summary>
        public string Tier { get; private set; }

        public Riot_SoloRankStatus(int wins, int losses,string tier, string rank)
        {
            Wins = wins;
            Losses = losses;
            Maches = Wins + Losses;
            Per = ((double)Wins / (double)Maches) * 100;
            Tier = tier;
            Rank = rank;
        }
       /// <summary>
       /// 언랭크
       /// </summary>
        public Riot_SoloRankStatus()
        {
            Wins = 0;
            Losses = 0;
            Maches = 0;
            Per = 0;
            Tier = "UNRANKED";
            Rank = "";
        }
        public string ToString_Tier()
        {
            return string.Format("{0} {1}",Tier, Rank);
        }
    }
}
