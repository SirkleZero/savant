using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudio.Interop.Ioc;

namespace VisualStudio.Interop
{
    public class Solution
    {
        #region private constants

        private const string SolutionNamePropertyName = "name";

        #endregion

        #region private fields

        private readonly EnvDTE.Solution solution;

        #endregion

        #region constructors

        public Solution() : this(ServiceLocator.GetInstance<EnvDTE.DTE>()) { }

        public Solution(EnvDTE.Solution solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }

            this.solution = solution;
        }

        public Solution(EnvDTE.DTE dte)
        {
            if (dte == null)
            {
                throw new ArgumentNullException("dte");
            }

            this.solution = dte.Solution;
        }

        #endregion

        #region public properties

        public string FullName
        {
            get
            {
                return this.solution.FullName;
            }
        }

        public string Name
        {
            get
            {
                return this.solution.Properties.Item(Solution.SolutionNamePropertyName).Value.ToString();
            }
        }

        public string Directory
        {
            get
            {
                return System.IO.Path.GetDirectoryName(this.FullName);
            }
        }

        public IEnumerable<Project> Projects
        {
            get
            {
                return from project in this.solution.Projects.Cast<EnvDTE.Project>()
                       select new Project(project);
            }
        }

        #endregion
    }
}
