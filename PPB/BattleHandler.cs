using System;
using System.Collections.Generic;
using System.Text;

namespace PPB
{
    public static class BattleHandler
    {
        public static (User winner, User loser, List<string> log) Battle(User Player1, User Player2)
        {
            
            List<string> log = new List<string>();
            User Winner;
            string Player1Actions = string.Join(",", Player1.Actions);
            string Player2Actions = string.Join(",", Player2.Actions);


            //Battle A vs B
            log.Add("Battle: " + Player1.Username + " -> " + Player2.Username);
            log.Add(Player1.Username + " (" + Player1Actions + ") vs " + Player2.Username + " (" + Player2Actions + ")");

            //Winner of Battle
            Winner = ActionBattle(Player1, Player2, log);              
                
            

            if (Winner == Player1)
            {
                log.Add(Player1.Username + " won the battle!");
                return (Player1, Player2, log);
            }
            else if (Winner == Player2)
            {
                log.Add(Player2.Username + " won the battle!");
                return (Player2, Player1, log);
            }
            else
            {
                log.Add(Player1.Username + " and " + Player2.Username + " are blocked!");
                return (null, null, log);
            }
        }

        public static User ActionBattle(User Player1, User Player2, List<string> log)
        {
            int RoundPointsPlayer1 = 0;
            int RoundPointsPlayer2 = 0;
            int round = 0;
            var RoundWinners = "";
            var ActionsPlayer1 = Player1.Actions;
            var ActionsPlayer2 = Player2.Actions;

            do
            {
                if ((ActionsPlayer1[round] == Actions.R && ActionsPlayer2[round] == Actions.R) || (ActionsPlayer1[round] == Actions.P && ActionsPlayer2[round] == Actions.P) || (ActionsPlayer1[round] == Actions.S && ActionsPlayer2[round] == Actions.S) || (ActionsPlayer1[round] == Actions.L && ActionsPlayer2[round] == Actions.L) || (ActionsPlayer1[round] == Actions.V && ActionsPlayer2[round] == Actions.V))
                {
                    if (round == 4) RoundWinners += "X";
                    else RoundWinners += "X,";
                } 
                else if ((ActionsPlayer1[round] == Actions.R && ActionsPlayer2[round] == Actions.S) || (ActionsPlayer1[round] == Actions.R && ActionsPlayer2[round] == Actions.L) || (ActionsPlayer1[round] == Actions.P && ActionsPlayer2[round] == Actions.R) || (ActionsPlayer1[round] == Actions.P && ActionsPlayer2[round] == Actions.V) || (ActionsPlayer1[round] == Actions.S && ActionsPlayer2[round] == Actions.P) || (ActionsPlayer1[round] == Actions.S && ActionsPlayer2[round] == Actions.L) || (ActionsPlayer1[round] == Actions.L && ActionsPlayer2[round] == Actions.V) || (ActionsPlayer1[round] == Actions.L && ActionsPlayer2[round] == Actions.P) || (ActionsPlayer1[round] == Actions.V && ActionsPlayer2[round] == Actions.S) || (ActionsPlayer1[round] == Actions.V && ActionsPlayer2[round] == Actions.R))
                {
                    if (round == 4) RoundWinners += Player1.Username;
                    else RoundWinners += Player1.Username + ",";
                    RoundPointsPlayer1++;
                } 
                else
                {
                    if (round == 4) RoundWinners += Player2.Username;
                    else RoundWinners += Player2.Username + ",";
                    RoundPointsPlayer2++;
                }

                round++;
            } while (round < 5);

            if (RoundPointsPlayer1 > RoundPointsPlayer2)
            {
                log.Add("winners: " + RoundWinners + " => +" + RoundPointsPlayer1 + " for " + Player1.Username + " => +1 battle point for " + Player1.Username + " / 0 battle points for " + Player2.Username);
                Player1.BattlePoints++;
                return Player1;
            }
            else if (RoundPointsPlayer2 > RoundPointsPlayer1)
            {
                log.Add("winners: " + RoundWinners + " => +" + RoundPointsPlayer2 + " for " + Player2.Username + " => +1 battle point for " + Player2.Username + " / 0 battle points for " + Player1.Username);
                Player2.BattlePoints++;
                return Player2;
            }
            else
            {
                log.Add("winners: " + RoundWinners + " => no winner because 5 draws => both users are blocked for the rest of the tournament.");
                return null;
            }
        }
    }
}