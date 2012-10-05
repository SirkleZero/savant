using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace Savant.Test.InteropTests
{
    [TestClass]
    public class SolutionTests
    {
        #region

        private const string SolutionName = "UnitTestSoloution";
        private const string ConsoleApplicationProjectName = "UnitTestConsoleApplication";
        private const string CSharpLanguageName = "CSharp";
        private const string ConsoleProjectTemplateName = "Console Application";

        #endregion

        #region fields

        private delegate void ThreadInvoker();

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

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
        public void CreateEmptySolution()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                TestUtils testUtils = new TestUtils();
                DTE dte = (DTE)VsIdeTestHostContext.ServiceProvider.GetService(typeof(DTE));

                // create the visual studio solution proxy object that we want to test.
                var solution = new VisualStudio.Interop.Solution(dte.Solution);

                // create an empty solution
                testUtils.CreateEmptySolution(TestContext.TestDir, SolutionTests.SolutionName);
                Assert.AreEqual<int>(solution.Projects.Count(), testUtils.ProjectCount());

                //test the solution name
                Assert.AreEqual<string>(SolutionTests.SolutionName, solution.Name);

                // test the solution file
                Assert.IsTrue(File.Exists(solution.FullName));

                // test the solution directory
                Assert.IsTrue(Directory.Exists(solution.Directory));
            });
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CreateEmptyConsoleApplication()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                TestUtils testUtils = new TestUtils();
                DTE dte = (DTE)VsIdeTestHostContext.ServiceProvider.GetService(typeof(DTE));

                // create the visual studio solution proxy object that we want to test.
                var solution = new VisualStudio.Interop.Solution(dte.Solution);

                // create an empty solution
                testUtils.CreateEmptySolution(TestContext.TestDir, SolutionTests.SolutionName);
                Assert.AreEqual<int>(solution.Projects.Count(), testUtils.ProjectCount());

                // add a console application to the solution and verify it
                testUtils.CreateProjectFromTemplate(SolutionTests.ConsoleApplicationProjectName, SolutionTests.ConsoleProjectTemplateName, SolutionTests.CSharpLanguageName, false);
                Assert.AreEqual<int>(solution.Projects.Count(), testUtils.ProjectCount());

                // test that the project was added successfully
                Assert.IsTrue(solution.Projects.Any(p => p.Name.Equals(SolutionTests.ConsoleApplicationProjectName, StringComparison.OrdinalIgnoreCase)));

                // test that the file is on disk
                var filename = solution.Projects.First(p => p.Name.Equals(SolutionTests.ConsoleApplicationProjectName, StringComparison.OrdinalIgnoreCase)).FullName;
                Assert.IsTrue(File.Exists(filename));
            });
        }
    }
}
