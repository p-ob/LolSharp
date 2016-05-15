namespace LolSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Moq;
    using Xunit;
    using RestSharp;
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
        private readonly Mock<IRiotClient> _riotClient;

        public RiotClientSummonerTests()
        {
            //_restClient.Setup(c => c.ExecuteAsync(
            //    It.IsAny<IRestRequest>(),
            //    It.IsAny<Action<IRestResponse<Dictionary<string, SummonerDto>>, RestRequestAsyncHandle>>()))
            //    .Callback<IRestRequest, Action<IRestResponse<Dictionary<string, SummonerDto>>, RestRequestAsyncHandle>>(
            //        (request, callback) =>
            //        {
            //            var responseMock = new Mock<IRestResponse<Dictionary<string, SummonerDto>>>();
            //            responseMock.Setup(r => r.Data).Returns(new Dictionary<string, SummonerDto> { { "drunk7irishman", _mockSummoner }});
            //            callback(responseMock.Object, null);
            //        });
            _riotClient = new Mock<IRiotClient>();

            _riotClient.Setup(c => c.GetSummoners(It.IsAny<IEnumerable<long>>(), null))
                .ReturnsAsync(new Dictionary<string, SummonerDto> {{"drunk7irishman", _mockSummoner}});
        }

        [Fact]
        public void TestGetExistingSummoner()
        {
            try
            {
            }
            catch (RiotApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
            }
        }

        [Fact]
        public void TestGetNonexistentSummoner()
        {
            try
            {
            }
            catch (RiotApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
            }
        }
    }
}
