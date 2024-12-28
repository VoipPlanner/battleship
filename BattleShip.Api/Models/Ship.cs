using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BattleShip.Api.Extensions;

namespace BattleShip.Api.Models
{
    public class Ship
    {
        public Guid Id { get; private set; }
        public int Length { get; private set; }
        public  Alignment Alignment { get; private set; }
        
        public List<BoardPosition> BoardPositions { get; private set; }
        
        private Ship(BoardPosition position, int length, Alignment alignment)
        {
            Id = Guid.NewGuid();
            BoardPositions = new List<BoardPosition>();
            Length = length;
            Alignment = alignment;
            var startPosition = new BoardPosition {X = position.X, Y = position.Y, Value = Id};
            
            //startPosition.BuildShipFrom(length, alignment);
            // BoardPositions = new List<BoardPosition>
            // {
            //     new BoardPosition{X= position.X,Y = position.Y,Value = Id}
            // };
            //StartingPosition = position;
            
            
            BoardPositions.AddRange(startPosition.BuildShipFrom(length, alignment));
        }
        public static Ship Create(ShipViewModel shipViewModel)
        {
            return shipViewModel.Length <= 0 ? null 
                : new Ship(shipViewModel.StartingPosition, shipViewModel.Length ,shipViewModel.Alignment);
        }
    }
    
    public class ShipViewModel
    {
        [Range(1, 10)]
        [Required]
        public int Length { get; set; }
        public  Alignment Alignment { get; set; }
        [Required]
        public  BoardPosition StartingPosition { get; set; }
    }
}