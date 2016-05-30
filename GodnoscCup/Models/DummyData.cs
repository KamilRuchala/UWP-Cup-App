using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodnoscCup.Models
{
    public static class DummyData
    {
        public static void Initialize(CustomContext context)
        {            
            if (!context.Teams.Any())
            {
                Team manutd = null;
                Team barca = null;

                if (!context.Teams.Any())
                {
                    barca = new Team("FC Barcelona");
                    manutd = new Team("Manchester United");
                    context.Teams.AddRange(barca, manutd);
                }
                else
                {
                    barca = context.Teams.Where(x => x.TeamName.Equals("FC Barcelona")).FirstOrDefault();
                    manutd = context.Teams.Where(x => x.TeamName.Equals("Manchester United")).FirstOrDefault();
                }

                context.Players.AddRange(
                   new Player("David De Gea", manutd, 1),
                   new Player("Bortwick-Jackson", manutd, 5),
                   new Player("Daley Blind", manutd, 4),
                   new Player("Chris Smalling", manutd, 3),
                   new Player("Matheo Darmian", manutd, 2),
                   new Player("Memphis Depay", manutd, 8),
                   new Player("Morgan Schneiderlin", manutd, 6),
                   new Player("Bastix Schweinsteiger", manutd,7),
                   new Player("Anthony Martial", manutd, 9),
                   new Player("Juan Mata", manutd, 10),
                   new Player("Wayne Rooney", manutd, 11),
                   new Player("Bamber Berbera", manutd, 12),
                   new Player("Jesse Lingard", manutd, 13),
                   new Player("Marouane Pellegrini", manutd, 14),

                   new Player("Claudio Bravo", barca, 1),
                   new Player("Maszczerano", barca, 3),
                   new Player("Gerard Pikieta", barca,2),
                   new Player("Jessica Alba", barca, 5),
                   new Player("Daniel Alwesz", barca, 2),
                   new Player("Rakietic", barca, 7),
                   new Player("Sergiusz Buskets", barca, 6),
                   new Player("Andrzej Iniesta", barca, 8),
                   new Player("Nejmar", barca, 9),
                   new Player("Luis Suarez", barca, 11),
                   new Player("Lionel Messi", barca, 10),
                   new Player("Arda Turan", barca, 12),
                   new Player("Sergiusz Roberto", barca, 13)
                );
                context.SaveChanges();
            }
        }
    }

}
