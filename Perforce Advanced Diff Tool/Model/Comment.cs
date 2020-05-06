using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// Data class for Comment and its fields
    /// </summary>
    public class Comment
    {
        private string _sourceVersion ;
        private string _destinationVersion;


        public string SourceVersion { get=> _sourceVersion; set=> _sourceVersion = value; }

        public string DestinationVersion { get => _destinationVersion; set => _destinationVersion = value; }


        private CommentState _state = CommentState.NoMatch;

        public CommentState CommentState { get=>_state; set=>_state = value; }
    }

    /// <summary>
    /// Describes whether the source file and destination file matched or mismatched.
    /// Ideally this enum should be in a different file.
    /// </summary>
    public enum CommentState
    {
        Match = 1,
        NoMatch = 0
    }
}
