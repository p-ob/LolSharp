namespace LolSharp
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RiotObjects.ChampionMastery;
    using RiotObjects.CurrentGame;
    using RiotObjects.Game;
    using RiotObjects.League;
    using RiotObjects.Match;
    using RiotObjects.MatchList;
    using RiotObjects.StaticData;
    using RiotObjects.Stats;
    using RiotObjects.Summoner;

    public interface IRiotClient
    {
        /// <summary>
        /// /api/lol/{region}/v1.4/summoner/{summonerIds}
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<SummonerDto> GetSummoner(long summonerId);

        /// <summary>
        /// /api/lol/{region}/v1.4/summoner/by-name/{summonerNames}
        /// </summary>
        /// <param name="summonerName"></param>
        /// <returns></returns>
        Task<SummonerDto> GetSummonerByName(string summonerName);

        /// <summary>
        /// /api/lol/{region}/v1.4/summoner/{summonerIds}
        /// </summary>
        /// <param name="summonerIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, SummonerDto>> GetSummoners(IEnumerable<long> summonerIds);

        /// <summary>
        /// /api/lol/{region}/v1.4/summoner/by-name/{summonerNames}
        /// </summary>
        /// <param name="summonerNames"></param>
        /// <returns></returns>
        Task<Dictionary<string, SummonerDto>> GetSummonersByName(IEnumerable<string> summonerNames);

        /// <summary>
        /// /api/lol/{region}/v2.2/matchlist/by-summoner/{summonerId}
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="summonerIds"></param>
        /// <param name="rankedQueues"></param>
        /// <param name="seasons"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="beginIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        Task<MatchList> GetMatchList(long summonerId, IEnumerable<long> summonerIds = null, IEnumerable<string> rankedQueues = null, IEnumerable<string> seasons = null, long? beginTime = null, long? endTime = null, int? beginIndex = null, int? endIndex = null);

        /// <summary>
        /// /api/lol/{region}/v2.2/match/{matchId}
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="includeTimeline"></param>
        /// <returns></returns>
        Task<MatchDetail> GetMatch(long matchId, bool includeTimeline = false);

        /// <summary>
        /// /api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/ranked
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        Task<RankedStatsDto> GetRankedStats(long summonerId, string season = null);

        /// <summary>
        /// /api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/summary
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        Task<PlayerStatsSummaryListDto> GetSummaryStats(long summonerId, string season = null);

        /// <summary>
        /// /api/lol/{region}/v1.3/game/by-summoner/{summonerId}/recent
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<RecentGamesDto> GetRecentGames(long summonerId);

        /// <summary>
        /// /championmastery/location/{platformId}/player/{playerId}/champion/{championId}
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="championId"></param>
        /// <returns></returns>
        Task<ChampionMasteryDto> GetChampionMasteryForChampion(long summonerId, long championId);

        /// <summary>
        /// /championmastery/location/{platformId}/player/{playerId}/champions
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<List<ChampionMasteryDto>> GetChampionMasteries(long summonerId);

        /// <summary>
        /// /championmastery/location/{platformId}/player/{playerId}/score
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<int> GetChampionMasteryTotalScore(long summonerId);

        /// <summary>
        /// /championmastery/location/{platformId}/player/{playerId}/topchampions
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId, int count = 3);

        /// <summary>
        /// /observer-mode/rest/consumer/getSpectatorGameInfo/{platformId}/{summonerId}
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<CurrentGameInfo> GetCurrentGame(long summonerId);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/champion
        /// </summary>
        /// <returns></returns>
        Task<ChampionListDto> GetChampionsStaticData(string locale = null, string version = null, string dataById = null, string champData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/champion/{id}
        /// </summary>
        /// <param name="championId"></param>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="dataById"></param>
        /// <param name="champData"></param>
        /// <returns></returns>
        Task<ChampionDto> GetChampionStaticData(int championId, string locale = null, string version = null, string dataById = null, string champData = null);

        /// <summary>
        /// /api/lol/{region}/v1.2/champion
        /// </summary>
        /// <returns></returns>
        Task<RiotObjects.Champion.ChampionListDto> GetChampionsData(bool freeToPlay = false);

        /// <summary>
        /// /api/lol/{region}/v1.2/champion/{id}
        /// </summary>
        /// <param name="championId"></param>
        /// <returns></returns>
        Task<RiotObjects.Champion.ChampionDto> GetChampionData(long championId);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-summoner/{summonerIds}
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<List<LeagueDto>> GetLeaguesForSummoner(long summonerId);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-summoner/{summonerIds}
        /// </summary>
        /// <param name="summonerIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForSummoners(IEnumerable<long> summonerIds);
    }
}
