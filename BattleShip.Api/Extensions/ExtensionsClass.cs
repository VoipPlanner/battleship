using System.Collections.Generic;
using System.Dynamic;
using BattleShip.Api.ExceptionHandling;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.Builder;

namespace BattleShip.Api.Extensions
{
    public static class ExtensionsClass
    {
        public static dynamic ToViewModel(this IGameTrackerService gameTrackerService)
        {
            if (gameTrackerService == null) return null;
            dynamic result = new ExpandoObject();
            result.status = gameTrackerService.Status.ToString();
            if(gameTrackerService.Ships!=null)
                result.ships = gameTrackerService.Ships;
            return result;
        }
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static List<BoardPosition> BuildShipFrom(this BoardPosition startingPosition,int length, Alignment alignment)
        {
            if (startingPosition == null) return null;
            var boardPositions = new List<BoardPosition>{startingPosition};
            for (var i = 1; i < length; i++)
            {
                if (alignment == Alignment.Horizontal)
                {
                    boardPositions.Add(new BoardPosition
                    {
                        X= startingPosition.X+i,
                        Y= startingPosition.Y,
                        Value = startingPosition.Value
                    });
                }
                else 
                {
                    boardPositions.Add(new BoardPosition
                    {
                        X= startingPosition.X,
                        Y= startingPosition.Y+i,
                        Value = startingPosition.Value
                    });
                }
            }
            return boardPositions;
        }
        
    }
}