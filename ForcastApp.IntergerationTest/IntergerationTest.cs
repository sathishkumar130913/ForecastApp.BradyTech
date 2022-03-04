using ForecastApp.MainWebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ForcastApp.IntergerationTest
{
    public class IntergerationTestClass :IClassFixture<WebApplicationFactory<Startup>>
    {
       private readonly WebApplicationFactory<Startup> _factory;
           
       public IntergerationTestClass(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;

        }
        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Home/Index")]
        [InlineData("/ForecastApp/SearchCity")]
        public async Task Validate_MiddlewareFlow(string url)
        {

            //Arrange

            var client = _factory.CreateClient();

            //Act

            var response = await client.GetAsync(url);

            //Assert

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());

        }
    }
}
