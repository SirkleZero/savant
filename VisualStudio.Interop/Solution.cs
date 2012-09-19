using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace VisualStudio.Interop
{
    public class Solution
    {
        private readonly EnvDTE.Solution solution;

        public Solution(EnvDTE.Solution solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            this.solution = solution;
        }

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
                return this.solution.Properties.Item("name").Value.ToString();
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
                return from p in this.solution.Projects.Cast<EnvDTE.Project>()
                       select new Project(p);
            }
        }
    }
}
