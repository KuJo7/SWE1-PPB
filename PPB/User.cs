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
        public string Name { get; set; } = "";
        public string Bio { get; set; } = "";
        public string Image { get; set; } = "";
        public int Elo { get; set; } = 100;
        public string Rank => Elo switch
        {
            < 101 => "Noob",
            > 100 and <= 120 => "Bronze",
            > 120 and <= 140 => "Silver",
            > 140 and <= 160 => "Gold",
            > 160 and <= 180 => "Diamond",
            > 180 and <= 200 => "Platinum",
            > 200 => "Challenger"
        };
        public List<MultiMediaContent> Library { get; set; }
        public List<Actions> Actions { get; set; }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public bool IsAdmin { get; set; } = false;
        public int BattlePoints { get; set; } = 0;
    }


}
