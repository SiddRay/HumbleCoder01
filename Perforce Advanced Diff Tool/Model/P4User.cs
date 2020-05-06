using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class for perforce user. This class can be extended for more attributes related to perforce user.
    /// </summary>
    public class P4User
    {
        private string _username;
        private string _password;
        private List<P4WorkSpace> _workSpaces;
        private List<P4Changelist> _changelists;

        public string Username
        {
            get => _username;
            set => _username = value;
        }
    }
}
