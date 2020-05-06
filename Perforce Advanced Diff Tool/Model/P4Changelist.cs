using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A Data Class.
    /// Describes a subset of properties of Perforce's description of a changelist.
    /// Not all fields may be currently used. Extension of fields may be required to define
    /// more ownership and permission fields.
    /// </summary>
    public class P4Changelist
    {
        private int _changelsitNumber;
        private string _changeListdate;
        private string _owner;
        private string _changeListDescription;
        private List<P4File> _changeListFiles;

        public P4Changelist()
        {
        }

        public P4Changelist(int changelsitNumber)
        {
            _changelsitNumber = changelsitNumber;
        }

        public int ChangelistId
        {
            get => _changelsitNumber;
            set => _changelsitNumber = value;
        }

        public string ChangeListdate
        {
            get => _changeListdate;
            set => _changeListDescription = value;
        }

        public string P4User
        {
            get => _owner;
            set => _owner = value;
        }

        public string ChangeListDescription
        {
            get => _changeListDescription;
            set => _changeListDescription = value;
        }
    }
}
