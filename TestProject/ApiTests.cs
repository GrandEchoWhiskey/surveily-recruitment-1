using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ApiTests
    {

        [TestMethod]
        public void GetResponse_on_grandechowhiskey_github_io()
        {
            // Arrange
            var a = new Api.Url("https://grandechowhiskey.github.io/");

            // Act
            var result = a.GetResponseStream();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetResponse_on_LocalHost_not_http_server()
        {
            // Arrange
            var a = new Api.Url("127.0.0.1");

            // Act
            var result = a.GetResponseStream();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetResponse_on_nonValidSite()
        {
            // Arrange
            var a = new Api.Url("https://notworkingsite.onetwo/");

            // Act
            var result = a.GetResponseStream();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CheckName_on_ValidName()
        {
            // Arrange
            var a = new Api.Url("https://somedomain.net/");

            // Act
            var result = a.CheckUrl();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CheckName_on_InvalidName()
        {
            // Arrange
            var a = new Api.Url("https://somedomain/.net");

            // Act
            var result = a.CheckUrl();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckName_on_InvalidInterface()
        {
            // Arrange
            var a = new Api.Url("htt://somedomain.net/");

            // Act
            var result = a.CheckUrl();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CheckName_on_LocalHost_ip()
        {
            // Arrange
            var a = new Api.Url("127.0.0.1");

            // Act
            var result = a.CheckUrl();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CheckName_on_Invalid_ip()
        {
            // Arrange
            var a = new Api.Url("364.0.0.1");

            // Act
            var result = a.CheckUrl();

            // Assert
            Assert.AreEqual(false, result);
        }

    }
}
