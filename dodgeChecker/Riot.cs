using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    class Riot
    {
        private RiotApi Api;
        public int PickIndex { get; private set; }
        public Riot_SummonerData SummonerData {get; private set; }
        public Riot_SoloRankStatus SoloRankStatus { get; private set; }
        public Riot_ChampMatery ChampMatery { get; set; }
        public List<Riot_Match> Matches { get; private set; }
        public Riot(string apiKey)
        {
            Api = RiotApi.NewInstance(apiKey);
        }
        public void GetRiotDatas(LCU_SummonerData data)
        {
            SummonerData = GetSummonerData(data.SummonerName);
            SoloRankStatus = GetSoloRankStatus();
            ChampMatery = GetChampionMastery(data.ChampionId);
            PickIndex = data.PickIndex;
            GetMaches();
        }


        private Riot_SummonerData GetSummonerData(string summonerName)
        {
            var summonerData = Api.SummonerV4.GetBySummonerNameAsync(Region.KR, summonerName).Result;
            if (null == summonerData)
            {
                // If a summoner is not found, the response will be null.
                Console.WriteLine($"Summoner '{summonerName}' not found.");
                return null;
            }
            var accountid = summonerData.AccountId;
            var name = summonerData.Name;
            var id = summonerData.Id;
            return new Riot_SummonerData(id, accountid, name);
        }

        private Riot_SoloRankStatus GetSoloRankStatus()
        {
            var league = Api.LeagueV4.GetLeagueEntriesForSummoner(Region.KR, SummonerData.ID);
            if (league == null)
            {
                return new Riot_SoloRankStatus();
            }
            foreach (var item in league)
            {
                if (item.QueueType == "RANKED_SOLO_5x5")
                {
                    var soloRankEntry = item;
                    int wins = soloRankEntry.Wins;
                    int losses = soloRankEntry.Losses;
                    var tier = soloRankEntry.Tier;
                    var Rank = soloRankEntry.Rank;
                    if (tier == null)
                    {
                        Rank = "";
                        tier = "UnRanked";
                    }
                    return new Riot_SoloRankStatus(wins, losses, tier, Rank);
                }
            }
            return new Riot_SoloRankStatus();
        }
        public  void GetMaches()
        {
            Matches =  GetMachesAsync().Result;
        }

        private async Task<List<Riot_Match>> GetMachesAsync()
        {
            List<Riot_Match> matches = new List<Riot_Match>();
            var matchlist = await Api.MatchV4.GetMatchlistAsync(
             Region.KR, SummonerData.AccountID, queue: new[] { 420 }, endIndex: 10);
            // Get match results (done asynchronously -> not blocking -> fast).
            if (matchlist == null)
            {
                matches.Add(new Riot_Match());
                return matches;
            }
            var matchDataTasks = matchlist.Matches.Select(
                   matchMetadata => Api.MatchV4.GetMatchAsync(Region.KR, matchMetadata.GameId)
               ).ToArray();
            // Wait for all task requests to complete asynchronously.
            var matchDatas = await Task.WhenAll(matchDataTasks);

            for (var i = 0; i < matchDatas.Count(); i++)
            {
                var matchData = matchDatas[i];
                // Get this summoner's participant ID info.
                var participantIdData = matchData.ParticipantIdentities
                    .First(pi => SummonerData.ID.Equals(pi.Player.SummonerId));
                // Find the corresponding participant.
                var participant = matchData.Participants
                    .First(p => p.ParticipantId == participantIdData.ParticipantId);

                var win = participant.Stats.Win;
                var champ = participant.ChampionId;

                var k = participant.Stats.Kills;
                var d = participant.Stats.Deaths;
                var a = participant.Stats.Assists;
                matches.Add(new Riot_Match(win, champ, k, d, a));
            }
            return matches;
        }

        public Riot_ChampMatery GetChampionMastery(long championid)
        {
            if (championid == 0)
                return new Riot_ChampMatery();
            var champ = (Champion)championid;
            var mastery =
            Api.ChampionMasteryV4.GetChampionMastery(Region.KR, championid, SummonerData.ID);
            if (mastery == null)
               return new Riot_ChampMatery(champ);
            int points = mastery.ChampionPoints;
            int level = mastery.ChampionLevel;
            return new Riot_ChampMatery(champ, points, level);
        }
    }
}
