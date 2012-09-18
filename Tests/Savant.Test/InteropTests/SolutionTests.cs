using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace Savant.Test.InteropTests
{
    [TestClass]
    public class SolutionTests
    {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ValidateSolutionProxy()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                DTE dte = (DTE)VsIdeTestHostContext.ServiceProvider.GetService(typeof(DTE));
                dte.Solution.Open(@"");

                var s = new VisualStudio.Interop.Solution(dte.Solution);
                Assert.AreEqual(2, s.Projects.Count());
            });
        }
    }
}
