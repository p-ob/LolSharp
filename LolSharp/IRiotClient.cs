namespace LolSharp
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        Task<SummonerDto> GetSummoner(long summonerId);

        Task<SummonerDto> GetSummonerByName(string summonerName);

        Task<Dictionary<string, SummonerDto>> GetSummoners(IEnumerable<long> summonerIds);

        Task<Dictionary<string, SummonerDto>> GetSummonersByName(IEnumerable<string> summonerNames);

        Task<MatchList> GetMatchList(long summonerId);

        Task<MatchDetail> GetMatch(long matchId);

        Task<RankedStatsDto> GetRankedStats(long summonerId);

        Task<PlayerStatsSummaryListDto> GetSummaryStats(long summonerId);

        Task<RecentGamesDto> GetRecentGames(long summonerId);

        Task<ChampionMasteryDto> GetChampionMasteryForChampion(long summonerId, long championId);

        Task<List<ChampionMasteryDto>> GetChampionMasteries(long summonerId);

        Task<int> GetChampionMasteryTotalScore(long summonerId);

        Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId);

        Task<CurrentGameInfo> GetCurrentGame(long summonerId);

        Task<RiotDtos.StaticData.ChampionListDto> GetChampionsStaticData();

        Task<RiotDtos.StaticData.ChampionDto> GetChampionStaticData(int championId);

        Task<RiotDtos.Champion.ChampionListDto> GetChampionsData();

        Task<RiotDtos.Champion.ChampionDto> GetChampionData(long championId);

        Task<List<LeagueDto>> GetLeaguesForSummoner(long summonerId);

        Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForSummoners(IEnumerable<long> summonerIds);
    }
}
