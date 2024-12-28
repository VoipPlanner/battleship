using System;
using System.Collections.Generic;
using BattleShip.Api.Models;
using Xunit;

namespace BattleShip.Unit.Tests
{
    public class ShipTests
    {
        [Fact]
        public void Create_A_Ship_Should_Assign_Positions()
        {
            
            var shipVewModel = new ShipViewModel
            {
                StartingPosition = new BoardPosition { X=1 , Y=1 },
                Length = 5,
                Alignment = Alignment.Horizontal
            };
            
            var ship = Ship.Create(shipVewModel);
            
            Assert.NotNull(ship);
            var boardPositions = ship.BoardPositions;
            
            Assert.Equal(shipVewModel.Length,ship.BoardPositions.Count);
            
            for (var i = 0; i < shipVewModel.Length; i++)
            {
                Assert.Contains(boardPositions, (n) =>
                    (shipVewModel.StartingPosition.Y == n.Y && shipVewModel.StartingPosition.X+i == n.X) 
                );
            }
        }

        
    }
}