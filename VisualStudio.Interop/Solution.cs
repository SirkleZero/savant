using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using VisualStudio.Interop.Ioc;
using DteProject = EnvDTE.Project;
using DteSolution = EnvDTE.Solution;

namespace VisualStudio.Interop
{
    /// <summary>
    /// Provides a proxy to simplify working with <see cref="EnvDTE.Solution"/> objects.
    /// </summary>
    public class Solution
    {
        #region private constants

        private const string SolutionNamePropertyName = "name";

        #endregion

        #region private fields

        private readonly EnvDTE.Solution solution;

        #endregion

        #region constructors

        public Solution() : this(ServiceLocator.GetInstance<DTE>()) { }

        public Solution(DteSolution solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }

            this.solution = solution;
        }

        public Solution(DTE dte)
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
                return Path.GetDirectoryName(this.FullName);
            }
        }

        public IEnumerable<Project> Projects
        {
            get
            {
                return from project in this.solution.Projects.Cast<DteProject>()
                       select new Project(project);
            }
        }

        #endregion
    }
}
