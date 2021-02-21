using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Net;

namespace PPB.Tests
{
    class PPBTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DBHandler.Login("kienboec", "daniel");
            context.Authorization = "kienboec-ppbToken";
        }

        RequestContext context = new RequestContext("", "", "", new Dictionary<string, string>(), "kienboec", "");

        [Test]
        public void TestProcessRequestLogin()
        {
            // Arrange
            context.Httpverb = "POST";
            context.Resource = "/sessions";
            context.Body = "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}";
            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.AreEqual("\"kienboec is logged in\"", result.returnBody);
        }

        [Test]
        public void TestProcessRequestGetPlaylist()
        {
            // Arrange
            context.Httpverb = "GET";
            context.Resource = "/playlist";
            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.NotNull(result.returnBody);
        }

        [Test]
        public void TestProcessRequestGetUserProfile()
        {
            // Arrange
            context.Httpverb = "GET";
            context.Resource = "/users/kienboec";

            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.AreEqual("{\"Name\":\"Kienboeck\",\"Bio\":\"me playin...\",\"Image\":\":-\"}", result.returnBody);
        }

        [Test]
        public void TestProcessRequestUpdateUser()
        {
            // Arrange
            context.Httpverb = "PUT";
            context.Resource = "/users/kienboec";
            context.Body = "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-\"}";

            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.AreEqual("\"Profile of kienboec updated\"", result.returnBody);
        }

        [Test]
        public void TestProcessRequestGetUserStats()
        {
            // Arrange
            context.Httpverb = "GET";
            context.Resource = "/stats";

            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.NotNull(result.returnBody);
        }

        [Test]
        public void TestProcessRequestGetLibrary()
        {
            // Arrange
            context.Httpverb = "GET";
            context.Resource = "/lib";
            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.NotNull(result.returnBody);
        }

        [Test]
        public void TestProcessRequestGetActions()
        {
            // Arrange
            context.Httpverb = "GET";
            context.Resource = "/actions";
            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.NotNull(result.returnBody);
        }

        [Test]
        public void TestProcessRequestPutActions()
        {
            // Arrange
            context.Httpverb = "PUT";
            context.Resource = "/actions";
            context.Body = "{\"actions\": \"RRRRR\"}";

            // Act
            var result = PPB.ProcessRequest(context);

            // Assert
            Assert.AreEqual("\"kienboec set actions\"", result.returnBody);
        }
    }
}