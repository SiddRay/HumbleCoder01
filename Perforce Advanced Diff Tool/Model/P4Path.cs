using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class containing path of a file as described in perforce.
    /// </summary>
    public class P4Path
    {

        private string _filepath;

        public P4Path()
        {
        }

        public P4Path(string filepath)
        {
            _filepath = filepath ?? throw new ArgumentNullException(nameof(filepath));
        }

        public string FilePath { get=>_filepath; set=>_filepath = value; }
    }
}
