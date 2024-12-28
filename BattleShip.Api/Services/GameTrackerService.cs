using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BattleShip.Api.ExceptionHandling;
using BattleShip.Api.Extensions;
using BattleShip.Api.Models;

namespace BattleShip.Api.Services
{
    public interface IGameTrackerService
    {
        void CreateBoard(int dimensions = 10);
        Ship AddShip(ShipViewModel shipViewModel);
        GameStatus Status { get; set; }
        void SetStatus(GameStatus newStatus);
        dynamic GetGameStatus();
        List<Ship> Ships { get; set; }
        Task<dynamic> AttackAsync(BoardPosition position);
    }

    public class GameTrackerService: IGameTrackerService
    {
        
        private int _dimensions = 10;
        
        private GameStatus _status;
        public GameStatus Status { 
            get => _status;
            set=> _status = value ; 
        }
        public List<Ship> Ships { get; set; }

        public GameTrackerService()
        {
            _status = GameStatus.NotStarted;
        }

        public void CreateBoard(int dimensions = 10)
        {
            _dimensions = dimensions;
            if (_status == GameStatus.Playing)
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden,$"You cannot setup the board while game status is {_status.ToString()}.");

            
            if(Ships== null)
                Ships = new List<Ship>();

            _status = GameStatus.Setup;
        }

        public Ship AddShip(ShipViewModel shipViewModel)
        {
            if (!CanAddShip())
                return null;
            
            var ship = Ship.Create(shipViewModel);
            
            if (!ValidateIfShipCanFitIn(ship))
                return null;
            
            if (!ValidateIfShipNotCrossingOtherShips(ship))
                return null;

            Ships.Add(ship);
            
            return ship;
        }

        public bool ValidateIfShipCanFitIn(Ship ship)
        {
            if (ship?.BoardPositions == null) return true;
            
            foreach (var position in ship.BoardPositions.Where(position => position.X>_dimensions||position.Y>_dimensions))
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden,$"Position X:{position.X}, Y:{position.Y} is crossing the board!");
            }
            return true;
        }

        public bool ValidateIfShipNotCrossingOtherShips(Ship ship)
        {
            if (ship?.BoardPositions == null) return true;
            
            var gameBoardPositions = Ships.SelectMany(i => i.BoardPositions).ToList();
            
            foreach (var position in ship.BoardPositions)
            {
                if (gameBoardPositions.Any(i=> i.X==position.X && i.Y ==position.Y))
                {
                    throw new HttpStatusCodeException(HttpStatusCode.Forbidden,$"Position X:{position.X}, Y:{position.Y} is occupied!");
                }
            }
            return true;
        }
        
        
        private bool CanAddShip()
        {
            if (_status==GameStatus.NotStarted)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,$"No setup board is found. Please create the board then add ships.");
            SetStatus(GameStatus.Setup);
            return true;
        }


        public void SetStatus(GameStatus newStatus)
        {
            switch (newStatus)
            {
                case GameStatus.Playing:
                    StartPlaying();
                    break;
                case GameStatus.Setup:
                    SetupGame();
                    break;
                case GameStatus.NotStarted:
                    ResetTheGame();
                    break;
            }
        }

        public void StartPlaying()
        {
            if(_status == GameStatus.NotStarted)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,"No setup board is found. Please create the board and add ships.");
            if (Ships.Count <= 0)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict,"No Ship is added yet. Please add ships.");

            _status = GameStatus.Playing;
        }
       
        void SetupGame()
        {
            switch (_status)
            {
                case GameStatus.NotStarted:
                    CreateBoard();
                    break;
                case GameStatus.Playing:
                    throw new HttpStatusCodeException(HttpStatusCode.Forbidden,$"Cannot move to status {GameStatus.Setup}, from current status {_status.ToString()}.");
                case GameStatus.Setup:
                    _status = GameStatus.Setup;
                    break;
                default:
                    throw new HttpStatusCodeException(HttpStatusCode.Forbidden,"invalid game status");
            }
        }

        public async Task<dynamic> AttackAsync(BoardPosition position)
        {
            
            SetStatus(GameStatus.Playing);
            
            var attackResponse = await Task.Run(() =>
            {
                dynamic result = new ExpandoObject();
                var allBoardPositions = Ships.SelectMany(s => s.BoardPositions).ToList();
                var hitPosition = allBoardPositions.FirstOrDefault(i => i.X == position.X && i.Y == position.Y && i.Value != Guid.Empty);
                if(hitPosition == null)
                {
                    result.Result = "miss";
                }
                else
                {
                    result.Result = "hit";

                    var suncked = ClearBoardPosition(allBoardPositions , hitPosition);

                    if (!string.IsNullOrEmpty(suncked))
                        result.ShipStatus = suncked;
                        
                    if (allBoardPositions.All(i => i.Value == Guid.Empty))
                    {
                        result.GameStatus = "I Lost Game!";
                        ResetTheGame();
                    }
                }
                return result;
            });
            return attackResponse;
        }

        private string ClearBoardPosition(List<BoardPosition> boardPositions, BoardPosition foundPosition)
        {
            Guid id;
            id = foundPosition.Value;
            foundPosition.Value = Guid.Empty;
            return boardPositions.Any(i => i.Value == id) ? null : "You Sunk my ship!";
        }

        
        private void ResetTheGame()
        {
            Ships = null;
            _status = GameStatus.NotStarted;
        }
        public dynamic GetGameStatus()
        {
            return this.ToViewModel();
        }
    }
}