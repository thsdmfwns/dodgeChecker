using MingweiSamuel.Camille.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Riot_Match
    {
        //var win = participant.Stats.Win;
        //var champ = (Champion)participant.ChampionId;

        //var k = participant.Stats.Kills;
        //var d = participant.Stats.Deaths;
        //var a = participant.Stats.Assists;
        //var kda = (k + a) / (float)d;
        public bool Win { get; set; }
        public Champion Champ { get; set; }
        public int Kill { get; set; }
        public int Death { get; set; }
        public int Assist { get; set; }
        public double KDA { get; private set; }

        public Riot_Match(bool win, int champ, int k ,int d, int a)
        {
            Win = win;
            Champ = (Champion)champ;
            Kill = k;
            Death = d;
            Assist = a;
            KDA = (double)(k + a) / (double)d;
        }
        public Riot_Match()
        {
            Win = false;
            Champ = 0;
            Kill = 0;
            Death = 0;
            Assist = 0;
            KDA = 0;
        }
        public void print()
        {
            string result = Win ? "이김" : "패배";
            Console.WriteLine("결과 : "+ result+"/// 챔피언 : "+Champ.Name()+"/// kda : "+KDA);
        }
    }
}
