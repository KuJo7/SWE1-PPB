using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Net;


namespace PPB.Tests
{
    class DBHandlerTests
    {

        [Test]
        public void TestGetAllUsers()
        {
            // Act
            var result = DBHandler.GetAllUsers();

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void TestGetUser()
        {

            // Arrange
            string username = "kienboec"; 
            // Act
            var result = DBHandler.GetUser(username);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(username, result.Username);
        }
    }
}
