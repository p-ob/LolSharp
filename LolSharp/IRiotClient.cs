namespace LolSharp
{
    using System.Collections.Generic;
    using RiotDtos.ChampionMastery;
    using RiotDtos.CurrentGame;
    using RiotDtos.Game;
    using RiotDtos.Match;
    using RiotDtos.MatchList;
    using RiotDtos.Stats;
    using RiotDtos.Summoner;
    using RiotDtos.League;

    public interface IRiotClient
    {
        SummonerDto GetSummoner(long summonerId);
        SummonerDto GetSummonerByName(string summonerName);
        Dictionary<string, SummonerDto> GetSummoners(IEnumerable<long> summonerIds);
        Dictionary<string, SummonerDto> GetSummonersByName(IEnumerable<string> summonerNames);
        MatchList GetMatchList(long summonerId);
        MatchDetail GetMatch(long matchId);
        RankedStatsDto GetRankedStats(long summonerId);
        PlayerStatsSummaryListDto GetSummaryStats(long summonerId);
        RecentGamesDto GetRecentGames(long summonerId);
        ChampionMasteryDto GetChampionMasteryForChampion(long summonerId, long championId);
        List<ChampionMasteryDto> GetChampionMasteries(long summonerId);
        int GetChampionMasteryTotalScore(long summonerId);
        List<ChampionMasteryDto> GetChampionMasteryTopChampions(long summonerId);
        CurrentGameInfo GetCurrentGame(long summonerId);
        RiotDtos.StaticData.ChampionListDto GetChampionsStaticData();
        RiotDtos.StaticData.ChampionDto GetChampionStaticData(int championId);
        RiotDtos.Champion.ChampionListDto GetChampionsData();
        RiotDtos.Champion.ChampionDto GetChampionData(long championId);
        List<LeagueDto> GetLeaguesForSummoner(long summonerId);
        Dictionary<string, List<LeagueDto>> GetLeaguesForSummoners(IEnumerable<long> summonerIds);
    }
}
