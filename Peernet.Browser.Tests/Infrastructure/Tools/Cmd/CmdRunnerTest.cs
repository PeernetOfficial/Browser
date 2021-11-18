using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peernet.Browser.Infrastructure.Tools;
using System.Threading;
using Moq;
using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.Tests.Infrastructure.Tools.Cmd
{
    [TestClass]
    public class CmdRunnerTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var mock = new Mock<ISettingsManager>();
            var o = new CmdRunner(mock.Object);
            //Act
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }

        [TestMethod]
        public void RunTest()
        {
            //Prepare
            var mock = new Mock<ISettingsManager>();
            var o = new CmdRunner(mock.Object);
            //Act
            o.Run();
            Thread.Sleep(5000);
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }
    }
}