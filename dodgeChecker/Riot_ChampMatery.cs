using MingweiSamuel.Camille.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Riot_ChampMatery
    {
       public Champion Champion { get; set; }
       public int ChampionPoints { get; set; }
       public int ChampionLevel { get; set; }
       //public DateTime Lastplay { get; set; }

        public Riot_ChampMatery(Champion name,int points, int level)
        {
            Champion = name;
            ChampionLevel = level;
            ChampionPoints = points;
            //Lastplay = lastplay;
        }
        /// <summary>
        /// 첫판일경우
        /// </summary>
        /// <param name="name"></param>
        public Riot_ChampMatery(Champion name)
        {
            Champion = name;
            ChampionLevel = 0;
            ChampionPoints = 0;
            //Lastplay = DateTime.;
        }
        public Riot_ChampMatery()
        {
            Champion = 0;
            ChampionLevel = 0;
            ChampionPoints = 0;
            //Lastplay = DateTime.;
        }
    }
}
