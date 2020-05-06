using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A Data Container class
    /// This class decribes a changelist view of type ITabView. All data for this class should be present
    /// only in this class.
    /// </summary>
    public class ChangelistView : ITabView
    {
        private P4Path _source = new P4Path();
        private P4Path _destination = new P4Path();
        private TabType _tabNumber;
        private P4Changelist _changelist = new P4Changelist();
        private P4WorkSpace _sourceWorkSpace= new P4WorkSpace();
        private P4WorkSpace _destinationWorkSpace = new P4WorkSpace();
        private P4WorkSpace _commonWorkSpace = new P4WorkSpace();


        public ChangelistView()
        {
        }

        
        public P4Path Source { get => _source; set => _source = value; }
        public P4Path Destination { get => _destination; set => _destination = value; }
        public TabType TabName { get => _tabNumber; set => _tabNumber = value; }
        public P4Changelist Changelist { get => _changelist; set => _changelist = value; }
        public P4WorkSpace SourceWorkSpace { get => _sourceWorkSpace; set => _sourceWorkSpace = value; }
        public P4WorkSpace DestinationWorkSpace { get => _destinationWorkSpace; set => _destinationWorkSpace = value; }
        public P4WorkSpace CommonWorkSpace { get => _commonWorkSpace; set => _commonWorkSpace = value; }


    }
}
