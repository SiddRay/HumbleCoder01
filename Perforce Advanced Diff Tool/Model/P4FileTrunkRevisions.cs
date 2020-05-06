
using System;
using System.Collections.Generic;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class to contain current file and it's older revisions w.r.t current file.
    /// </summary>
    public class P4FileTrunkRevisions
    {
        private P4File _currentFile;
        private List<P4File> _olderRevisions = new List<P4File>();

        public P4FileTrunkRevisions(P4File currentFile)
        {
            CurrentFile = currentFile ?? throw new ArgumentNullException(nameof(currentFile));
        }

        public P4File CurrentFile { get=>_currentFile; set=>_currentFile=value; }
        public List<P4File> OlderRevisions { get =>_olderRevisions; set =>_olderRevisions=value; }

    }
}
