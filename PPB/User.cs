using System;
using System.Collections.Generic;
using System.Text;

namespace PPB
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public int TournamentPoints { get; set; } = 100;
        public string Rank => TournamentPoints switch
        {
            < 101 => "Noob",
            > 100 and <= 105 => "Bronze",
            > 105 and <= 110 => "Silver",
            > 110 and <= 115 => "Gold",
            > 115 and <= 120 => "Diamond",
            > 120 and <= 125 => "Platinum",
            > 125 => "Challenger"
        };
        public List<MultiMediaContent> Library { get; set; } = new List<MultiMediaContent>();
        public List<Actions> Actions { get; set; } = new List<Actions>();
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public bool IsBlocked { get; set; } = false;
        public int BattlePoints { get; set; } = 0;
        public int GamesPlayed { get; set; } = 0;
    }


}
