using System;
using System.Linq;
using Microsoft.Win32;
using WixSharp;
using WixSharp.CommonTasks;

namespace Quintity.TestFramework.TestEngineer.Setup
{
    class Setup
    {
#if DEBUG
        static private string build = "Debug";
#else
        static private string build = "Release";
#endif
        static void Main(string[] args)
        {
            Project project = new Project("Quintity.TestFramework.TestListenersService",
                new Dir(@"C:\Quintity.TestFramework.TestListenersService",
                    //new File($@"C:\DevProjects\Quintity\Repos\Quintity.TestFramework.TestListenersService\Quintity.TestFramework.TestListenersService.Host\bin\Debug\Quintity.TestFramework.TestListenersService.Host.exe",
                    new File($@"..\Quintity.TestFramework.TestListenersService.Host\bin\{build}\Quintity.TestFramework.TestListenersService.Host.exe",
                        new FileShortcut("Quintity TestListeners Service", @"%Desktop%")),
                    new Files($@"..\Quintity.TestFramework.TestListenersService.Host\bin\{build}\*.dll"),
                    new Files($@"..\Quintity.TestFramework.TestListenersService.Host\bin\{build}\*.config"))
            );

            project.OutDir = $@".\bin\{build}\";
            project.GUID = new Guid("96F03431-CD66-4A3E-958D-E71FE7662BD3");
            project.UI = WUI.WixUI_ProgressOnly;
            project.BuildMsi();
        }
    }
}
