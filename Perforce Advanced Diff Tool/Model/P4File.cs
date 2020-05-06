using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A Data Class to contain a perforce file and related attributes to a file.
    /// This class can be further segmented.
    /// </summary>
    public class P4File
    {
        private string _fileName;
        private P4Path _currentPath;
        private P4FileMetaData _fileMetaData;

        public string FileName { get => _fileName; set => _fileName = value; }
        public P4Path CurrentPath { get => _currentPath; set => _currentPath = value; }

        public P4FileMetaData FileMetaData { get => _fileMetaData; set => _fileMetaData = value; }


        public P4File(string fileName, string filePath)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            P4Path path = new P4Path(filePath);
            CurrentPath = path ?? throw new ArgumentNullException(nameof(path));
        }

        public P4File()
        {
        }
    }
}
