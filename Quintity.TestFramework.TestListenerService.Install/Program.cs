using System;
using WixSharp;

namespace Quintity.TestFramework.TestEngineer.Setup
{
    class Setup
    {
#if DEBUG
        static private string binSource = @"..\Quintity.TestFramework.TestListenersService.Host\bin\Debug\{0}";
        static private string build = "Debug";
#else
        static private string binSource = @"..\Quintity.TestFramework.TestListenersService.Host\bin\Release\{0}");
        static private string build = "Release";
#endif
        static void Main(string[] args)
        {
            File service = null;

            Project project = new Project("Quintity.TestFramework.TestListenersService",
                new Dir(@"C:\Quintity.TestFramework.TestListenersService",
                    service = new File(string.Format(binSource, "Quintity.TestFramework.TestListenersService.Host.exe")),

                    new Files($@"..\Quintity.TestFramework.TestListenersService.Host\bin\{build}\*.dll"),
                    new Files($@"..\Quintity.TestFramework.TestListenersService.Host\bin\{build}\*.config"),

                    new Dir("%Desktop%",
                        new ExeFileShortcut("Quintity TestLIsteners Service", "[INSTALLDIR]Quintity.TestFramework.TestListenersService.Host.exe", "")),

                    new ExeFileShortcut("Uninstall TestListenersService", "[System64Folder]msiexec.exe", "/x [ProductCode]"))
            );

            service.ServiceInstaller = new ServiceInstaller
            {
                Name = "Quintity TestListenersService",
                Description = "Quintity runtime listener test management service.",

                StartOn = SvcEvent.Install,
                StopOn = SvcEvent.InstallUninstall_Wait,
                RemoveOn = SvcEvent.Uninstall_Wait,
                Arguments = "/S"
            };

            project.OutDir = $@".\bin\{build}\";
            project.ControlPanelInfo.Manufacturer = "Quintity, LLC";
            project.ControlPanelInfo.InstallLocation = "[INSTALLDIR]";
            project.GUID = new Guid("96F03431-CD66-4A3E-958D-E71FE7662BD3");
            project.UI = WUI.WixUI_ProgressOnly;
            Compiler.BuildMsi(project);
        }
    }
}
