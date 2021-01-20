using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class LCU_SummonerData
    {
        public int PickIndex { get; set; }
        public int ChampionId { get; set; }
        public int SummonerId { get; set; }
        public string SummonerName { get; set; }

        public LCU_SummonerData(int index,int cid, int sid, string name)
        {
            PickIndex = index;
            ChampionId = cid;
            SummonerId = sid;
            SummonerName = name;
        }
        //콘솔용
        public void print ()
        {
            Console.WriteLine("==========");
            Console.WriteLine("픽순서  : " + PickIndex);
            Console.WriteLine("챔피언 id : " + ChampionId);
            Console.WriteLine("소환사 이름 : " + SummonerName);
            Console.WriteLine("==========");
        }
    }
}
