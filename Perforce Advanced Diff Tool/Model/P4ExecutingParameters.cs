using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A Data class for defining parameters to be used for a process to run.
    /// </summary>
    public class P4ExecutingParameters
    {
        private string _fileName;

        private string _command;

        private string _workingDirectoryVar;

        private List<string> _parameters;

        private string _preCommands;

        public P4ExecutingParameters(string fileName, string command, string workingDirectory = null, List<string> parameters = null, string precmd=null)
        {
            Parameters = parameters;
            WorkingDirectory = workingDirectory;
            PreCommand = precmd;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string PreCommand { get=> _preCommands; set=> _preCommands=value; }

        public List<string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }


        public string WorkingDirectory
        {
            get { return _workingDirectoryVar; }
            set { _workingDirectoryVar = value; }
        }


        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }


        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

    }
}
