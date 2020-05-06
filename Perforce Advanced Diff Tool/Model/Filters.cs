using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// Filters Data class defines the current selected values of various filters in the Main Window.
    /// </summary>
    public class Filters
    {
        private bool _findMatch;

        public bool FindMatch
        {
            get { return _findMatch; }
            set { _findMatch = value; }
        }

        private FilesToDisplay _filesDisplayType;
       
        public FilesToDisplay FilesDisplayType
        {
            get { return _filesDisplayType; }
            set { _filesDisplayType = value; }
        }

        public Filters(bool findMatch, FilesToDisplay filesDisplayType)
        {
            FindMatch = findMatch;
            FilesDisplayType = filesDisplayType;
        }

        public Filters()
        {
        }
    }
}
