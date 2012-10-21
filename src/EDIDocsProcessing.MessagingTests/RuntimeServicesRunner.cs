using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EDIDocsProcessing.MessagingTests
{
    public static class RuntimeServicesRunner
    {
        private static Process _runtimeServicesProcess;

        public static void EnsureRuntimeServicesAreRunning()
        {
            Process[] processes = Process.GetProcesses();
            bool isRuntimeServicesRunning =
                processes.Where(x => x.ProcessName == "MassTransit.RuntimeServices").Count() > 0;

            if (isRuntimeServicesRunning == false)
            {
                ProcessStartInfo processStartInfo =
                    findRuntimeServicesPath(Directory.GetParent(Directory.GetCurrentDirectory()));
                _runtimeServicesProcess = Process.Start(processStartInfo);
            }
        }

        private static ProcessStartInfo findRuntimeServicesPath(DirectoryInfo startDirectory)
        {
            List<DirectoryInfo> distDirectories = startDirectory.GetDirectories().Where(x => x.Name == "dist").ToList();

            if (distDirectories.Count() == 0)
            {
                return findRuntimeServicesPath(startDirectory.Parent);
            }

            FileInfo fileInfo =
                distDirectories[0].GetDirectories("MassTransit")[0].GetDirectories("net-4.0")[0].GetDirectories("RuntimeServices")[0].GetFiles("MassTransit.RuntimeServices.exe")[0];
            var startInfo = new ProcessStartInfo(fileInfo.FullName);
            return startInfo;
        }

        public static void Kill()
        {
            if (_runtimeServicesProcess != null) _runtimeServicesProcess.Kill();
        }
    }
}