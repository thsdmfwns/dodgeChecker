using LCUSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class LCU_Control
    {
        private LCU Lcu;
        private Thread thread_ChampionSelectChange;
        private Thread thread_GameFlowChange;
        private event EventHandler GameFlowChanged;
        private event EventHandler ChampionSelectChanged;
        public event EventHandler SendLCUDatas;
        private string GameFlow = "";

        public LCU_Control()
        {
            Lcu = new LCU();
            GameFlowChanged += Handle_GameFlowChanged;
            ChampionSelectChanged += Handle_ChampionSelectChanged;


        }

        private void Handle_ChampionSelectChanged(object sender, EventArgs e)
        {
            if (GameFlow == "\"ChampSelect\"")
            {
                var myteam = Lcu.GetSummerData((JToken)sender);
                if(myteam != null)
                {
                    SendLCUDatas(myteam, null);
                }
            }
        }

        public async Task ConnectLCUAsync()
        {
              await Lcu.connectClientAsync();
        }
        public void StartThread()
        {
            thread_GameFlowChange = new Thread(Detect_GameFlowChange);
            thread_GameFlowChange.Start();
        }
        private void Handle_GameFlowChanged(object sender, EventArgs e)
        {
            GameFlow = (string)sender;
            if (GameFlow == "\"ChampSelect\"")
            {
                if (thread_ChampionSelectChange == null)
                {
                    thread_ChampionSelectChange = new Thread(Detect_ChampionSelect);
                    thread_ChampionSelectChange.Start();
                }
            }
            else
            {
                if (thread_ChampionSelectChange != null)
                {
                    thread_ChampionSelectChange.Abort();
                }
            }
        }

        private async void Detect_GameFlowChange()
        {
            var Client = Lcu.Client;
            while (true)
            {
                var gameflow = Client.GetHttpClient().GetAsync("/lol-gameflow/v1/gameflow-phase").Result.Content.ReadAsStringAsync().Result;
                if (GameFlow != gameflow)
                {
                    GameFlowChanged(gameflow, null);
                }
                await Task.Delay(103);
            }
        }

        private async void Detect_ChampionSelect()
        {
            var Client = Lcu.Client;
            string temp = "";
            while (true)
            {
                var session = Client.GetHttpClient().GetAsync("/lol-champ-select/v1/session").Result.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(session);
                var myteam = json["myTeam"];
                if (myteam != null)
                {
                    var smyteam = myteam.ToString();
                    if (smyteam != temp)
                        ChampionSelectChanged(myteam, null);
                    temp = smyteam;
                }
                else if(GameFlow == "\"ChampSelect\"")
                {
                    if (session != temp)
                        SendLCUDatas(null, null);
                    temp = session;
                }
                await Task.Delay(10);
            }
        }
    }
}
