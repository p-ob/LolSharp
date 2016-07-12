namespace LolSharp.Tests
{
    // TODO fix tests
    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Net;
    //using Moq;
    //using RestSharp;
    //using RiotObjects.StaticData;
    //using Xunit;
    using RiotObjects.Summoner;

    public class RiotClientSummonerTests
    {
        private const long MockSummonerId = 25886496;
        private const string MockSummonerName = "drunk7irishman";
        private readonly SummonerDto _mockSummoner = new SummonerDto
        {
            Id = MockSummonerId,
            Name = "Drunk7Irishman",
            SummonerLevel = 30
        };

        private readonly IRiotClient _riotClient;

        //public RiotClientSummonerTests()
        //{
        //    var restClientMock = new Mock<IRestClient>();

        //    // For CheckApiCall called in RiotClient constructor
        //    restClientMock.Setup(c => c.ExecuteTaskAsync<ChampionDto>(It.IsAny<IRestRequest>()))
        //        .ReturnsAsync(new RestResponse<ChampionDto>
        //        {
        //            Data = new ChampionDto(),
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    restClientMock.Setup(
        //        c =>
        //            c.ExecuteTaskAsync<Dictionary<string, SummonerDto>>(
        //                It.IsAny<IRestRequest>()))
        //        .ReturnsAsync(new RestResponse<Dictionary<string, SummonerDto>>
        //        {
        //            StatusCode = HttpStatusCode.NotFound,
        //            Data = null
        //        });

        //    restClientMock.Setup(
        //        c =>
        //            c.ExecuteTaskAsync<Dictionary<string, SummonerDto>>(
        //                It.Is<IRestRequest>(
        //                    r =>
        //                        r.Parameters.Any(
        //                            p =>
        //                                p.Name == "summonerIds" && ((string)p.Value).Contains(MockSummonerId.ToString())))))
        //        .ReturnsAsync(new RestResponse<Dictionary<string, SummonerDto>>
        //        {
        //            Data = new Dictionary<string, SummonerDto> { { MockSummonerId.ToString(), _mockSummoner } },
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    restClientMock.Setup(c =>
        //        c.ExecuteTaskAsync<Dictionary<string, SummonerDto>>(
        //            It.Is<IRestRequest>(
        //                r =>
        //                    r.Parameters.Any(
        //                        p =>
        //                            p.Name == "summonerNames" && ((string) p.Value).Contains(MockSummonerName)))))
        //        .ReturnsAsync(new RestResponse<Dictionary<string, SummonerDto>>
        //        {
        //            Data = new Dictionary<string, SummonerDto> {{MockSummonerName, _mockSummoner}},
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    restClientMock.Setup(c =>
        //           c.ExecuteTaskAsync<Dictionary<string, MasteryPagesDto>>(
        //               It.Is<IRestRequest>(
        //                   r =>
        //                       r.Parameters.Any(
        //                           p =>
        //                               p.Name == "summonerIds" && ((string)p.Value).Contains(MockSummonerId.ToString())))))

        //        .ReturnsAsync(new RestResponse<Dictionary<string, MasteryPagesDto>>
        //        {
        //            Data = new Dictionary<string, MasteryPagesDto> { { MockSummonerId.ToString(), new MasteryPagesDto() } },
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    restClientMock.Setup(c =>
        //        c.ExecuteTaskAsync<Dictionary<string, RunePagesDto>>(
        //            It.Is<IRestRequest>(
        //                r =>
        //                    r.Parameters.Any(
        //                        p =>
        //                            p.Name == "summonerIds" && ((string) p.Value).Contains(MockSummonerId.ToString())))))

        //        .ReturnsAsync(new RestResponse<Dictionary<string, RunePagesDto>>
        //        {
        //            Data = new Dictionary<string, RunePagesDto> { { MockSummonerId.ToString(), new RunePagesDto() } },
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    restClientMock.Setup(c =>
        //        c.ExecuteTaskAsync<Dictionary<string, string>>(
        //            It.Is<IRestRequest>(
        //                r =>
        //                    r.Parameters.Any(
        //                        p =>
        //                            p.Name == "summonerIds" && ((string) p.Value).Contains(MockSummonerId.ToString())))))
        //        .ReturnsAsync(new RestResponse<Dictionary<string, string>>
        //        {
        //            Data = new Dictionary<string, string> { {MockSummonerId.ToString(), MockSummonerName } },
        //            StatusCode = HttpStatusCode.OK
        //        });

        //    _riotClient = new RiotClient(restClientMock.Object, "fake-api-key", RiotRegion.Na, true);
        //}

        //[Fact]
        //public async void TestGetExistingSummoner()
        //{
        //    var gottenSummoner = await _riotClient.GetSummoner(MockSummonerId);
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetNonexistentSummoner()
        //{
        //    var exception = await Assert.ThrowsAsync<RiotApiException>(() => _riotClient.GetSummoner(MockSummonerId - 1));
        //    Assert.True(exception.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async void TestGetExistingSummoners()
        //{
        //    var summonerIds = new List<long> { MockSummonerId };
        //    var gottenSummoners = await _riotClient.GetSummoners(summonerIds);

        //    Assert.True(gottenSummoners.Count == summonerIds.Count);
        //    Assert.True(gottenSummoners.ContainsKey(MockSummonerId.ToString()));

        //    var gottenSummoner = gottenSummoners[MockSummonerId.ToString()];
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetNonexistentSummoners()
        //{
        //    var summonerIds = new List<long> { MockSummonerId - 1 };

        //    var exception = await Assert.ThrowsAsync<RiotApiException>(() => _riotClient.GetSummoners(summonerIds));
        //    Assert.True(exception.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async void TestGetSomeExistingSummoners()
        //{
        //    var summonerIds = new List<long> { MockSummonerId, MockSummonerId - 1 };
        //    var gottenSummoners = await _riotClient.GetSummoners(summonerIds);

        //    Assert.True(gottenSummoners.Count == summonerIds.Count - 1);
        //    Assert.True(gottenSummoners.ContainsKey(MockSummonerId.ToString()));
        //    Assert.False(gottenSummoners.ContainsKey((MockSummonerId - 1).ToString()));

        //    var gottenSummoner = gottenSummoners[MockSummonerId.ToString()];
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetExistingSummonerByName()
        //{
        //    var gottenSummoner = await _riotClient.GetSummonerByName(MockSummonerName);
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetNonexistentSummonerByName()
        //{
        //    var exception = await Assert.ThrowsAsync<RiotApiException>(() => _riotClient.GetSummonerByName(Guid.NewGuid().ToString()));
        //    Assert.True(exception.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async void TestGetExistingSummonersByName()
        //{
        //    var summonerNames = new List<string> { MockSummonerName };
        //    var gottenSummoners = await _riotClient.GetSummonersByName(summonerNames);

        //    Assert.True(gottenSummoners.Count == summonerNames.Count);
        //    Assert.True(gottenSummoners.ContainsKey(MockSummonerName));
        //    // ensure the dictionary returned is case-insensitive
        //    Assert.True(gottenSummoners.ContainsKey(MockSummonerName.ToUpper()));

        //    var gottenSummoner = gottenSummoners[MockSummonerName];
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetNonexistentSummonersByName()
        //{
        //    var summonerNames = new List<string> { Guid.NewGuid().ToString() };

        //    var exception = await Assert.ThrowsAsync<RiotApiException>(() => _riotClient.GetSummonersByName(summonerNames));
        //    Assert.True(exception.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async void TestGetSomeExistingSummonersByName()
        //{
        //    var randomName = Guid.NewGuid().ToString();
        //    var summonerNames = new List<string> { MockSummonerName, randomName };
        //    var gottenSummoners = await _riotClient.GetSummonersByName(summonerNames);

        //    Assert.True(gottenSummoners.Count == summonerNames.Count - 1);
        //    Assert.True(gottenSummoners.ContainsKey(MockSummonerName));
        //    Assert.False(gottenSummoners.ContainsKey(randomName));

        //    var gottenSummoner = gottenSummoners[MockSummonerName];
        //    Assert.True(gottenSummoner.Id == MockSummonerId);
        //}

        //[Fact]
        //public async void TestGetMasteriesForSummoners()
        //{
        //    var summonerIds = new List<long> { MockSummonerId };
        //    var masteries = await _riotClient.GetMasteriesForSummoners(summonerIds);

        //    Assert.NotNull(masteries);
        //    Assert.True(masteries.ContainsKey(MockSummonerId.ToString()));
        //    Assert.IsType<MasteryPagesDto>(masteries[MockSummonerId.ToString()]);
        //}

        //[Fact]
        //public async void TestGetRunesForSummoners()
        //{
        //    var summonerIds = new List<long> { MockSummonerId };
        //    var runes = await _riotClient.GetRunesForSummoners(summonerIds);

        //    Assert.NotNull(runes);
        //    Assert.True(runes.ContainsKey(MockSummonerId.ToString()));
        //    Assert.IsType<RunePagesDto>(runes[MockSummonerId.ToString()]);
        //}

        //[Fact]
        //public async void TestGetSummonerNames()
        //{
        //    var summonerIds = new List<long> { MockSummonerId };
        //    var names = await _riotClient.GetSummonerNames(summonerIds);

        //    Assert.NotNull(names);
        //    Assert.True(names.ContainsKey(MockSummonerId.ToString()));
        //    Assert.True(names[MockSummonerId.ToString()] == MockSummonerName);
        //}
    }
}
