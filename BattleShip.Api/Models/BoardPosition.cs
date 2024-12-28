using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BattleShip.Api.Models
{
    public class BoardPosition
    {
        [JsonIgnore]
        public Guid Value { get; set; }
        [Required]
        [Range(1, 10)]
        public int X { get; set; }
        
        [Required]
        [Range(1, 10)]
        public int Y { get; set; }

        public BoardPosition()
        {
            Value = Guid.Empty;
        }

        public override string ToString()
        {
            return $"Board position X:{X}, Y:{Y}, Id:{Value.ToString()}";
        }
    }
}