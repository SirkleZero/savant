using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace Savant.Test.InteropTests
{
    [TestClass]
    public class SolutionTests
    {
        #region fields

        private delegate void ThreadInvoker();
        private TestContext _testContext;

        #endregion

        #region properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }

        #endregion

        #region ctors

        public SolutionTests() { }

        #endregion

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        [HostType("VS IDE")]
        public void ValidateSolutionProxy()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                string solutionName = "UnitTestSoloution";
                string solutionDirectory = Path.Combine(TestContext.TestDir, solutionName);
                string projectName = "UnitTestConsoleApplication";

                TestUtils testUtils = new TestUtils();
                DTE dte = (DTE)VsIdeTestHostContext.ServiceProvider.GetService(typeof(DTE));
                var solution = new VisualStudio.Interop.Solution(dte.Solution);

                // create an empty solution
                testUtils.CreateEmptySolution(TestContext.TestDir, solutionName);
                Assert.AreEqual<int>(solution.Projects.Count(), testUtils.ProjectCount());

                // add a console application to the solution and verify it
                testUtils.CreateProjectFromTemplate(projectName, "Console Application", "CSharp", false);
                Assert.AreEqual<int>(solution.Projects.Count(), testUtils.ProjectCount());

                // test the solution name
                Assert.AreEqual<string>(solutionName, solution.Name);

                // test the solution file
                Assert.IsTrue(File.Exists(solution.FullName));

                // test the solution directory
                Assert.IsTrue(Directory.Exists(solution.Directory));
            });
        }
    }
}
