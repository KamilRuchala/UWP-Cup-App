using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.Models
{
    public class Scorer
    {
        public int ScorerId { get; set; }

        public int PlayerId { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }
}
