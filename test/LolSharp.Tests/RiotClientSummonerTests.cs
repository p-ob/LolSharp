namespace LolSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Moq;
    using RestSharp;
    using RiotObjects.StaticData;
    using Xunit;
    using RiotObjects.Summoner;

    public class RiotClientSummonerTests
    {
        private const long SummonerId = 25886496;
        private readonly SummonerDto _mockSummoner = new SummonerDto
        {
            Id = SummonerId,
            Name = "Drunk7Irishman",
            SummonerLevel = 30
        };

        private readonly IRiotClient _riotClient;

        public RiotClientSummonerTests()
        {
            var restClientMock = new Mock<IRestClient>();
            // For CheckApiCall called in RiotClient constructor
            restClientMock.Setup(c => c.ExecuteTaskAsync<ChampionDto>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse<ChampionDto>
                {
                    Data = new ChampionDto(),
                    StatusCode = HttpStatusCode.OK
                });

            restClientMock.Setup(
                c =>
                    c.ExecuteTaskAsync<Dictionary<string, SummonerDto>>(
                        It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse<Dictionary<string, SummonerDto>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null
                });

            restClientMock.Setup(
                c =>
                    c.ExecuteTaskAsync<Dictionary<string, SummonerDto>>(
                        It.Is<IRestRequest>(
                            r =>
                                r.Parameters.Any(
                                    p =>
                                        p.Name == "summonerIds" && ((string)p.Value).Contains(SummonerId.ToString())))))
                .ReturnsAsync(new RestResponse<Dictionary<string, SummonerDto>>
                {
                    Data = new Dictionary<string, SummonerDto> { { SummonerId.ToString(), _mockSummoner } },
                    StatusCode = HttpStatusCode.OK
                });

            


            _riotClient = new RiotClient(restClientMock.Object, "fake-api-key", true);
        }

        [Fact]
        public async void TestGetExistingSummoner()
        {
            var gottenSummoner = await _riotClient.GetSummoner(SummonerId);
            Assert.True(gottenSummoner.Id == SummonerId);
        }

        [Fact]
        public async void TestGetNonexistentSummoner()
        {
            var exception = await Assert.ThrowsAsync<RiotApiException>(() => _riotClient.GetSummoner(SummonerId - 1));
            Assert.True(exception.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
