using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BattleShip.Api;
using BattleShip.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Battleship.Integration.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public const string Get_Board_Url = "/board";
        public const string Get_Game_Url = "/game";
        public const string ApplicationJson_ContentType = "application/json; charset=utf-8";
        public const string Create_Board_Url = "/board/create";
        public const string Add_Ship_URL = "/board/ship";
        public const string Set_Game_Status = "/game/status";

        private readonly ShipViewModel _stubShipViewModel;
        public ApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _stubShipViewModel = new ShipViewModel
            {
                Alignment = Alignment.Horizontal,
                Length = 3,
                StartingPosition = new BoardPosition
                {
                    X=3,
                    Y=3
                }
            };
        }
        
        [Theory]
        [InlineData(Get_Game_Url)]
        [InlineData(Get_Board_Url)]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(ApplicationJson_ContentType, response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Create_Board_Is_Successful()
        {
            
            var response = await CreateBoard();
            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(ApplicationJson_ContentType, response.Content.Headers.ContentType.ToString());
        }

        private async Task<HttpResponseMessage> CreateBoard()
        {
            var client = _factory.CreateClient();
            var stringContent = new StringContent("" ,Encoding.UTF8,"application/json");
            var response = await client.PostAsync(Create_Board_Url, stringContent);
            return response;
        }
        
        [Fact]
        public async Task Create_Board_And_Add_Ship_Is_Successful()
        {
            await CreateBoard();
            var response = await AddStubedShip();

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(ApplicationJson_ContentType, response.Content.Headers.ContentType.ToString());
        }
        [Fact]
        public async Task Set_GameStatus_To_Setup_Should_Create_The_Board()
        {
            var client = _factory.CreateClient();

            var response = await client.PutAsync(Set_Game_Status+"/setup", null);

            // Assert
            response.EnsureSuccessStatusCode(); 
            
            Assert.Equal(ApplicationJson_ContentType, response.Content.Headers.ContentType.ToString());
            
        }

        private async Task<HttpResponseMessage> AddStubedShip()
        {
            var client = _factory.CreateClient();
            var jsonBody = JsonConvert.SerializeObject(_stubShipViewModel);
            var stringContent = new StringContent(jsonBody ,Encoding.UTF8,"application/json");
            return  await client.PostAsync(Add_Ship_URL, stringContent);
        }
    }
}