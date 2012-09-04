using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualStudio.PlatformUI;

namespace Savant.Interface
{
    /// <summary>
    /// Interaction logic for AdapterGeneratorWindow.xaml
    /// </summary>
    public partial class AdapterGeneratorWindow : DialogWindow
    {
        public AdapterGeneratorWindow()
            : base()
        {
            this.InitializeComponent();

            // EXAMPLE: http://blogs.microsoft.co.il/blogs/davids/archive/2009/06/04/hierarchicaldatatemplate-and-treeview.aspx
            // EXAMPLE: http://blogs.microsoft.co.il/blogs/davids/archive/2009/06/05/treeview-databinding-and-the-composite-pattern.aspx
            treeView.ItemsSource = GetProjects();
        }

        public List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>()
            {
                new Project()
                {
                    Name = "EventRegistration.Data",
                    Items = new List<ProjectItem>()
                    {
                        new ClassProjectItem(){ Name = "Adapter1.cs" },
                        new ClassProjectItem(){ Name = "Adapter2.cs" },
                        new ClassProjectItem(){ Name = "Adapter3.cs" },
                        new ClassProjectItem(){ Name = "Adapter4.cs" },
                        new ClassProjectItem(){ Name = "Adapter5.cs" },
                        new ClassProjectItem(){ Name = "Adapter6.cs" },
                        new ClassProjectItem(){ Name = "Adapter7.cs" },
                        new ClassProjectItem(){ Name = "Adapter8.cs" },
                        new ClassProjectItem(){ Name = "Adapter9.cs" }
                    }
                },
                new Project()
                {
                    Name = "EventRegistration.Extensions",
                    Items = new List<ProjectItem>()
                    {
                        new ClassProjectItem(){ Name = "BooleanExtensions.cs" },
                        new ClassProjectItem(){ Name = "ByteExtensions.cs" },
                        new ClassProjectItem(){ Name = "WPFExtensions.cs" },
                        new ClassProjectItem(){ Name = "StringExtensions.cs" }
                    }
                },
                new Project()
                {
                    Name = "EventRegistration.Model",
                    Items = new List<ProjectItem>()
                    {
                        new FolderProjectItem(){ Name = "EventManagement", Items = new List<ProjectItem>()
                        {
                            new FolderProjectItem(){ Name = "Commands", Items = new List<ProjectItem>()
                            {
                                new ClassProjectItem(){ Name = "InsertEventCommand.cs" },
                                new ClassProjectItem(){ Name = "UpdateEventCommand.cs" },
                                new ClassProjectItem(){ Name = "SaveEventCommand.cs" }
                            }},
                            new ClassProjectItem(){ Name = "EventFactory.cs" },
                            new ClassProjectItem(){ Name = "Event.cs" },
                            new ClassProjectItem(){ Name = "EventCommandFactory.cs" },
                            new ClassProjectItem(){ Name = "EventBase.cs" }
                        }},
                        new FolderProjectItem(){ Name = "Membership", Items = new List<ProjectItem>()
                        {
                            new FolderProjectItem(){ Name = "Commands", Items = new List<ProjectItem>()
                            {
                                new ClassProjectItem(){ Name = "InsertUserCommand.cs" },
                                new ClassProjectItem(){ Name = "UpdateUserCommand.cs" },
                                new ClassProjectItem(){ Name = "SaveUserCommand.cs" }
                            }},
                            new ClassProjectItem(){ Name = "User.cs" },
                            new ClassProjectItem(){ Name = "UserFactory.cs" },
                            new ClassProjectItem(){ Name = "UserCommandFactory.cs" },
                            new ClassProjectItem(){ Name = "UserManager.cs" },
                            new ClassProjectItem(){ Name = "UserBase.cs" }
                        }},
                        new FolderProjectItem(){ Name = "Xaml", Items = new List<ProjectItem>()
                        {
                            new ClassProjectItem(){ Name = "MagicXamlForm1.cs" },
                            new ClassProjectItem(){ Name = "MagicXamlForm2.cs" }
                        }},
                        new ClassProjectItem(){ Name = "ApplicationState.cs" },
                        new ClassProjectItem(){ Name = "IKeyed.cs" },
                        new ClassProjectItem(){ Name = "IRefreshable.cs" },
                        new ClassProjectItem(){ Name = "IControlled.cs" },
                        new ClassProjectItem(){ Name = "SessionState.cs" }
                    }
                }
            };

            return projects;
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public List<ProjectItem> Items { get; set; }
    }

    public class ProjectItem
    {
        public string Name { get; set; }
        public ProjectItemType ItemType { get; set; }
        public List<ProjectItem> Items { get; set; }
    }

    public class FolderProjectItem : ProjectItem
    {
        public FolderProjectItem()
        {
            ItemType = ProjectItemType.Folder;
        }
    }

    public class ClassProjectItem : ProjectItem
    {
        public ClassProjectItem()
        {
            ItemType = ProjectItemType.Class;
        }
    }

    public enum ProjectItemType
    {
        Class,
        Folder
    }
}
