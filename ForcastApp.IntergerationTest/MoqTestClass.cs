using ForecastApp.MainWebApp;
using ForecastApp.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Net;
using System.Threading;
using ForecastApp.OpenWeatherMapModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ForcastApp.IntergerationTest
{
    public class MoqTestClass
    {
        [Fact]
        public async Task ForecastService_GetForecast_ValidReponseAsync()
        {

            //arrange 

            var main = new Main();
            main.Temp = 7.5f;
            main.Temp_Max = 9.5f;
            main.Temp_Min = 4.5f;
            var weather = new WeatherResponse(main);

            var handlerMock = new Mock<HttpMessageHandler>();


            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(weather), System.Text.Encoding.UTF8, "application/json")

            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            //act//

            var httpClient = new HttpClient(handlerMock.Object);
            IForecastRepository forecastRepository = new ForecastRepository(httpClient);
            var result = await forecastRepository.GetForecastASync("London");

            //Assert

            Assert.NotNull(result);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task WhenUnMacthInputProvided_ServiceShouldReturnNull()
        {
            var weather = new WeatherResponse();
            var handlerMock = new Mock<HttpMessageHandler>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(JsonConvert.SerializeObject(null),System.Text.Encoding.UTF8, "application/json")

            };

            handlerMock
                .Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            //act//

            var httpclient = new HttpClient(handlerMock.Object);
            var forcastrepo = new ForecastRepository(httpclient);
            var result = await forcastrepo.GetForecastASync("Leedssss"); //invalid input as been given

            //assert
             
            Assert.Null(result);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
                );

        }
    }
}
