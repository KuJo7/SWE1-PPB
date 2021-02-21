using NUnit.Framework;
using System.Net;
using System.Collections.Generic;

namespace PPB.Tests
{
    public class Tests
    {
      

        [Test]
        public void TestBattleWinLoss()
        {
            // Arrange
            User player1 = new User() { Actions = new List<Actions>() { Actions.R,Actions.R, Actions.R, Actions.R, Actions.R } };
            User player2 = new User() { Actions = new List<Actions>() { Actions.S, Actions.S, Actions.S, Actions.S, Actions.S } };

            // Act
            var result = BattleHandler.Battle(player1, player2);

            // Assert
            Assert.AreEqual(player1, result.winner);
            Assert.AreEqual(player2, result.loser);
        }

        [Test]
        public void TestBattleDraw()
        {
            // Arrange
            User player1 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };
            User player2 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };

            // Act
            var result = BattleHandler.Battle(player1, player2);

            // Assert
            Assert.AreEqual(null, result.winner);
            Assert.AreEqual(null, result.loser);
        }

        [Test]
        public void TestActionBattleWin()
        {
            // Arrange
            User player1 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };
            User player2 = new User() { Actions = new List<Actions>() { Actions.S, Actions.S, Actions.S, Actions.S, Actions.S } };
            List<string> log = new List<string>();
            // Act
            var result = BattleHandler.ActionBattle(player1, player2, log);

            // Assert
            Assert.AreEqual(player1, result);
        }        
        
        [Test]
        public void TestActionBattleLoss()
        {
            // Arrange
            User player1 = new User() { Actions = new List<Actions>() { Actions.S, Actions.S, Actions.S, Actions.S, Actions.S } };
            User player2 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };
            List<string> log = new List<string>();

            // Act
            var result = BattleHandler.ActionBattle(player1, player2, log);

            // Assert
            Assert.AreEqual(player2, result);
        }

        [Test]
        public void TestActionBattleDraw()
        {
            // Arrange
            User player1 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };
            User player2 = new User() { Actions = new List<Actions>() { Actions.R, Actions.R, Actions.R, Actions.R, Actions.R } };
            List<string> log = new List<string>();

            // Act
            var result = BattleHandler.ActionBattle(player1, player2, log);

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}