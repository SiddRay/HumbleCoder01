using System.Collections.Generic;
using System.Diagnostics;
using Submission_Tracker.Model;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A class defining all the methods for executing a perforce command.
    /// This is a segmented class which consumes fields of P4ExecutingParameters data only class.
    /// As a design choice the two classes can be merged together.
    /// The segmentation is made with an insight of defining different roles for an executer and data container class during multi-threaded/distributed environment.
    /// </summary>
    public class P4Execute
    {
        //Messages msg = new Messages();

        /// <summary>
        /// Executes the process defined in executing parameters.
        /// </summary>
        /// <param name="exeParameters"></param>
        /// <returns></returns>
        public string ExecuteCommand(P4ExecutingParameters exeParameters)
        {
            string output = null;
            if (!exeParameters.Equals(null))
            {
                Process process = new Process();
                process.StartInfo = createStartInfo(exeParameters);
                process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
                process.Start();
                process.BeginErrorReadLine();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            return output;
        }
        
        /// <summary>
        /// Handles Errors by a process and reports.
        /// </summary>
        /// <param name="sendingProcess"></param>
        /// <param name="errLine"></param>
        private static void ErrorDataHandler(object sendingProcess,
            DataReceivedEventArgs errLine)
        {
            if (!string.IsNullOrEmpty(errLine.Data))
            {
                Messages msg = new Messages(errLine.Data.Trim());
                msg.PublishMessage();
                // Stop execution.
                return;
            }
        }

        /// <summary>
        /// Creates the arguments of a .NET process. 
        /// </summary>
        /// <param name="exeParameters"></param>
        /// <returns></returns>
        private ProcessStartInfo createStartInfo(P4ExecutingParameters exeParameters)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = exeParameters.FileName;
            //psi.WorkingDirectory = exeParameters.WorkingDirectory;
            psi.Arguments = CreateCommandArguments(exeParameters);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.Verb = "runas";
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            return psi;
        }

        /// <summary>
        /// Parses the executing parameters into right format to be passed to perforce in process call.
        /// </summary>
        /// <param name="exeParameters"></param>
        /// <returns></returns>
        private string CreateCommandArguments(P4ExecutingParameters exeParameters)
        {
            string cmd = "/c " + exeParameters.PreCommand + "p4 " + exeParameters.Command + " ";
            string cmd2 = exeParameters.Parameters != null ? Concatcommands(exeParameters.Parameters) : "";
            cmd = cmd + @cmd2.Replace("\\","/");
            return @cmd;
        }

        /// <summary>
        /// Parses the commands in right format to be used to create a command.
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        private string Concatcommands(List<string> cmds)
        {
            string totalString = "";
            foreach(var item in cmds)
            {
                totalString = totalString + @item + " ";
            }
            return @totalString;
        }
    }
}
