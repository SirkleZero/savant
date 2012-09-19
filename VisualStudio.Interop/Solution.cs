using System;
using System.Collections.Generic;
using System.Linq;

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

        public Solution(EnvDTE.Solution solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            this.solution = solution;
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
