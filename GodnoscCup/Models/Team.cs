using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public Team()
        { }

        public Team(string teamName)
        {
            this.TeamName = teamName;
        }
    }
}
