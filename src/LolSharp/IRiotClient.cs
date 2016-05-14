namespace LolSharp
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RiotObjects.ChampionMastery;
    using RiotObjects.CurrentGame;
    using RiotObjects.FeaturedGame;
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
        Task<MatchDetail> GetMatch(long matchId, bool? includeTimeline = null);

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
        Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId, int? count = null);

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
        /// /api/lol/static-data/{region}/v1.2/item
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="itemListData"></param>
        /// <returns></returns>
        Task<ItemListDto> GetItems(string locale = null, string version = null, string itemListData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/item/{id}
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="itemData"></param>
        /// <returns></returns>
        Task<ItemDto> GetItem(int itemId, string locale = null, string version = null, string itemData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/language-strings
        /// </summary>
        /// <returns></returns>
        Task<LanguageStringsDto> GetLanguageStrings();

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/languages
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetLanguages();

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/map
        /// </summary>
        /// <returns></returns>
        Task<MapDataDto> GetMaps();

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/mastery
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="masteryListData"></param>
        /// <returns></returns>
        Task<MasteryListDto> GetMasteries(string locale = null, string version = null, string masteryListData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/mastery/{id}
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="masteryData"></param>
        /// <returns></returns>
        Task<MasteryDto> GetMastery(int masterId, string locale = null, string version = null, string masteryData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/realm
        /// </summary>
        /// <returns></returns>
        Task<RealmDto> GetRealms();

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/rune
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="runeListData"></param>
        /// <returns></returns>
        Task<RuneListDto> GetRunes(string locale = null, string version = null, string runeListData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/rune/{id}
        /// </summary>
        /// <param name="runeId"></param>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="runeData"></param>
        /// <returns></returns>
        Task<RuneDto> GetRune(int runeId, string locale = null, string version = null, string runeData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/summoner-spell
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="spellData"></param>
        /// <returns></returns>
        Task<SummonerSpellListDto> GetSummonerSpells(string locale = null, string version = null, string spellData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/summoner-spell/{id}
        /// </summary>
        /// <param name="summonerSpellId"></param>
        /// <param name="locale"></param>
        /// <param name="version"></param>
        /// <param name="spellData"></param>
        /// <returns></returns>
        Task<SummonerSpellDto> GetSummonerSpell(int summonerSpellId, string locale = null, string version = null, string spellData = null);

        /// <summary>
        /// /api/lol/static-data/{region}/v1.2/versions
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetVersions();

        /// <summary>
        /// /api/lol/{region}/v1.2/champion
        /// </summary>
        /// <returns></returns>
        Task<RiotObjects.Champion.ChampionListDto> GetChampionsData(bool? freeToPlay = null);

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

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-summoner/{summonerIds}/entry
        /// </summary>
        /// <param name="summonerId"></param>
        /// <returns></returns>
        Task<List<LeagueDto>> GetLeagueEntriesForSummoner(long summonerId);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-summoner/{summonerIds}/entry
        /// </summary>
        /// <param name="summonerIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<LeagueDto>>> GetLeagueEntriesForSummoners(IEnumerable<long> summonerIds);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-team/{teamIds}
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<List<LeagueDto>> GetLeaguesForTeam(long teamId);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-team/{teamIds}
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForTeams(IEnumerable<long> teamIds);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-team/{teamIds}/entry
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<List<LeagueDto>> GetLeagueEntriesForTeam(long teamId);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/by-team/{teamIds}/entry
        /// </summary>
        /// <param name="teamIds"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<LeagueDto>>> GetLeagueEntriesForTeams(IEnumerable<long> teamIds);

        /// <summary>
        /// /api/lol/{region}/v2.5/league/challenger
        /// </summary>
        /// <returns></returns>
        Task<LeagueDto> GetChallengerLeague();

        /// <summary>
        /// /api/lol/{region}/v2.5/league/master
        /// </summary>
        /// <returns></returns>
        Task<LeagueDto> GetMasterLeague();

        /// <summary>
        /// /observer-mode/rest/featured
        /// </summary>
        /// <returns></returns>
        Task<FeaturedGames> GetFeaturedGames();
    }
}
