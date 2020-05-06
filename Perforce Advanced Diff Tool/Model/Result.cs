
using System;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class Result contains the basic elements of a row visible in main UI.
    /// A row has repeated properties for source and destination. This class contains
    /// data related to such repeated properties while computation is done.
    /// </summary>
    public class Result
    {
        private string _filename;
        private string _CurrentVersion;
        private string _filePath;
        private FileState _status = FileState.Latest;

        public Result()
        {
        }

        public Result(string filename, string currentVersion, string filePath)
        {
            Filename = filename ?? throw new ArgumentNullException(nameof(filename));
            CurrentVersion = currentVersion ?? throw new ArgumentNullException(nameof(currentVersion));
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public string Filename
        { get=>_filename; set=>_filename=value; }

        public string CurrentVersion
        { get => _CurrentVersion; set => _CurrentVersion = value; }

        public string FilePath
        { get => _filePath; set => _filePath = value; }

        public FileState Status
        { get => _status; set => _status = value; }

    }
}
