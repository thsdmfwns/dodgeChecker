using LCUSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class LCU
    {
        public ILeagueClient Client { get; private set; }
        public string CurrentedSummoner { get; private set; }
        public async Task connectClientAsync()
        {
            Client =  await LeagueClient.Connect(@"C:\Riot Games\League of Legends");
            //CurrentedSummoner = Client.GetSummonersModule().GetCurrentSummoner().DisplayName;
        }

        public List<LCU_SummonerData> GetSummerData(JToken session)
        {
            List<LCU_SummonerData> MyTeam = new List<LCU_SummonerData>();
            var myteam = session;
            if (myteam == null)
                return null;
            int i = 0;
            foreach (var item in myteam)
            {
               
                int cid = int.Parse(item["championId"].ToString());
                var sumid = item["summonerId"].ToString();
                int sid = int.Parse(item["summonerId"].ToString());
                string sname = "";
                if (sumid != "0")
                {
                    var sum = Client.GetHttpClient().GetAsync("lol-summoner/v1/summoners/" + sumid).Result.Content.ReadAsStringAsync().Result;
                    var sumjson = JObject.Parse(sum);
                    var sumname = sumjson["displayName"];
                    sname = sumname.ToString();
                }
                else
                {
                    sname = "BOT";
                }
                MyTeam.Add(new LCU_SummonerData(i,cid, sid, sname));
                i++;
            }
            return MyTeam;
        }

    }
}
