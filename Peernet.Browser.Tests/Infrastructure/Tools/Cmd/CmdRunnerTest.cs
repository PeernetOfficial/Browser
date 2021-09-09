using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var o = new CmdRunner();
            //Act
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }

        [TestMethod]
        public void RunTest()
        {
            //Prepare
            var o = new CmdRunner();
            //Act
            o.Run();
            Thread.Sleep(5000);
            o.Dispose();
            //Assert
            Assert.IsNotNull(o);
        }
    }
}