using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.Models
{
    public class Player
    {
        public int PlayerId { get; set; }

        public string PlayerName { get; set; } 

        public int TeamId { get; set; }

        public int SequenceNumber { get; set; }

        public int ScoredGoals { get; set; }

        public virtual Team Team { get; set; }

        public Player() { }

        public Player(string name, Team tim, int seq, int goals = 0)
        {
            this.PlayerName = name;
            this.Team = tim;
            this.ScoredGoals = goals;
            this.SequenceNumber = seq;
        }
    }
}
