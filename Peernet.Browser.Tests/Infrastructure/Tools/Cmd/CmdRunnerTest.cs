using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Tools;
using System.Threading;

namespace Peernet.Browser.Tests.Infrastructure.Tools.Cmd
{
    [TestClass]
    public class CmdRunnerTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var settingsManagerMock = new Mock<ISettingsManager>();
            var shutdownServiceMock = new Mock<IShutdownService>();
            var apiServiceMock = new Mock<IApiService>();
            var o = new CmdRunner(settingsManagerMock.Object, shutdownServiceMock.Object, apiServiceMock.Object);
            //Act
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }

        [TestMethod]
        public void RunTest()
        {
            //Prepare
            var settingsManagerMock = new Mock<ISettingsManager>();
            var shutdownServiceMock = new Mock<IShutdownService>();
            var apiServiceMock = new Mock<IApiService>();
            var o = new CmdRunner(settingsManagerMock.Object, shutdownServiceMock.Object, apiServiceMock.Object);
            //Act
            o.Run();
            Thread.Sleep(5000);
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }
    }
}