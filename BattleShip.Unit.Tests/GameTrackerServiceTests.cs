using System;
using BattleShip.Api.ExceptionHandling;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Xunit;

namespace BattleShip.Unit.Tests
{
    public class GameTrackerServiceTests
    {
        [Fact]
        public void Cannot_Setup_Board_When_Its_in_Playing_Status()
        {
            var gameTracker = new GameTrackerService();
            gameTracker.Status = GameStatus.Playing;
            Action act = () => gameTracker.CreateBoard();
            Assert.Throws<HttpStatusCodeException>(act);
        }

        [Theory]
        [InlineData(GameStatus.NotStarted, GameStatus.Setup, null)]
        [InlineData(GameStatus.Setup, GameStatus.Playing, typeof(HttpStatusCodeException))]
        [InlineData(GameStatus.NotStarted, GameStatus.Playing, typeof(HttpStatusCodeException))]
        void CanMove_GameStatue_From_To(GameStatus from, GameStatus to, Type type = null)
        {
            var tracker = new GameTrackerService();
            tracker.SetStatus(from);
            Action act = () => tracker.SetStatus(to);
            if (type != null)
            {
                var exception = Assert.Throws<HttpStatusCodeException>(act);
            }
            else
            {
                act();
                Assert.Equal(to, tracker.Status);
            }
        }
        
        [Fact]
        public void ValidateShipPositions_Should_Throw_Exception_When_Ships_Position_Cross()
        {
            var tracker = new GameTrackerService();
            tracker.CreateBoard();
            
            
            tracker.Ships.Add(
                Ship.Create( new ShipViewModel
            {
                StartingPosition = new BoardPosition {X = 3, Y = 3},
                Alignment = Alignment.Horizontal,
                Length = 3
            }));
            
            var newShip = Ship.Create(new ShipViewModel
            {
                StartingPosition = new BoardPosition {X = 4, Y = 1},
                Alignment = Alignment.Vertical,
                Length = 4
            });
            Action act = ()=> tracker.ValidateIfShipNotCrossingOtherShips(newShip);
            Assert.Throws<HttpStatusCodeException>(act);
        }

        [Fact]
        public void ValidateIfShipCanFitIn_Should_Throw_Exception_When_Ship_Position_Cross_Border()
        {
        
            //Arrange
            var shipViewModel = new ShipViewModel
            {
                StartingPosition = new BoardPosition {X = 3, Y = 3},
                Alignment = Alignment.Horizontal,
                Length = 10
            };
            var ship = Ship.Create(shipViewModel); 
            
            var tracker = new GameTrackerService();
            tracker.Status = GameStatus.Setup;
            
           Action act = () => tracker.ValidateIfShipCanFitIn(ship);
           Assert.Throws<HttpStatusCodeException>(act);
        }
    }
}