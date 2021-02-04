using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace PPB
{
    public class Scoreboard
    {

        public List<User> GetUserswithMostWins()
        {

            var users = DBHandler.GetallUsers();
            var sortedUsers = users.OrderByDescending(user => user.Wins).ToList();
             
            return sortedUsers;
        }

        public List<User> GetUserswithMostLosses()
        {

            var users = DBHandler.GetallUsers();
            var sortedUsers = users.OrderByDescending(user => user.Losses).ToList();

            return sortedUsers;
        }

        public List<User> GetUserswithWinsLossesRatio()
        {

            var users = DBHandler.GetallUsers();
            var sortedUsers = users.OrderByDescending(user => user.Wins/user.Losses).ToList();

            return sortedUsers;
        }

        public List<User> GetUserswithHighestElo()
        {

            var users = DBHandler.GetallUsers();
            var sortedUsers = users.OrderByDescending(user => user.Elo).ToList();

            return sortedUsers;
        }
    }
}
