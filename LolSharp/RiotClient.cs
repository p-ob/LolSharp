namespace LolSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using RiotDtos.League;
    using RestSharp;
    using RiotDtos.ChampionMastery;
    using RiotDtos.CurrentGame;
    using RiotDtos.Game;
    using RiotDtos.Match;
    using RiotDtos.MatchList;
    using RiotDtos.StaticData;
    using RiotDtos.Stats;
    using RiotDtos.Summoner;

    public class RiotClient : IRiotClient
    {
        private readonly string _apiKey;

        public RiotRegion Region { get; set; }

        public RiotClient(string apiKey) : this(apiKey, RiotRegion.Na)
        {
        }

        public RiotClient(string apiKey, RiotRegion region)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("apiKey is required", nameof(apiKey));
            if (!Enum.IsDefined(typeof(RiotRegion), region))
            {
                throw new ArgumentException("Region is not valid.", nameof(region));
            }

            _apiKey = apiKey;
            Region = region;
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
            var caseInsensitveSummoner = new Dictionary<string, SummonerDto>(summoner, StringComparer.OrdinalIgnoreCase);

            SummonerDto summonerDto;
            return caseInsensitveSummoner.TryGetValue(summonerName, out summonerDto) ? summonerDto : null;
        }

        public async Task<Dictionary<string, SummonerDto>> GetSummoners(IEnumerable<long> summonerIds)
        {
            summonerIds = summonerIds.ToList();
            if (summonerIds.Count() > 40)
            {
                throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerIds));
            }

            var request = new RestRequest { Resource = "api/lol/{region}/v1.4/summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds.Select(sId => sId.ToString())), ParameterType.UrlSegment);

            return await Execute<Dictionary<string, SummonerDto>>(request);

        }

        public async Task<Dictionary<string, SummonerDto>> GetSummonersByName(IEnumerable<string> summonerNames)
        {
            summonerNames = summonerNames.ToList();
            if (summonerNames.Count() > 40)
            {
                throw new ArgumentException("Can only pass in up to 40 summoners to retrieve", nameof(summonerNames));
            }
            var request = new RestRequest { Resource = "api/lol/{region}/v1.4/summoner/by-name/{summonerNames}" };

            request.AddParameter("summonerNames", string.Join(",", summonerNames), ParameterType.UrlSegment);

            return await Execute<Dictionary<string, SummonerDto>>(request);
        }

        public async Task<MatchList> GetMatchList(long summonerId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v2.2/matchlist/by-summoner/{summonerId}" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);

            return await Execute<MatchList>(request);
        }

        public async Task<MatchDetail> GetMatch(long matchId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v2.2/match/{matchId}" };
            request.AddParameter("matchId", matchId, ParameterType.UrlSegment);

            return await Execute<MatchDetail>(request);
        }

        public async Task<RankedStatsDto> GetRankedStats(long summonerId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/ranked" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);

            return await Execute<RankedStatsDto>(request);
        }

        public async Task<PlayerStatsSummaryListDto> GetSummaryStats(long summonerId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.3/stats/by-summoner/{summonerId}/summary" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);

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

        public async Task<List<ChampionMasteryDto>> GetChampionMasteryTopChampions(long summonerId)
        {
            var request = new RestRequest { Resource = "championmastery/location/{platformId}/player/{playerId}/topchampions" };
            request.AddParameter("playerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);

            return await Execute<List<ChampionMasteryDto>>(request);
        }

        public Task<CurrentGameInfo> GetCurrentGame(long summonerId)
        {
            var request = new RestRequest { Resource = "observer-mode/rest/consumer/getSpectatorGameInfo/{platformId}/{summonerId}" };
            request.AddParameter("summonerId", summonerId, ParameterType.UrlSegment);
            request.AddParameter("platformId", MapRegionToPlatform().ToString().ToUpper(), ParameterType.UrlSegment);

            return Execute<CurrentGameInfo>(request);
        }

        public async Task<ChampionListDto> GetChampionsStaticData()
        {
            var request = new RestRequest { Resource = "api/lol/static-data/{region}/v1.2/champion" };

            return await Execute<ChampionListDto>(request);
        }

        public Task<ChampionDto> GetChampionStaticData(int championId)
        {
            var request = new RestRequest { Resource = "api/lol/static-data/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlSegment);

            return Execute<ChampionDto>(request);
        }

        public async Task<RiotDtos.Champion.ChampionListDto> GetChampionsData()
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.2/champion" };

            return await Execute<RiotDtos.Champion.ChampionListDto>(request);
        }

        public async Task<RiotDtos.Champion.ChampionDto> GetChampionData(long championId)
        {
            var request = new RestRequest { Resource = "api/lol/{region}/v1.2/champion/{id}" };
            request.AddParameter("id", championId, ParameterType.UrlSegment);

            return await Execute<RiotDtos.Champion.ChampionDto>(request);
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
            if (summonerIds.Count() > 10)
            {
                throw new ArgumentException("Can only pass in up to 10 summoners to retrieve", nameof(summonerIds));
            }
            var request = new RestRequest { Resource = "api/lol/{region}/v2.5/league/by-summoner/{summonerIds}" };

            request.AddParameter("summonerIds", string.Join(",", summonerIds), ParameterType.UrlSegment);

            var leagues = await Execute<Dictionary<string, List<LeagueDto>>>(request);

            return leagues;
        }

        private async Task<T> Execute<T>(IRestRequest request) where T : new()
        {
            var regionString = Region.ToString().ToLower();
            var uri = new Uri($"https://{regionString}.api.pvp.net");
            var client = new RestClient(uri);
            request.AddParameter("api_key", _apiKey, ParameterType.QueryString);
            request.AddParameter("region", regionString, ParameterType.UrlSegment);
            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null)
            {
                // TooManyRequests
                if (response.StatusCode == (HttpStatusCode)429)
                {
                    var retryAfterHeader = response.Headers.FirstOrDefault(h => h.Name == "Retry-After");
                    if (retryAfterHeader != null)
                    {
                        var retryAfterValue = retryAfterHeader.Value as int?;
                        throw new TooManyRequestsException(retryAfterValue ?? 0, "Riot responded with an HTTP status code of 429. Check RetryAfter to see minimum wait time.", response.ErrorException);
                    }
                }
                const string message = "Error retrieving response. Check inner details for more info.";
                var riotException = new Exception(message, response.ErrorException);
                throw riotException;
            }
            return response.Data;
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
    public class TooManyRequestsException : Exception
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException() : base()
        {

        }

        public TooManyRequestsException(int retryAfter)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message) : base(message)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message, Exception innerException)
            : base(message, innerException)
        {
            RetryAfter = retryAfter;
        }
    }
}
