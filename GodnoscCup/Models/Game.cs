using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public int TeamOneId { get; set; }

        public int TeamTwoId { get; set; }

        public int TeamOneScore { get; set; }

        public int TeamTwoScore { get; set; }

        public int TeamOnePoints { get; set; }

        public int TeamTwoPoints { get; set; }

        public DateTime GameDate { get; set; }

        public virtual ICollection<Scorer> Scorers { get; set; }

        public Game() { }
    }
}
