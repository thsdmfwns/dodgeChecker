using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleApp9;
using MingweiSamuel.Camille.Enums;

namespace dodgeChecker
{
    public partial class Form1 : Form
    {
        ConsoleApp9.Control con;
        public Form1()
        {
            InitializeComponent();

        }
        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            con = new ConsoleApp9.Control();
            await con.ConnectClientAsync();
            con.StartThread();
            con.GetInPick += When_GetInPick;
            con.ChangeChamp += When_ChangeChamp;
            con.EndPick += When_EndPick;
            CSafeSetText(Pick0_Rank_LB, "등급 : ");
            CSafeSetText(Pick1_Rank_LB, "등급 : ");
            CSafeSetText(Pick2_Rank_LB, "등급 : ");
            CSafeSetText(Pick3_Rank_LB, "등급 : ");
            CSafeSetText(Pick4_Rank_LB, "등급 : ");
        }

        delegate void CrossThreadSafetySetText(System.Windows.Forms.Control ctl, String text);
        private void CSafeSetText(System.Windows.Forms.Control ctl, String text)
        {
            /*
             * InvokeRequired 속성 (Control.InvokeRequired, MSDN)
             *   짧게 말해서, 이 컨트롤이 만들어진 스레드와 현재의 스레드가 달라서
             *   컨트롤에서 스레드를 만들어야 하는지를 나타내는 속성입니다.  
             * 
             * InvokeRequired 속성의 값이 참이면, 컨트롤에서 스레드를 만들어 텍스트를 변경하고,
             * 그렇지 않은 경우에는 그냥 변경해도 아무 오류가 없기 때문에 텍스트를 변경합니다.
             */
            if (ctl.InvokeRequired)
                ctl.Invoke(new CrossThreadSafetySetText(CSafeSetText), ctl, text);
            else
                ctl.Text = text;
        }

        delegate void CrossThreadSafetyAddListVew(ListView ctl, String[] text);
        private void CSafeSetAddListVew(ListView ctl, String[] text)
        {
            /*
             * InvokeRequired 속성 (Control.InvokeRequired, MSDN)
             *   짧게 말해서, 이 컨트롤이 만들어진 스레드와 현재의 스레드가 달라서
             *   컨트롤에서 스레드를 만들어야 하는지를 나타내는 속성입니다.  
             * 
             * InvokeRequired 속성의 값이 참이면, 컨트롤에서 스레드를 만들어 텍스트를 변경하고,
             * 그렇지 않은 경우에는 그냥 변경해도 아무 오류가 없기 때문에 텍스트를 변경합니다.
             */
            if (ctl.InvokeRequired)
                ctl.Invoke(new CrossThreadSafetyAddListVew(CSafeSetAddListVew), ctl, text);
            else
                ctl.Items.Add(new ListViewItem(text));
        }

        delegate void CrossThreadSafetySetPictureBox(PictureBox ctl, String text);
        private void CSafeSetPictureBox(PictureBox ctl, String text)
        {
            /*
             * InvokeRequired 속성 (Control.InvokeRequired, MSDN)
             *   짧게 말해서, 이 컨트롤이 만들어진 스레드와 현재의 스레드가 달라서
             *   컨트롤에서 스레드를 만들어야 하는지를 나타내는 속성입니다.  
             * 
             * InvokeRequired 속성의 값이 참이면, 컨트롤에서 스레드를 만들어 텍스트를 변경하고,
             * 그렇지 않은 경우에는 그냥 변경해도 아무 오류가 없기 때문에 텍스트를 변경합니다.
             */
            if (ctl.InvokeRequired)
                ctl.Invoke(new CrossThreadSafetySetPictureBox(CSafeSetPictureBox), ctl, text);
            else
            {
                ctl.SizeMode = PictureBoxSizeMode.Zoom;
                ctl.ImageLocation = text;
            }

        }

        delegate void CrossThreadSafetyResetListVew(ListView ctl);
        private void CSafeSetResetListVew(ListView ctl)
        {
            /*
             * InvokeRequired 속성 (Control.InvokeRequired, MSDN)
             *   짧게 말해서, 이 컨트롤이 만들어진 스레드와 현재의 스레드가 달라서
             *   컨트롤에서 스레드를 만들어야 하는지를 나타내는 속성입니다.  
             * 
             * InvokeRequired 속성의 값이 참이면, 컨트롤에서 스레드를 만들어 텍스트를 변경하고,
             * 그렇지 않은 경우에는 그냥 변경해도 아무 오류가 없기 때문에 텍스트를 변경합니다.
             */
            if (ctl.InvokeRequired)
                ctl.Invoke(new CrossThreadSafetyResetListVew(CSafeSetResetListVew), ctl);
            else
                ctl.Items.Clear();
        }


        private void When_EndPick(object sender, EventArgs e)
        {
            ResetAllControl();
        }

        private void When_ChangeChamp(object sender, EventArgs e)
        {
            var data = con.SummornerData;
            foreach (var item in data)
            {
                UpdateUI_ChangeChamp(item.Key);
            }
        }

        private void When_GetInPick(object sender, EventArgs e)
        {
            var data = con.SummornerData;
            foreach (var item in data)
            {
                Thread t2 = new Thread(new ParameterizedThreadStart(UpdateUI_GetInPick));
                t2.Start(item.Key);
            }
        }

        private void FileSearch_Click(object sender, EventArgs e)
        {
            ShowOpenSearchDlg();
        }
        private void ShowOpenSearchDlg()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowDialog();
        }

        #region UI_ChangeChamp
        private void UpdateUI_ChangeChamp(int index)
        {
            switch (index)
            {
                case 0:
                    UpdateUI_0_champ();
                    break;
                case 1:
                    UpdateUI_1_champ();
                    break;
                case 2:
                    UpdateUI_2_champ();
                    break;
                case 3:
                    UpdateUI_3_champ();
                    break;
                case 4:
                    UpdateUI_4_champ();
                    break;
            }
        }
        private void UpdateUI_0_champ()
        {
            var data = con.SummornerData[0];
            var score = GetScore(data);
            var rank = (ScoreRank)GetScoreRank(score);
            CSafeSetText(Pick0_Score_LB, "점수 : "+score.ToString());
            CSafeSetText(Pick0_Rank_LB, "등급 : " + rank.ToString());
            CSafeSetText(Pick0_ChampLV_LB, data.ChampMatery.ChampionLevel.ToString());
            CSafeSetText(Pick0_ChampPoint_LB, data.ChampMatery.ChampionPoints.ToString());
            var champId = (long)data.ChampMatery.Champion;
            string path = string.Format("championPicture\\{0}.jpg", champId);
            string Rpath = string.Format("Rank\\{0}.png", rank.ToString());
            CSafeSetPictureBox(Pick0_Champion_PB, path);
            CSafeSetPictureBox(Pick0_Rank_PB, Rpath);
        }
        private void UpdateUI_1_champ()
        {
            var data = con.SummornerData[1];
            var score = GetScore(data);
            var rank = (ScoreRank)GetScoreRank(score);
            CSafeSetText(Pick1_Score_LB, "점수 : " + score.ToString());
            CSafeSetText(Pick1_Rank_LB, "등급 : " + rank.ToString());
            CSafeSetText(Pick1_ChampLV_LB, data.ChampMatery.ChampionLevel.ToString());
            CSafeSetText(Pick1_ChampPoint_LB, data.ChampMatery.ChampionPoints.ToString());
            var champId = (long)data.ChampMatery.Champion;
            string path = string.Format("championPicture\\{0}.jpg", champId);
            string Rpath = string.Format("Rank\\{0}.png", rank.ToString());
            CSafeSetPictureBox(Pick1_Champion_PB, path);
            CSafeSetPictureBox(Pick1_Rank_PB, Rpath);
        }
        private void UpdateUI_2_champ()
        {
            var data = con.SummornerData[2];
            var score = GetScore(data);
            var rank = (ScoreRank)GetScoreRank(score);
            CSafeSetText(Pick2_Score_LB, "점수 : " + score.ToString());
            CSafeSetText(Pick2_Rank_LB, "등급 : " + rank.ToString());
            CSafeSetText(Pick2_ChampLV_LB, data.ChampMatery.ChampionLevel.ToString());
            CSafeSetText(Pick2_ChampPoint_LB, data.ChampMatery.ChampionPoints.ToString());
            var champId = (long)data.ChampMatery.Champion;
            string path = string.Format("championPicture\\{0}.jpg", champId);
            string Rpath = string.Format("Rank\\{0}.png", rank.ToString());
            CSafeSetPictureBox(Pick2_Champion_PB, path);
            CSafeSetPictureBox(Pick2_Rank_PB, Rpath);
        }
        private void UpdateUI_3_champ()
        {
            var data = con.SummornerData[3];
            var score = GetScore(data);
            var rank = (ScoreRank)GetScoreRank(score);
            CSafeSetText(Pick3_Score_LB, "점수 : " + score.ToString());
            CSafeSetText(Pick3_Rank_LB, "등급 : " + rank.ToString());
            CSafeSetText(Pick3_ChampLV_LB, data.ChampMatery.ChampionLevel.ToString());
            CSafeSetText(Pick3_ChampPoint_LB, data.ChampMatery.ChampionPoints.ToString());
            var champId = (long)data.ChampMatery.Champion;
            string path = string.Format("championPicture\\{0}.jpg", champId);
            string Rpath = string.Format("Rank\\{0}.png", rank.ToString());
            CSafeSetPictureBox(Pick3_Champion_PB, path);
            CSafeSetPictureBox(Pick3_Rank_PB, Rpath);
        }
        private void UpdateUI_4_champ()
        {
            var data = con.SummornerData[4];
            var score = GetScore(data);
            var rank = (ScoreRank)GetScoreRank(score);
            CSafeSetText(Pick4_Score_LB, "점수 : " + score.ToString());
            CSafeSetText(Pick4_Rank_LB, "등급 : " + rank.ToString());
            CSafeSetText(Pick4_ChampLV_LB, data.ChampMatery.ChampionLevel.ToString());
            CSafeSetText(Pick4_ChampPoint_LB, data.ChampMatery.ChampionPoints.ToString());
            var champId = (long)data.ChampMatery.Champion;
            string path = string.Format("championPicture\\{0}.jpg",champId);
            string Rpath = string.Format("Rank\\{0}.png", rank.ToString());
            CSafeSetPictureBox(Pick4_Champion_PB, path);
            CSafeSetPictureBox(Pick4_Rank_PB, Rpath);
        }

        public int GetScore(object dat)
        {
            var data = (Riot)dat;
            int score = 100;
            foreach (var item in data.Matches)
            {
                if (item.Win == false)
                    score -= 10;
            }
            if (data.ChampMatery.ChampionLevel == 0 || data.ChampMatery.ChampionPoints == 0)
            {
                score -= 30;
            }
            else if (data.ChampMatery.ChampionLevel == 1)
            {
                score -= 20;
            }
            else if (data.ChampMatery.ChampionLevel == 2)
            {
                score -= 10;
            }
            else
            {
                score += data.ChampMatery.ChampionLevel * 7;
            }
           
            if (data.ChampMatery.ChampionPoints > 150000)
            {
                score += 10;
            }
            if (data.ChampMatery.ChampionPoints > 350000)
            {
                score += 20;
            }

            if (data.SoloRankStatus.Per <45.0)
            {
                score -= 30;
            }
            else if(data.SoloRankStatus.Per < 50.0)
            {
                score -= 10;
            }
            else if (data.SoloRankStatus.Per < 55.0)
            {
                score -= 0;
            }
            else if (data.SoloRankStatus.Per < 60.0)
            {
                score += 10;
            }
            else if (data.SoloRankStatus.Per > 60.0)
            {
                score += 30;
            }

            return score;
        }
        public int GetScoreRank(int score)
        {
            if (score < 30)
                return (int)ScoreRank.TROLL;
            else if (score < 50)
                return (int)ScoreRank.BABY;
            else if (score < 75)
                return (int)ScoreRank.NOMAL;
            else if (score < 100)
                return (int)ScoreRank.GOSU;
            else if (score > 100)
                return (int)ScoreRank.BUSDRIVER;

            return 0;

        }
        #endregion
        #region GetInpick UI
        private void UpdateUI_GetInPick(object index)
        {

            try
            {
                switch ((int)index)
                {
                    case 0:
                        UpdateUI_0();
                        break;
                    case 1:
                        UpdateUI_1();
                        break;
                    case 2:
                        UpdateUI_2();
                        break;
                    case 3:
                        UpdateUI_3();
                        break;
                    case 4:
                        UpdateUI_4();
                        break;
                }
             }
            catch (Exception)
            {
            }
        }
        private void UpdateUI_0()
        {
            var data = con.SummornerData[0];
            CSafeSetText(Pick0_Name_LB, data.SummonerData.Name);
            CSafeSetText(Pick0_asd_Tier_LB, data.SoloRankStatus.Tier + " " + data.SoloRankStatus.Rank);
            CSafeSetText(Pick0_Wins_LB, data.SoloRankStatus.Wins.ToString());
            CSafeSetText(Pick0_Per_LB, string.Format("{0}%", data.SoloRankStatus.Per.ToString("F2")));
            CSafeSetText(Pick0_Losses_LB, data.SoloRankStatus.Losses.ToString());
            var path = string.Format("TierIcon\\{0}.png", data.SoloRankStatus.Tier);
            CSafeSetPictureBox(Pick0_Tier_PB, path);
            UpdateUI_Maches_0(data);
        }
        private void UpdateUI_1()
        {
            var data = con.SummornerData[1];
            CSafeSetText(Pick1_Name_LB, data.SummonerData.Name);
            CSafeSetText(Pick1_Tier_LB, data.SoloRankStatus.ToString_Tier());
            CSafeSetText(Pick1_Wins_LB, data.SoloRankStatus.Wins.ToString());
            CSafeSetText(Pick1_Per_LB, string.Format("{0}%", data.SoloRankStatus.Per.ToString("F2")));
            CSafeSetText(Pick1_Losses_LB, data.SoloRankStatus.Losses.ToString());
            var path = string.Format("TierIcon\\{0}.png", data.SoloRankStatus.Tier);
            CSafeSetPictureBox(Pick1_Tier_PB, path);
            UpdateUI_Maches_1(data);
        }
        private void UpdateUI_2()
        {
            var data = con.SummornerData[2];
            CSafeSetText(Pick2_Name_LB, data.SummonerData.Name);
            CSafeSetText(Pick2_Tier_LB, data.SoloRankStatus.ToString_Tier());
            CSafeSetText(Pick2_Wins_LB, data.SoloRankStatus.Wins.ToString());
            CSafeSetText(Pick2_Per_LB, string.Format("{0}%", data.SoloRankStatus.Per.ToString("F2")));
            CSafeSetText(Pick2_Losses_LB, data.SoloRankStatus.Losses.ToString());
            var path = string.Format("TierIcon\\{0}.png", data.SoloRankStatus.Tier);
            CSafeSetPictureBox(Pick2_Tier_PB, path);
            UpdateUI_Maches_2(data);
        }
        private void UpdateUI_3()
        {
            var data = con.SummornerData[3];
            CSafeSetText(Pick3_Name_LB, data.SummonerData.Name);
            CSafeSetText(Pick3_Tier_LB, data.SoloRankStatus.ToString_Tier());
            CSafeSetText(Pick3_Wins_LB, data.SoloRankStatus.Wins.ToString());
            CSafeSetText(Pick3_Per_LB, string.Format("{0}%", data.SoloRankStatus.Per.ToString("F2")));
            CSafeSetText(Pick3_Losses_LB, data.SoloRankStatus.Losses.ToString());
            var path = string.Format("TierIcon\\{0}.png", data.SoloRankStatus.Tier);
            CSafeSetPictureBox(Pick3_Tier_PB, path);
            UpdateUI_Maches_3(data);
        }
        private void UpdateUI_4()
        {
            var data = con.SummornerData[4];
            CSafeSetText(Pick4_Name_LB, data.SummonerData.Name);
            CSafeSetText(Pick4_Tier_LB, data.SoloRankStatus.ToString_Tier());
            CSafeSetText(Pick4_Wins_LB, data.SoloRankStatus.Wins.ToString());
            CSafeSetText(Pick4_Per_LB, string.Format("{0}%", data.SoloRankStatus.Per.ToString("F2")));
            CSafeSetText(Pick3_Losses_LB, data.SoloRankStatus.Losses.ToString());
            var path = string.Format("TierIcon\\{0}.png", data.SoloRankStatus.Tier);
            CSafeSetPictureBox(Pick4_Tier_PB, path);
            UpdateUI_Maches_4(data);

        }
        private void UpdateUI_Maches_0(Riot data)
        {
            var matches = data.Matches;
            List<string> list = new List<string>();
            foreach (var item in matches)
            {
                list.Clear();
                var win = item.Win ? "승리" : "패배";
                list.Add(win);
                list.Add(item.Champ.Name());
                var kda = string.Format("{0}/{1}/{2}", item.Kill, item.Death, item.Assist);
                list.Add(kda);
                list.Add(item.KDA.ToString("F2"));
                string[] str = list.ToArray();
                CSafeSetAddListVew(Pick0_Maches_LV, str);
            }
        }
        private void UpdateUI_Maches_1(Riot data)
        {
            var matches = data.Matches;
            List<string> list = new List<string>();
            foreach (var item in matches)
            {
                list.Clear();
                var win = item.Win ? "승리" : "패배";
                list.Add(win);
                list.Add(item.Champ.Name());
                var kda = string.Format("{0}/{1}/{2}", item.Kill, item.Death, item.Assist);
                list.Add(kda);
                list.Add(item.KDA.ToString("F2"));
                string[] str = list.ToArray();
                CSafeSetAddListVew(Pick1_Maches_LV, str);
            }
        }
        private void UpdateUI_Maches_2(Riot data)
        {
            var matches = data.Matches;
            List<string> list = new List<string>();
            foreach (var item in matches)
            {
                list.Clear();
                var win = item.Win ? "승리" : "패배";
                list.Add(win);
                list.Add(item.Champ.Name());
                var kda = string.Format("{0}/{1}/{2}", item.Kill, item.Death, item.Assist);
                list.Add(kda);
                list.Add(item.KDA.ToString("F2"));
                string[] str = list.ToArray();
                CSafeSetAddListVew(Pick2_Maches_LV, str);
            }
        }
        private void UpdateUI_Maches_3(Riot data)
        {
            var matches = data.Matches;
            List<string> list = new List<string>();
            foreach (var item in matches)
            {
                list.Clear();
                var win = item.Win ? "승리" : "패배";
                list.Add(win);
                list.Add(item.Champ.Name());
                var kda = string.Format("{0}/{1}/{2}", item.Kill, item.Death, item.Assist);
                list.Add(kda);
                list.Add(item.KDA.ToString("F2"));
                string[] str = list.ToArray();
                CSafeSetAddListVew(Pick3_Maches_LV, str);
            }
        }
        private void UpdateUI_Maches_4(Riot data)
        {
            var matches = data.Matches;
            List<string> list = new List<string>();
            foreach (var item in matches)
            {
                list.Clear();
                var win = item.Win ? "승리" : "패배";
                list.Add(win);
                list.Add(item.Champ.Name());
                var kda = string.Format("{0}/{1}/{2}", item.Kill, item.Death, item.Assist);
                list.Add(kda);
                list.Add(item.KDA.ToString("F2"));
                string[] str = list.ToArray();
                CSafeSetAddListVew(Pick4_Maches_LV, str);
            }
        }


        #endregion
        private void ResetAllControl()
        {
            CSafeSetText(Pick0_ChampLV_LB, "");
            CSafeSetText(Pick0_ChampPoint_LB, "");
            CSafeSetText(Pick1_ChampLV_LB, "");
            CSafeSetText(Pick1_ChampPoint_LB, "");
            CSafeSetText(Pick2_ChampLV_LB, "");
            CSafeSetText(Pick2_ChampPoint_LB, "");
            CSafeSetText(Pick3_ChampLV_LB, "");
            CSafeSetText(Pick3_ChampPoint_LB, "");
            CSafeSetText(Pick4_ChampLV_LB, "");
            CSafeSetText(Pick4_ChampPoint_LB, "");
            //
            CSafeSetText(Pick0_asd_Tier_LB, "");
            CSafeSetText(Pick0_Wins_LB, "");
            CSafeSetText(Pick0_Per_LB, "");
            CSafeSetText(Pick0_Losses_LB, "");
            CSafeSetText(Pick1_Tier_LB, "");
            CSafeSetText(Pick1_Wins_LB, "");
            CSafeSetText(Pick1_Per_LB, "");
            CSafeSetText(Pick1_Losses_LB, "");
            CSafeSetText(Pick2_Tier_LB, "");
            CSafeSetText(Pick2_Wins_LB, "");
            CSafeSetText(Pick2_Per_LB, "");
            CSafeSetText(Pick2_Losses_LB, "");
            CSafeSetText(Pick3_Tier_LB, "");
            CSafeSetText(Pick3_Wins_LB, "");
            CSafeSetText(Pick3_Per_LB, "");
            CSafeSetText(Pick3_Losses_LB, "");
            CSafeSetText(Pick4_Tier_LB, "");
            CSafeSetText(Pick4_Wins_LB, "");
            CSafeSetText(Pick4_Per_LB, "");
            CSafeSetText(Pick4_Losses_LB, "");
            //
            CSafeSetText(Pick0_Name_LB, "");
            CSafeSetText(Pick1_Name_LB, "");
            CSafeSetText(Pick2_Name_LB, "");
            CSafeSetText(Pick3_Name_LB, "");
            CSafeSetText(Pick4_Name_LB, "");
            //
            CSafeSetPictureBox(Pick0_Tier_PB, "");
            CSafeSetPictureBox(Pick1_Tier_PB, "");
            CSafeSetPictureBox(Pick2_Tier_PB, "");
            CSafeSetPictureBox(Pick3_Tier_PB, "");
            CSafeSetPictureBox(Pick4_Tier_PB, "");
            CSafeSetPictureBox(Pick0_Champion_PB, "");
            CSafeSetPictureBox(Pick1_Champion_PB, "");
            CSafeSetPictureBox(Pick2_Champion_PB, "");
            CSafeSetPictureBox(Pick3_Champion_PB, "");
            CSafeSetPictureBox(Pick4_Champion_PB, "");
            //
            CSafeSetResetListVew(Pick0_Maches_LV);
            CSafeSetResetListVew(Pick1_Maches_LV);
            CSafeSetResetListVew(Pick2_Maches_LV);
            CSafeSetResetListVew(Pick3_Maches_LV);
            CSafeSetResetListVew(Pick4_Maches_LV);
            //
            CSafeSetText(Pick0_Score_LB, "점수 : ");
            CSafeSetText(Pick1_Score_LB, "점수 : ");
            CSafeSetText(Pick2_Score_LB, "점수 : ");
            CSafeSetText(Pick3_Score_LB, "점수 : ");
            CSafeSetText(Pick4_Score_LB, "점수 : ");
            //
            CSafeSetText(Pick0_Rank_LB, "등급 : ");
            CSafeSetText(Pick1_Rank_LB, "등급 : ");
            CSafeSetText(Pick2_Rank_LB, "등급 : ");
            CSafeSetText(Pick3_Rank_LB, "등급 : ");
            CSafeSetText(Pick4_Rank_LB, "등급 : ");
            //
            CSafeSetPictureBox(Pick0_Rank_PB, "");
            CSafeSetPictureBox(Pick1_Rank_PB, "");
            CSafeSetPictureBox(Pick2_Rank_PB, "");
            CSafeSetPictureBox(Pick3_Rank_PB, "");
            CSafeSetPictureBox(Pick4_Rank_PB, "");
        }
    }

}
