using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure;
using RestSharp;

namespace Peernet.Browser.Tests.Infrastructure
{
    [TestClass]
    public class ApiClientTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var s1 = new Mock<IRestClientFactory>();
            var s2 = new Mock<ISettingsManager>();
            s2.Setup(foo => foo.ApiUrl).Returns("http://127.0.0.1:112");
            //Act
            var o = new ApiClient(s1.Object, s2.Object);
            //Assert
            Assert.IsNotNull(o);
        }
    }
}