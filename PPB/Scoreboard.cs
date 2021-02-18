using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace PPB
{
    public static class Scoreboard
    {

        public static List<User> GetUserswithMostWins()
        {

            var users = DBHandler.GetAllUsers();
            var sortedUsers = users.OrderByDescending(user => user.Wins).ToList();
             
            return sortedUsers;
        }

        public static List<User> GetUserswithMostLosses()
        {

            var users = DBHandler.GetAllUsers();
            var sortedUsers = users.OrderByDescending(user => user.Losses).ToList();

            return sortedUsers;
        }

        public static List<User> GetUserswithWinsLossesRatio()
        {

            var users = DBHandler.GetAllUsers();
            var sortedUsers = users.OrderByDescending(user => user.Wins/user.Losses).ToList();

            return sortedUsers;
        }

        public static List<User> GetUserswithHighestTournamentPoints()
        {

            var users = DBHandler.GetAllUsers();
            var sortedUsers = users.OrderByDescending(user => user.TournamentPoints).ToList();

            return sortedUsers;
        }

    }
}
