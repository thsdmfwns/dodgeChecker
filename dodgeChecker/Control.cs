using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Control
    {
        public Dictionary<int, Riot> SummornerData = new Dictionary<int, Riot>();
        LCU_Control lcu = new LCU_Control();
        public event EventHandler GetInPick;
        public event EventHandler ChangeChamp;
        public event EventHandler EndPick;
        string RiotApi_Key = "RGAPI-63f74ad9-11d2-49f0-bc49-ad657e5da701";
        public Control()
        {
            lcu.SendLCUDatas += Lcu_ChampionSelectChanged;
        }

        private void Lcu_ChampionSelectChanged(object sender, EventArgs e)
        {
            var LCUDatas = (List<LCU_SummonerData>)sender;
            if (LCUDatas != null)
            {
                if (SummornerData.Count == 0)
                {
                    LCUToRiot(LCUDatas);
                    GetInPick(null, null);
                }
                else
                {
                    for (int i = 0; i < LCUDatas.Count; i++)
                    {
                        int id = LCUDatas[i].ChampionId;
                        SummornerData[i].ChampMatery = SummornerData[i].GetChampionMastery(id);
                    }
                    ChangeChamp(null, null);
                }
            }
            else
            {
                EndPick(null, null);
                SummornerData.Clear();
            }


        }

        public void StartThread()
        {
            lcu.StartThread();
        }

        public async Task ConnectClientAsync()
        {
             await lcu.ConnectLCUAsync();
        }
        private void LCUToRiot(List<LCU_SummonerData> LCU)
        {

            foreach (var item in LCU)
            {
                Riot riot = new Riot(RiotApi_Key);
                riot.GetRiotDatas(item);
                SummornerData.Add(item.PickIndex, riot);
            }
        }

    }
}
