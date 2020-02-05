using System;
using System.Configuration.Install;
using System.ComponentModel;

namespace TestService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
