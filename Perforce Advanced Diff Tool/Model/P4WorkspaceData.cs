using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class to contain perforce workspace related data and attributes.
    /// This class should be extended and modified based on future needs.
    /// </summary>
    public class P4WorkspaceData
    {
        private string _dateCreated;
        private string _dateModified;
        private string _owner;
        private string _description;
        private string _extraFields;

        public P4WorkspaceData()
        { 
        }

        public P4WorkspaceData(string dateCreated, string dateModified, string owner, string description,
            string extraFields)
        {
            DateCreated = dateCreated;
            DateModified = dateModified;
            Owner = owner;
            Description = description;
            ExtraFields = extraFields;
        }

        public string DateCreated { get=> _dateCreated; set=> _dateCreated=value; }
        public string DateModified { get => _dateModified; set => _dateModified=value; }
        public string Owner { get => _owner; set => _owner=value; }
        public string Description { get => _description; set => _description=value; }
        public string ExtraFields { get => _extraFields; set => _extraFields=value; }
    }
}
