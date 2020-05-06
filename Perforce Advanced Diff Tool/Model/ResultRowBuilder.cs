using System;
using System.Collections.Generic;
using System.Linq;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// ResultRowBuilder processes the parsed data after computation to generate a row and applies filter to the row.
    /// </summary>
    public class ResultRowBuilder
    {
        private Dictionary<string, FileCluster> _calculated = new Dictionary<string, FileCluster>();
        public ResultRowBuilder()
        { }
        public ResultRow GenerateRow(Dictionary<string, FileCluster> leftData, Dictionary<string, FileCluster> rightData, Filters filters)
        {
            FileCluster leftfiles = leftData[leftData.Keys.First()];
            FileCluster rightfiles = null;
            if (rightData !=null)
             rightfiles = rightData[rightData.Keys.First()];

            ResultRow row = new ResultRow();
            FileCluster contentName = _calculated.ContainsKey(leftfiles.MatchedFile.FileName)
                ? _calculated[leftfiles.MatchedFile.FileName]
                : null;
            if ( contentName != null && (_calculated[leftfiles.MatchedFile.FileName].Calculated || contentName.OriginalFile.FileName.Equals(leftfiles.OriginalFile.FileName)))
                return null;
            else
            {
                leftfiles.Calculated = true;
                _calculated[leftfiles.OriginalFile.FileName] = leftfiles;
                Result left = new Result(leftfiles.MatchedFile.FileName, leftfiles.MatchedFile.FileMetaData.HeadRevision,leftfiles.MatchedFile.CurrentPath.FilePath);

                Result right = new Result();
                if (rightData != null)
                    right = new Result(rightfiles.MatchedFile.FileName, rightfiles.MatchedFile.FileMetaData.HeadRevision, rightfiles.MatchedFile.CurrentPath.FilePath);

                row.SourceResult = left;
                row.DestinationResult = right;
                row.Comment.SourceVersion = (leftfiles.MatchedFile.FileMetaData.HeadRevision);
                if (rightData != null)
                    row.Comment.DestinationVersion = (rightfiles.MatchedFile.FileMetaData.HeadRevision);

                left.Status = GetStatus(leftfiles.OriginalFile.FileMetaData.HeadRevision,left.CurrentVersion);
                if (rightData != null)
                    right.Status = GetStatus(rightfiles.OriginalFile.FileMetaData.HeadRevision,right.CurrentVersion);
                else
                    right.Status = FileState.Conflict;

                if (rightData != null && Int32.Parse(left.CurrentVersion) == Int32.Parse(right.CurrentVersion))
                    row.Comment.CommentState = CommentState.Match;
                else if (rightData != null && Int32.Parse(left.CurrentVersion) > Int32.Parse(right.CurrentVersion))
                    row.Comment.CommentState = CommentState.Match;
                else if (rightData == null)
                    row.Comment.CommentState = CommentState.NoMatch;

                return row;
            }

        }

        /// <summary>
        /// Method to decide if a file is latest match or not.
        /// </summary>
        /// <param name="currentVersion"></param>
        /// <param name="MatchVersion"></param>
        /// <returns></returns>
        private FileState GetStatus(string currentVersion, string MatchVersion)
        {
            if (Int32.Parse(currentVersion) == Int32.Parse(MatchVersion))
                return FileState.Latest;
            else
                return FileState.Conflict;
        }
       
    }
}
