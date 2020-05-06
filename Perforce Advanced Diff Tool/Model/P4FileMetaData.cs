using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A Data Class containing P4 Filelog meta data as recieved from perforce server.
    /// </summary>
    public class P4FileMetaData
    {
        private string _headRevision;
        private string _headChange;
        private string _headTime;
        private string _haveRevision;
        private string _filesize;
        private string _digest;
        private string _isMapped;
        private string _headAction;
        private string _headType;
        private string _headModTime;
        private string _depotFile;
        private string _extraHeadProperties;

        public string IsMapped { get => _isMapped; set => _isMapped = value; }
        public string HeadAction { get => _headAction; set => _headAction = value; }
        public string HeadType { get => _headType; set => _headType = value; }
        public string HeadModTime { get => _headModTime; set => _headModTime = value; }
        public string DepotFile { get => _depotFile; set => _depotFile = value; }
        public string HeadRevision { get=>_headRevision; set=> _headRevision = value; }
        public string HeadChange { get => _headChange; set =>_headChange=value; }
        public string HeadTime { get =>_headTime; set=>_headTime=value; }
        public string HaveRevision { get=>_haveRevision; set =>_haveRevision=value; }
        public string Filesize { get =>_filesize; set =>_filesize=value; }
        public string Digest { get =>_digest; set =>_digest=value; }
        public string ExtraHeadProperties { get =>_extraHeadProperties; set =>_extraHeadProperties = value; }


        public P4FileMetaData(string headRevision)
        {
            string digits = string.Empty;
            int i=0;
            string hRevision = headRevision.Trim();
            while(i<headRevision.Length && char.IsDigit(hRevision[i]))
            {
                digits = string.Concat(hRevision[i++]);
            }
            _headRevision = digits ?? throw new ArgumentNullException(nameof(headRevision));
        }

        public P4FileMetaData(string headRevision, string headChange, string headTime, string haveRevision, string filesize, string digest, string extraHeadProperties)
        {
            _headRevision = headRevision;// ?? throw new ArgumentNullException(nameof(headRevision));
            _headChange = headChange;// ?? throw new ArgumentNullException(nameof(headChange));
            _headTime = headTime;// ?? throw new ArgumentNullException(nameof(headTime));
            _haveRevision = haveRevision;// ?? throw new ArgumentNullException(nameof(haveRevision));
            _filesize = filesize;// ?? throw new ArgumentNullException(nameof(filesize));
            _digest = digest;// ?? throw new ArgumentNullException(nameof(digest));
            _extraHeadProperties =
                extraHeadProperties; // ?? throw new ArgumentNullException(nameof(extraHeadProperties));
        }

        public P4FileMetaData()
        {
        }
    }
}
