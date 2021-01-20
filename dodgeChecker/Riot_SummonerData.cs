using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Riot_SummonerData
    {
        public string ID { get; private set; }
        public string AccountID { get; private set; }
        public string Name { get; private set; }

        public Riot_SummonerData(string id, string accountid, string name)
        {
            ID = id;
            AccountID = accountid;
            Name = name;
        }

   

    }
}
