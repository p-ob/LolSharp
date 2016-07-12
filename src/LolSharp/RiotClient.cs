namespace LolSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using RestCore;
    using RiotObjects.ChampionMastery;
    using RiotObjects.CurrentGame;
    using RiotObjects.FeaturedGame;
    using RiotObjects.Game;
    using RiotObjects.League;
    using RiotObjects.Match;
    using RiotObjects.MatchList;
    using RiotObjects.StaticData;
    using RiotObjects.Stats;
    using RiotObjects.Status;
    using RiotObjects.Summoner;
    using MasteryDto = RiotObjects.StaticData.MasteryDto;

    public class RiotClient : IRiotClient
    {
        private readonly string _apiKey;
        private readonly bool _debugMode;
        private readonly IRestCore _restClient;

        private RiotRegion _region;

        public RiotRegion Region
        {
            get { return _region; }
            set
            {
                if (!Enum.IsDefined(typeof(RiotRegion), value)) throw new RiotClientException("Region is not valid");
                _region = value;
                ConstructRestClient();
            }
        }

        /// <summary>
        /// Constructor for RiotClient class. RiotClient.Region will default to RiotRegion.Na. 
        /// Will make one api request on instantiation to check the validity of the api key given. This api request has no affect on your rate limit.
        /// </summary>
        /// <param name="apiKey">apik key acquired from: https://developer.riotgames.com</param>
        /// <param name="region">Region to make api requests against. Defaults to NA.</param>
        /// <param name="debugMode">when set to true, all RiotApiExceptions will bring the IRestResponse of the api call (note that the api key will be visible in that object)</param>
        public RiotClient(string apiKey, RiotRegion region = RiotRegion.Na, bool debugMode = false) : this(null, apiKey, region, debugMode)
        {
        }

        /// <summary>
        /// Constructor for RiotClient class with full configuration. Will make one api request on instantiation to check the validity of the api key given.
        /// This api request has no affect on your rate limit.
        /// </summary>
        /// <param name="restClient">If given, uses this instance of RestClient to make all api requests. Otherwise, will create its own RestClient.</param>
        /// <param name="apiKey">apik key acquired from: https://developer.riotgames.com</param>
        /// <param name="region">Region to make api requests against. Defaults to NA.</param>
        /// <param name="debugMode">when set to true, all RiotApiExceptions will bring the IRestResponse of the api call (note that the api key will be visible in that object)</param>
        public RiotClient(IRestCore restClient, string apiKey, RiotRegion region = RiotRegion.Na, bool debugMode = false)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentException("apiKey is required", nameof(apiKey));

            _apiKey = apiKey;
            _debugMode = debugMode;
            _restClient = restClient ?? new RestCore.RestCore();
            Region = region;

            CheckApiKey().Wait();
        }

        public async Task<SummonerDto> GetSummoner(long summonerId, RiotRegion? region = null)
        {
            var summoner = await GetSummoners(new List<long> { summonerId }, region);
            SummonerDto summonerDto;
            return summoner.TryGetValue(summonerId.ToString(), out summonerDto) ? summonerDto : null;
        }

        public async Task<SummonerDto> GetSummonerByName(string summonerName, RiotRegion? region = null)
        {
            var summoner = await GetSummonersByName(new List<string> { summonerName }, region);

            SummonerDto summonerDto;
            return summoner.TryGetValue(summonerName, out summonerDto) ? summonerDto : null;
        }

        public async Task<MasteryPagesDto> GetMasteriesForSummoner(long summonerId, RiotRegion? region = null)
        {
            var masteries = await GetMasteriesForSummoners(new List<long> { summonerId }, region);

            MasteryPagesDto masteriesDto;
            return masteries.TryGetValue(summonerId.ToString(), out masteriesDto) ? masteriesDto : null;
        }

        public async Task<RunePagesDto> GetRunesForSummoner(long summonerId, RiotRegion? region = null)
        {
            var runes = await GetRunesForSummoners(new List<long> { summonerId }, region);

            RunePagesDto runesDto;
            return runes.TryGetValue(summonerId.ToString(), out runesDto) ? runesDto : null;
        }

        public async Task<Dictionary<string, SummonerDto>> GetSummoners(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));

            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.4/summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlParam);

            return await Execute<Dictionary<string, SummonerDto>>(request);
        }

        public async Task<Dictionary<string, SummonerDto>> GetSummonersByName(IEnumerable<string> summonerNames, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerNames = summonerNames.ToList();
            if (summonerNames.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerNames));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.4/summoner/by-name/{summonerNames}" };

            request.AddParameter("summonerNames", string.Join(",", summonerNames), ParameterType.UrlParam);

            var resp = await Execute<Dictionary<string, SummonerDto>>(request);

            return new Dictionary<string, SummonerDto>(resp, StringComparer.OrdinalIgnoreCase);
        }


        public async Task<Dictionary<string, MasteryPagesDto>> GetMasteriesForSummoners(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.4/summoner/{summonerIds}/masteries" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlParam);

            return await Execute<Dictionary<string, MasteryPagesDto>>(request);
        }

        public async Task<Dictionary<string, RunePagesDto>> GetRunesForSummoners(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.4/summoner/{summonerIds}/runes" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlParam);

            return await Execute<Dictionary<string, RunePagesDto>>(request);
        }

        public async Task<Dictionary<string, string>> GetSummonerNames(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.4/summoner/{summonerIds}/names" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlParam);

            return await Execute<Dictionary<string, string>>(request);
        }

        public async Task<MatchList> GetMatchList(long summonerId, IEnumerable<long> championIds = null, IEnumerable<string> rankedQueues = null, IEnumerable<string> seasons = null, long? beginTime = null, long? endTime = null, int? beginIndex = null, int? endIndex = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.2/matchlist/by-summoner/{summonerId}"};
            request.AddParameter("summonerId", summonerId, ParameterType.UrlParam);

            if (championIds != null) request.AddParameter("championIds", string.Join(",", championIds.Select(s => s.ToString())), ParameterType.QueryParam);
            if (rankedQueues != null) request.AddParameter("rankedQueues", string.Join(",", rankedQueues), ParameterType.QueryParam);
            if (seasons != null) request.AddParameter("seasons", string.Join(",", seasons), ParameterType.QueryParam);
            if (beginTime != null) request.AddParameter("beginTime", beginTime, ParameterType.QueryParam);
            if (endTime != null) request.AddParameter("endTime", endTime, ParameterType.QueryParam);
            if (beginIndex != null) request.AddParameter("beginIndex", beginIndex, ParameterType.QueryParam);
            if (endIndex != null) request.AddParameter("endIndex", endIndex, ParameterType.QueryParam);

            return await Execute<MatchList>(request);
        }

        public async Task<MatchDetail> GetMatch(long matchId, bool? includeTimeline = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.2/match/{matchId}" };
            request.AddParameter("matchId", matchId, ParameterType.UrlParam);
            if (includeTimeline != null) request.AddParameter("includeTimeline", includeTimeline, ParameterType.QueryParam);

            return await Execute<MatchDetail>(request);
        }

        public async Task<RankedStatsDto> GetRankedStats(long summonerId, string season = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/ranked" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlParam);
            if (!string.IsNullOrEmpty(season)) request.AddParameter("season", season, ParameterType.QueryParam);

            return await Execute<RankedStatsDto>(request);
        }

        public async Task<PlayerStatsSummaryListDto> GetSummaryStats(long summonerId, string season = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/summary" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlParam);
            if (!string.IsNullOrEmpty(season)) request.AddParameter("season", season, ParameterType.QueryParam);

            return await Execute<PlayerStatsSummaryListDto>(request);
        }

        public async Task<RecentGamesDto> GetRecentGames(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.3/game/by-summoner/{summonerId}/recent" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlParam);

            return await Execute<RecentGamesDto>(request);
        }

        public async Task<ChampionMasteryDto> GetChampionMasteryForChampion(long summonerId, long championId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "championmastery/location/{platformId}/player/{playerId}/champion/{championId}" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlParam);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlParam);
            request.AddParameter("championId", championId, ParameterType.UrlParam);

            return await Execute<ChampionMasteryDto>(request);
        }

        public async Task<List<ChampionMasteryDto>> GetChampionMasteries(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "championmastery/location/{platformId}/player/{playerId}/champions" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlParam);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlParam);

            return await Execute<List<ChampionMasteryDto>>(request);
        }

        public async Task<int> GetChampionMasteryTotalScore(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "championmastery/location/{platformId}/player/{playerId}/score" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlParam);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlParam);

            return await Execute<int>(request);
        }

        public async Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId, int? count = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "championmastery/location/{platformId}/player/{playerId}/topchampions" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlParam);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlParam);
            if (count != null) request.AddParameter("count", count, ParameterType.QueryParam);

            return await Execute<List<ChampionMasteryDto>>(request);
        }

        public async Task<CurrentGameInfo> GetCurrentGame(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "observer-mode/rest/consumer/getSpectatorGameInfo/{platformId}/{summonerId}" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlParam);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlParam);

            return await Execute<CurrentGameInfo>(request);
        }

        public async Task<ChampionListDto> GetChampionsStaticData(string locale = null, string version = null, string dataById = null, string champData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/champion" };

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(dataById)) request.AddParameter("dataById", dataById, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(champData)) request.AddParameter("champData", champData, ParameterType.QueryParam);

            return await Execute<ChampionListDto>(request);
        }

        public async Task<ChampionDto> GetChampionStaticData(int championId, string locale = null, string version = null, string dataById = null, string champData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlParam);

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(dataById)) request.AddParameter("dataById", dataById, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(champData)) request.AddParameter("champData", champData, ParameterType.QueryParam);

            return await Execute<ChampionDto>(request);
        }

        public async Task<ItemListDto> GetItems(string locale = null, string version = null, string itemListData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/item" };

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(itemListData)) request.AddParameter("itemListData", itemListData, ParameterType.QueryParam);

            return await Execute<ItemListDto>(request);
        }

        public async Task<ItemDto> GetItem(int itemId, string locale = null, string version = null, string itemData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/item/{id}" };
            request.AddParameter("id", itemId, ParameterType.UrlParam);

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(itemData)) request.AddParameter("itemData", itemData, ParameterType.QueryParam);

            return await Execute<ItemDto>(request);
        }

        public async Task<LanguageStringsDto> GetLanguageStrings(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/language-strings" };

            return await Execute<LanguageStringsDto>(request);
        }

        public async Task<List<string>> GetLanguages(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/languages" };

            return await Execute<List<string>>(request);
        }

        public async Task<MapDataDto> GetMaps(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/map" };

            return await Execute<MapDataDto>(request);
        }

        public async Task<MasteryListDto> GetMasteries(string locale = null, string version = null, string masteryListData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/mastery" };

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(masteryListData)) request.AddParameter("itemData", masteryListData, ParameterType.QueryParam);

            return await Execute<MasteryListDto>(request);
        }

        public async Task<MasteryDto> GetMastery(int masterId, string locale = null, string version = null, string masteryData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/mastery/{id}" };
            request.AddParameter("id", masterId, ParameterType.UrlParam);

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(masteryData)) request.AddParameter("masteryData", masteryData, ParameterType.QueryParam);

            return await Execute<MasteryDto>(request);
        }

        public async Task<RealmDto> GetRealms(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/realm" };

            return await Execute<RealmDto>(request);
        }

        public async Task<RuneListDto> GetRunes(string locale = null, string version = null, string runeListData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/rune" };

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(runeListData)) request.AddParameter("runeListData", runeListData, ParameterType.QueryParam);

            return await Execute<RuneListDto>(request);
        }

        public async Task<RuneDto> GetRune(int runeId, string locale = null, string version = null, string runeData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/rune/{id}" };
            request.AddParameter("id", runeId, ParameterType.UrlParam);

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(runeData)) request.AddParameter("runeData", runeData, ParameterType.QueryParam);

            return await Execute<RuneDto>(request);
        }

        public async Task<SummonerSpellListDto> GetSummonerSpells(string locale = null, string version = null, string spellData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/summoner-spell" };

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(spellData)) request.AddParameter("runeData", spellData, ParameterType.QueryParam);

            return await Execute<SummonerSpellListDto>(request);
        }

        public async Task<SummonerSpellDto> GetSummonerSpell(int summonerSpellId, string locale = null, string version = null, string spellData = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/summoner-spell/{id}" };
            request.AddParameter("id", summonerSpellId, ParameterType.UrlParam);

            if (!string.IsNullOrEmpty(locale)) request.AddParameter("locale", locale, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(version)) request.AddParameter("version", version, ParameterType.QueryParam);
            if (!string.IsNullOrEmpty(spellData)) request.AddParameter("spellData", spellData, ParameterType.QueryParam);

            return await Execute<SummonerSpellDto>(request);
        }

        public async Task<List<string>> GetVersions(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/static-data/{region}/v1.2/versions" };


            return await Execute<List<string>>(request);
        }

        public async Task<RiotObjects.Champion.ChampionListDto> GetChampionsData(bool? freeToPlay = null, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.2/champion" };
            if (freeToPlay != null) request.AddParameter("freeToPlay", freeToPlay, ParameterType.QueryParam);

            return await Execute<RiotObjects.Champion.ChampionListDto>(request);
        }

        public async Task<RiotObjects.Champion.ChampionDto> GetChampionData(long championId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlParam);

            return await Execute<RiotObjects.Champion.ChampionDto>(request);
        }

        public async Task<List<LeagueDto>> GetLeaguesForSummoner(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var leagues = await GetLeaguesForSummoners(new List<long> { summonerId });
            List<LeagueDto> leagueDtos;

            return leagues.TryGetValue(summonerId.ToString(), out leagueDtos) ? leagueDtos : null;
        }

        public async Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForSummoners(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 10) throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/by-summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds), ParameterType.UrlParam);

            return await Execute<Dictionary<string, List<LeagueDto>>>(request);
        }

        public async Task<List<LeagueDto>> GetLeagueEntriesForSummoner(long summonerId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var leagues = await GetLeagueEntriesForSummoners(new List<long> { summonerId });
            List<LeagueDto> leagueDtos;

            return leagues.TryGetValue(summonerId.ToString(), out leagueDtos) ? leagueDtos : null;
        }

        public async Task<Dictionary<string, List<LeagueDto>>> GetLeagueEntriesForSummoners(IEnumerable<long> summonerIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 10) throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/by-summoner/{summonerIds}/entry" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds), ParameterType.UrlParam);

            return await Execute<Dictionary<string, List<LeagueDto>>>(request);
        }

        public async Task<List<LeagueDto>> GetLeaguesForTeam(long teamId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var leagues = await GetLeaguesForTeams(new List<long> { teamId });
            List<LeagueDto> leagueDtos;

            return leagues.TryGetValue(teamId.ToString(), out leagueDtos) ? leagueDtos : null;
        }

        public async Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForTeams(IEnumerable<long> teamIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            teamIds = teamIds.ToList();
            if (teamIds.Count() > 10) throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(teamIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/by-team/{teamIds}" };

            request.AddParameter("teamIds", string.Join(",", teamIds), ParameterType.UrlParam);

            return await Execute<Dictionary<string, List<LeagueDto>>>(request);
        }

        public async Task<List<LeagueDto>> GetLeagueEntriesForTeam(long teamId, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var leagues = await GetLeagueEntriesForTeams(new List<long> { teamId });
            List<LeagueDto> leagueDtos;

            return leagues.TryGetValue(teamId.ToString(), out leagueDtos) ? leagueDtos : null;
        }

        public async Task<Dictionary<string, List<LeagueDto>>> GetLeagueEntriesForTeams(IEnumerable<long> teamIds, RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            teamIds = teamIds.ToList();
            if (teamIds.Count() > 10) throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(teamIds));
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/by-team/{teamIds}/entry" };

            request.AddParameter("teamIds", string.Join(",", teamIds), ParameterType.UrlParam);

            return await Execute<Dictionary<string, List<LeagueDto>>>(request);
        }

        public async Task<LeagueDto> GetChallengerLeague(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/challenger" };

            return await Execute<LeagueDto>(request);
        }

        public async Task<LeagueDto> GetMasterLeague(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "api/lol/{region}/v2.5/league/master" };

            return await Execute<LeagueDto>(request);
        }

        public async Task<FeaturedGames> GetFeaturedGames(RiotRegion? region = null)
        {
            if (region != null) Region = region.Value;
            var request = new RestRequest { RequestUri = "observer-mode/rest/featured" };

            return await Execute<FeaturedGames>(request);
        }

        public async Task<List<Shard>> GetShards()
        {
            var request = new RestRequest { RequestUri = "/shards"};

            return await Execute<List<Shard>>(request);
        }

        public async Task<ShardStatus> GetShard(string shardRegion)
        {
            var request = new RestRequest { RequestUri = "shards/{shardRegion}" };
            request.AddParameter("shardRegion", shardRegion, ParameterType.UrlParam);

            return await Execute<ShardStatus>(request);
        }

        private async Task<T> Execute<T>(RestRequest request) where T : new()
        {
            var regionString = Region.ToString().ToLower();
            request.AddParameter("api_key", _apiKey, ParameterType.QueryParam);
            request.AddParameter("region", regionString, ParameterType.UrlParam);
            var response = await _restClient.Execute<T>(request);

            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
            {
                // TooManyRequests
                if (response.StatusCode == (HttpStatusCode)429)
                {
                    var retryAfterHeader = response.Headers.FirstOrDefault(h => h.Name == "Retry-After");
                    var retryAfterValue = retryAfterHeader?.Value as int?;
                    if (retryAfterValue != null)
                    {
                        throw new TooManyRequestsException(retryAfterValue.Value, response.ErrorException, _debugMode ? response : null);
                    }

                    throw new TooManyRequestsException(response.ErrorException, response);
                }
                var message = $"Error retrieving response. Riot returned an HTTP Status of {response.StatusCode}. Check inner details for more info.";
                throw new RiotApiException(response.StatusCode, message, response.ErrorException, _debugMode ? response : null);
            }
            return response.Data;
        }

        private void ConstructRestClient()
        {
            var regionString = Region.ToString().ToLower();
            _restClient.BaseUrl = $"https://{regionString}.api.pvp.net";
        }

        // runs an api call that doesn't count against the rate limit and checks for an HTTP Status of 401
        private async Task CheckApiKey()
        {
            try
            {
                await GetChampionStaticData(1);
            }
            catch (RiotApiException e) when (e.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new RiotClientException("Api key is not valid. Get a valid api key from https://developer.riotgames.com/", e);
            }
        }

        private RiotPlatform MapRegionToPlatform()
        {
            switch (Region)
            {
                case RiotRegion.Br:
                    return RiotPlatform.Br1;
                case RiotRegion.Eune:
                    return RiotPlatform.Eun1;
                case RiotRegion.Euw:
                    return RiotPlatform.Euw1;
                case RiotRegion.Jp:
                    return RiotPlatform.Jp1;
                case RiotRegion.Kr:
                    return RiotPlatform.Kr;
                case RiotRegion.Lan:
                    return RiotPlatform.La1;
                case RiotRegion.Las:
                    return RiotPlatform.La2;
                case RiotRegion.Na:
                    return RiotPlatform.Na1;
                case RiotRegion.Oce:
                    return RiotPlatform.Oc1;
                case RiotRegion.Ru:
                    return RiotPlatform.Ru;
                case RiotRegion.Tr:
                    return RiotPlatform.Tr1;
                default:
                    throw new ArgumentException("Region is not valid.", nameof(Region));
            }
        }
    }
}
