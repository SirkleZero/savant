using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using VSLangProj;

namespace VisualStudio.Interop
{
    public class Project
    {
        #region private constants

        private const string DefaultNamespacePropertyName = "rootnamespace";

        #endregion

        #region private fields

        private readonly EnvDTE.Project project;

        #endregion

        #region constructors

        public Project(EnvDTE.Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            this.project = project;
        }

        #endregion

        #region public properties

        public string Name
        {
            get
            {
                return this.project.Name;
            }
        }

        public string FullName
        {
            get
            {
                return this.project.FullName;
            }
        }

        public string FileName
        {
            get
            {
                return this.project.FileName;
            }
        }

        public string Directory
        {
            get
            {
                return Path.GetDirectoryName(this.FullName);
            }
        }

        public string Language
        {
            get
            {
                return this.project.CodeModel.Language;
            }
        }

        public string DefaultNamespace
        {
            get
            {
                return this.GetProjectProperty(Project.DefaultNamespacePropertyName);
            }
        }

        #endregion

        #region public methods

        #region assembly management methods

        public bool HasAssemblyReference(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentOutOfRangeException("assemblyPath");
            }
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException();
            }

            var vsProject = this.project.Object as VSProject;
            return vsProject.References.Cast<Reference>().Any(reference => reference.Path.Equals(assemblyPath, StringComparison.OrdinalIgnoreCase));
        }

        public void AddAssemblyReference(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentOutOfRangeException("assemblyPath");
            }
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException();
            }

            if (!this.HasAssemblyReference(assemblyPath))
            {
                var vsProject = this.project.Object as VSProject;
                vsProject.References.Add(assemblyPath);
            }
        }

        public void RemoveAssemblyReferenceByPath(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentOutOfRangeException("assemblyPath");
            }
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException();
            }

            if (this.HasAssemblyReference(assemblyPath))
            {
                var vsProject = this.project.Object as VSProject;
                foreach (var r in vsProject.References.Cast<Reference>())
                {
                    if (r.Path.Equals(assemblyPath, StringComparison.OrdinalIgnoreCase))
                    {
                        r.Remove();
                    }
                }
            }
        }

        public void RemoveAssemblyReferenceByName(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentOutOfRangeException("assemblyName");
            }

            var vsProject = this.project.Object as VSProject;
            foreach (var reference in vsProject.References.Cast<Reference>())
            {
                if (reference.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
                {
                    reference.Remove();
                }
            }
        }

        #endregion

        #endregion

        #region private methods

        private string GetProjectProperty(string propertyName)
        {
            // TODO: Add caching to this?
            for (var i = 1; i < this.project.Properties.Count; i++)
            {
                if (string.Equals(this.project.Properties.Item(i).Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return this.project.Properties.Item(i).Value.ToString();
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
