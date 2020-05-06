using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submission_Tracker.Model;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A Data class.
    /// A file clustor defines which file version (i.e. matched file) of a file 
    /// (i.e. original file in the path) is matched.
    /// </summary>
    public class FileCluster
    {
        P4File _matchedFile, _originalFile;
        private bool calculated = false;

        public bool Calculated
        {
            get { return calculated; }
            set { calculated = value; }
        }

        public FileCluster(P4File matchedFile, P4File originalFile)
        {
            _matchedFile = matchedFile;
            _originalFile = originalFile;
        }

        public P4File MatchedFile { get =>_matchedFile; set=> _matchedFile = value; }
        public P4File OriginalFile { get => _originalFile; set => _originalFile = value; }

    }
}
