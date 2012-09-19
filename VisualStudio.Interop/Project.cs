using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Savant.Core.Resources;
using VSLangProj;

namespace VisualStudio.Interop
{
    public class Project : IEquatable<Project>
    {
        #region private constants

        private const string DefaultNamespacePropertyName = "rootnamespace";
        private const string AssemblyNamePropertyName = "AssemblyName";

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

        public string AssemblyName
        {
            get
            {
                return this.GetProjectProperty(Project.AssemblyNamePropertyName);
            }
        }

        public IEnumerable<CodeNamespace> Namespaces
        {
            get
            {
                foreach (var element in this.project.CodeModel.CodeElements.Cast<CodeElement>())
                {
                    if (!element.IsCodeType)
                    {
                        if (element.Kind.Equals(vsCMElement.vsCMElementNamespace))
                        {
                            yield return element as CodeNamespace;
                        }
                    }
                }
            }
        }

        public IEnumerable<ProjectItem> ProjectItems
        {
            get
            {
                return this.project.ProjectItems.Cast<ProjectItem>();
            }
        }

        #endregion

        #region public methods

        #region assembly management methods

        public bool HasAssemblyReference(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(SavantResources.FileNotFoundExceptionMessage, assemblyPath);
            }

            var vsProject = this.project.Object as VSProject;
            return vsProject.References.Cast<Reference>().Any(r => r.Path.Equals(assemblyPath, StringComparison.OrdinalIgnoreCase));
        }

        public void AddAssemblyReference(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(SavantResources.FileNotFoundExceptionMessage, assemblyPath);
            }

            if (!this.HasAssemblyReference(assemblyPath))
            {
                var vsProject = this.project.Object as VSProject;
                vsProject.References.Add(assemblyPath);
            }
        }

        public void RemoveAssemblyReferenceByPath(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(SavantResources.FileNotFoundExceptionMessage, assemblyPath);
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

        public AssemblyReference CheckAssemblyReference(string assemblyName, string version)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentOutOfRangeException("assemblyName", assemblyName);
            }

            var vsProject = this.project.Object as VSProject;
            if (vsProject != null)
            {
                foreach (var projectReference in vsProject.References.Cast<Reference>())
                {
                    if (string.Equals(projectReference.Name, assemblyName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(version))
                        {
                            return AssemblyReference.Current;
                        }
                        var sourceVersion = new Version(version);
                        var referenceVersion = new Version(projectReference.Version);

                        var result = sourceVersion.CompareTo(referenceVersion);
                        if (result < 0)
                        {
                            return AssemblyReference.Newer;
                        }
                        else if (result.Equals(0))
                        {
                            return AssemblyReference.Current;
                        }
                        else if (result > 0)
                        {
                            return AssemblyReference.Older;
                        }
                    }
                }
            }

            return AssemblyReference.Missing;
        }

        public AssemblyReference CheckAssemblyReference(string assemblyName)
        {
            return this.CheckAssemblyReference(assemblyName, null);
        }

        #endregion

        #region project item methods

        public ProjectItem AddProjectItemFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(SavantResources.FileNotFoundExceptionMessage, path);
            }

            return this.project.ProjectItems.AddFromFile(path);
        }

        #endregion

        #region equality methods

        public static bool Equals(Project x, Project y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            if (x.FileName.Equals(y.FileName))
            {
                return true;
            }
            return false;
        }

        public bool Equals(Project other)
        {
            return Project.Equals(this, other);
        }

        public override bool Equals(object objectToCompare)
        {
            return Project.Equals(this, objectToCompare as Project);
        }

        public static bool operator ==(Project x, Project y)
        {
            return Project.Equals(x, y);
        }

        public static bool operator !=(Project x, Project y)
        {
            return !Project.Equals(x, y);
        }

        public override int GetHashCode()
        {
            return this.FileName.GetHashCode();
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
