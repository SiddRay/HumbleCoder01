using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// IEnumerable ResultItem class is mapped to the columns of the main UI and 
    /// is used to display the result row in main UI.
    /// </summary>
    class ResultItem : IEnumerable<string>
    {
        List<string> totalItems = new List<string>();
        private string _filename1;
        private string _CurrentVersion1;
        private string _filePath1;
        private string _state1;
        private string _status;
        private string _filename2;
        private string _CurrentVersion2;
        private string _filePath2;
        private string _state2;
        private string _comment;

        public string Filename1 { get=> _filename1; set => _filename1=value; }
        public string CurrentVersion1 { get => _CurrentVersion1; set => _CurrentVersion1=value; }
        public string FilePath1 { get => _filePath1; set => _filePath1=value; }
        public string State1 { get => _state1; set => _state1=value; }
        public string Status { get => _status; set => _status=value; }
        public string Filename2 { get => _filename2; set => _filename2=value; }
        public string CurrentVersion2 { get => _CurrentVersion2; set => _CurrentVersion2=value; }
        public string FilePath2 { get => _filePath2; set => _filePath2=value; }
        public string State2 { get => _state2; set => _state2=value; }
        public string Comment { get => _comment; set => _comment=value; }


        public ResultItem()
        {
            totalItems.Add(_filename1);
            totalItems.Add(_filePath1);
            totalItems.Add(_CurrentVersion1);
            totalItems.Add(_state1);
            totalItems.Add(_status);
            totalItems.Add(_filename2);
            totalItems.Add(_filePath2);
            totalItems.Add(_CurrentVersion2);
            totalItems.Add(_state2);
            totalItems.Add(_comment);
        }

        public ResultItem(string filename1, string filePath1, string CurrentVersion1, string state1, string status, string filename2, string filePath2, string CurrentVersion2, string state2, string comment)
        {
            _filename1 = filename1;
            _CurrentVersion1 = CurrentVersion1;
            _filePath1 = filePath1;
            _state1 = state1;
            _status = status;
            _filename2 = filename2;
            _CurrentVersion2 = CurrentVersion2;
            _filePath2 = filePath2;
            _state2 = state2;
            _comment = comment;
        }

        /// <summary>
        /// Method to enumerate on result rows calculated.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach(var item in totalItems)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Method returning current object's enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
