namespace LolSharp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using RestSharp;
    using RiotObjects.ChampionMastery;
    using RiotObjects.CurrentGame;
    using RiotObjects.Game;
    using RiotObjects.League;
    using RiotObjects.Match;
    using RiotObjects.MatchList;
    using RiotObjects.StaticData;
    using RiotObjects.Stats;
    using RiotObjects.Summoner;

    public class RiotClient : IRiotClient
    {
        private readonly string _apiKey;

        private RiotRegion _region;

        public RiotRegion Region
        {
            get { return _region; }
            set
            {
                if (!Enum.IsDefined(typeof(RiotRegion), value)) throw new RiotClientException("Region is not valid");
                _region = value;
            }
        }

        public RiotClient(string apiKey) : this(apiKey, RiotRegion.Na)
        {
        }

        public RiotClient(string apiKey, RiotRegion region)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("apiKey is required", nameof(apiKey));

            _apiKey = apiKey;
            Region = region;

            CheckApiKey();
        }

        public async Task<SummonerDto> GetSummoner(long summonerId)
        {
            var summoner = await GetSummoners(new List<long> { summonerId });
            SummonerDto summonerDto;
            return summoner.TryGetValue(summonerId.ToString(), out summonerDto) ? summonerDto : null;
        }

        public async Task<SummonerDto> GetSummonerByName(string summonerName)
        {
            var summoner = await GetSummonersByName(new List<string> { summonerName });

            SummonerDto summonerDto;
            return summoner.TryGetValue(summonerName, out summonerDto) ? summonerDto : null;
        }

        public async Task<Dictionary<string, SummonerDto>> GetSummoners(IEnumerable<long> summonerIds)
        {
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));

            var request = new RestRequest { Resource = "api/lol/{region}/v1.4/summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlSegment);

            return await Execute<Dictionary<string, SummonerDto>>(request);
        }

        public async Task<Dictionary<string, SummonerDto>> GetSummonersByName(IEnumerable<string> summonerNames)
        {
            summonerNames = summonerNames.ToList();
            if (summonerNames.Count() > 40) throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerNames));
            var request = new RestRequest { Resource = "api/lol/{region}/v1.4/summoner/by-name/{summonerNames}" };

            request.AddParameter("summonerNames", string.Join(",", summonerNames), ParameterType.UrlSegment);

            var resp = await Execute<Dictionary<string, SummonerDto>>(request);

            return new Dictionary<string, SummonerDto>(resp, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<MatchList> GetMatchList(long summonerId, IEnumerable<long> summonerIds = null, IEnumerable<string> rankedQueues = null, IEnumerable<string> seasons = null, long? beginTime = null, long? endTime = null, int? beginIndex = null, int? endIndex = null)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v2.2/matchlist/by-summoner/{summonerId}" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);

            if (summonerIds != null) request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(s => s.ToString())), ParameterType.QueryString);
            if (rankedQueues != null) request.AddParameter("rankedQueues", string.Join(",", rankedQueues), ParameterType.QueryString);
            if (seasons != null) request.AddParameter("seasons", string.Join(",", seasons), ParameterType.QueryString);
            if (beginTime != null) request.AddParameter("beginTime", beginTime, ParameterType.QueryString);
            if (endTime != null) request.AddParameter("endTime", endTime, ParameterType.QueryString);
            if (beginIndex != null) request.AddParameter("beginIndex", beginIndex, ParameterType.QueryString);
            if (endIndex != null) request.AddParameter("endIndex", endIndex, ParameterType.QueryString);

            return await Execute<MatchList>(request);
        }

        public async Task<MatchDetail> GetMatch(long matchId, bool includeTimeline = false)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v2.2/match/{matchId}" };
            request.AddParameter("matchId", matchId, ParameterType.UrlSegment);
            request.AddParameter("includeTimeline", includeTimeline, ParameterType.QueryString);

            return await Execute<MatchDetail>(request);
        }

        public async Task<RankedStatsDto> GetRankedStats(long summonerId, string season = null)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/ranked" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);
            if (!string.IsNullOrWhiteSpace(season)) request.AddParameter("season", season, ParameterType.QueryString);

            return await Execute<RankedStatsDto>(request);
        }

        public async Task<PlayerStatsSummaryListDto> GetSummaryStats(long summonerId, string season = null)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/summary" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);
            if (!string.IsNullOrWhiteSpace(season)) request.AddParameter("season", season, ParameterType.QueryString);

            return await Execute<PlayerStatsSummaryListDto>(request);
        }

        public async Task<RecentGamesDto> GetRecentGames(long summonerId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.3/game/by-summoner/{summonerId}/recent" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);

            return await Execute<RecentGamesDto>(request);
        }

        public async Task<ChampionMasteryDto> GetChampionMasteryForChampion(long summonerId, long championId)
        {
            var request = new RestRequest { Resource = "championmastery/location/{platformId}/player/{playerId}/champion/{championId}" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);
            request.AddParameter("championId", championId, ParameterType.UrlSegment);

            return await Execute<ChampionMasteryDto>(request);
        }

        public async Task<List<ChampionMasteryDto>> GetChampionMasteries(long summonerId)
        {
            var request = new RestRequest { Resource = "championmastery/location/{platformId}/player/{playerId}/champions" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);

            return await Execute<List<ChampionMasteryDto>>(request);
        }

        public async Task<int> GetChampionMasteryTotalScore(long summonerId)
        {
            var request = new RestRequest { Resource = "championmastery/location/{platformId}/player/{playerId}/score" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);

            return await Execute<int>(request);
        }

        public async Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId, int count = 3)
        {
            var request = new RestRequest { Resource = "championmastery/location/{platformId}/player/{playerId}/topchampions" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);
            request.AddParameter("count", count, ParameterType.QueryString);

            return await Execute<List<ChampionMasteryDto>>(request);
        }

        public Task<CurrentGameInfo> GetCurrentGame(long summonerId)
        {
            var request = new RestRequest { Resource = "observer-mode/rest/consumer/getSpectatorGameInfo/{platformId}/{summonerId}" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);

            return Execute<CurrentGameInfo>(request);
        }

        public async Task<ChampionListDto> GetChampionsStaticData(string locale = null, string version = null, string dataById = null, string champData = null)
        {
            var request = new RestRequest { Resource = "api/lol/static-data/{region}/v1.2/champion" };

            if (!string.IsNullOrWhiteSpace(locale)) request.AddParameter("locale", locale, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(version)) request.AddParameter("version", version, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(dataById)) request.AddParameter("dataById", dataById, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(champData)) request.AddParameter("champData", champData, ParameterType.QueryString);

            return await Execute<ChampionListDto>(request);
        }

        public Task<ChampionDto> GetChampionStaticData(int championId, string locale = null, string version = null, string dataById = null, string champData = null)
        {
            var request = new RestRequest { Resource = "api/lol/static-data/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlSegment);

            if (!string.IsNullOrWhiteSpace(locale)) request.AddParameter("locale", locale, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(version)) request.AddParameter("version", version, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(dataById)) request.AddParameter("dataById", dataById, ParameterType.QueryString);
            if (!string.IsNullOrWhiteSpace(champData)) request.AddParameter("champData", champData, ParameterType.QueryString);

            return Execute<ChampionDto>(request);
        }

        public async Task<RiotObjects.Champion.ChampionListDto> GetChampionsData(bool freeToPlay = false)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.2/champion" };
            request.AddParameter("freeToPlay", freeToPlay, ParameterType.QueryString);

            return await Execute<RiotObjects.Champion.ChampionListDto>(request);
        }

        public async Task<RiotObjects.Champion.ChampionDto> GetChampionData(long championId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlSegment);

            return await Execute<RiotObjects.Champion.ChampionDto>(request);
        }

        public async Task<List<LeagueDto>> GetLeaguesForSummoner(long summonerId)
        {
            var leagues = await GetLeaguesForSummoners(new List<long> { summonerId });
            List<LeagueDto> leagueDtos;

            return leagues.TryGetValue(summonerId.ToString(), out leagueDtos) ? leagueDtos : null;
        }

        public async Task<Dictionary<string, List<LeagueDto>>> GetLeaguesForSummoners(IEnumerable<long> summonerIds)
        {
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 10) throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(summonerIds));
            var request = new RestRequest { Resource = "api/lol/{region}/v2.5/league/by-summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds), ParameterType.UrlSegment);

            return await Execute<Dictionary<string, List<LeagueDto>>>(request);

        }

        private async Task<T> Execute<T>(IRestRequest request) where T : new()
        {
            var regionString = Region.ToString().ToLower();
            var uri = new Uri($"https://{regionString}.api.pvp.net");
            var client = new RestClient(uri);
            request.AddParameter("api_key", _apiKey, ParameterType.QueryString);
            request.AddParameter("region", regionString, ParameterType.UrlSegment);
            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
            {
                // TooManyRequests
                if (response.StatusCode == (HttpStatusCode)429)
                {
                    var retryAfterHeader = response.Headers.FirstOrDefault(h => h.Name == "Retry-After");
                    var retryAfterValue = retryAfterHeader?.Value as int?;
                    if (retryAfterValue != null)
                    {
                        throw new TooManyRequestsException(retryAfterValue.Value, "Riot responded with an HTTP status code of 429. Check RetryAfter to see minimum wait time.", response.ErrorException);
                    }

                    throw new TooManyRequestsException();
                }
                var message = $"Error retrieving response. Riot returned an HTTP Status of {response.StatusCode}. Check inner details for more info.";
                throw new RiotApiException(response.StatusCode, message, response.ErrorException);
            }
            return response.Data;
        }

        // runs an api call that doesn't count against the rate limit and checks for an HTTP Status of 401
        private void CheckApiKey()
        {
            try
            {
                GetChampionStaticData(1);
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

    public enum RiotRegion
    {
        Br,
        Eune,
        Euw,
        Jp,
        Kr,
        Lan,
        Las,
        Na,
        Oce,
        Ru,
        Tr
    }

    public enum RiotPlatform
    {
        Br1,
        Eun1,
        Euw1,
        Jp1,
        Kr,
        La1,
        La2,
        Na1,
        Oc1,
        Ru,
        Tr1
    }

    [Serializable]
    public class TooManyRequestsException : RiotApiException
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException() : this(-1, "No retry after value given.")
        {
        }

        public TooManyRequestsException(int retryAfter) : base((HttpStatusCode)429, string.Empty)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message) : base((HttpStatusCode)429, message)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message, Exception innerException)
            : base((HttpStatusCode)429, message, innerException)
        {
            RetryAfter = retryAfter;
        }
    }

    [Serializable]
    public class RiotApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public RiotApiException(HttpStatusCode statusCode) : this(statusCode, string.Empty, null)
        {
        }

        public RiotApiException(HttpStatusCode statusCode, string message) : this(statusCode, message, null)
        {
        }

        public RiotApiException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

    [Serializable]
    public class RiotClientException : Exception
    {
        public RiotClientException(string message) : this(message, null)
        {
            
        }

        public RiotClientException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
