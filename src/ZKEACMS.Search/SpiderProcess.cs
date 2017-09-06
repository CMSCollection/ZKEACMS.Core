/* http://www.zkea.net/ 
 * Copyright 2017 
 * ZKEASOFT 
 * http://www.zkea.net/licenses 
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ZKEACMS.Search
{
    public static class SpiderProcess
    {
        public static string OutputMessage { get; set; }
        public static string ErrorMessage { get; set; }
        static Process mainProcess;
        public static void Start(string app)
        {
            if (mainProcess == null)
            {
                OutputMessage = string.Empty;
                ErrorMessage = string.Empty;
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = app;
                psi.Arguments = typeof(SearchPlug).Assembly.Location;
                psi.WorkingDirectory = PluginBase.GetPath<SearchPlug>();
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.RedirectStandardInput = false;
                psi.UseShellExecute = false;
                mainProcess = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true
                };

                mainProcess.Exited += Proc_Exited;
                mainProcess.ErrorDataReceived += Proc_ErrorDataReceived;
                mainProcess.OutputDataReceived += Proc_OutputDataReceived;

                mainProcess.Start();
                mainProcess.BeginErrorReadLine();
                mainProcess.BeginOutputReadLine();
            }
        }

        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputMessage.Length > 1000)
            {
                OutputMessage = string.Empty;
            }
            if (e.Data != null)
            {
                OutputMessage += e.Data + "\r\n";
            }
        }

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                ErrorMessage += e.Data + "\r\n";
            }
        }

        private static void Proc_Exited(object sender, EventArgs e)
        {
            OutputMessage = string.Empty;
            mainProcess.Dispose();
            mainProcess = null;
        }

    }
}
