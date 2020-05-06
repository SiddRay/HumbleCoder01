using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class representing an implementation of ITabView interface.
    /// This class can contain all the data for 1st tab in Main Window.
    /// Currently all the tabs defined as independent of eachother in functioning which is the basic idea of having
    /// current interface defined the way it is.
    /// </summary>
    public class PathView : ITabView
    {
        private P4Path _source = new P4Path();
        private P4Path _destination = new P4Path();
        private TabType _tabName;
        private P4WorkSpace _sourceWorkSpace = new P4WorkSpace();
        private P4WorkSpace _destinationWorkSpace = new P4WorkSpace();
        private P4WorkSpace _commonWorkSpace = new P4WorkSpace();

        public PathView()
        {
        }

        public PathView(string source, string destination, TabType tabName)
        {
            Source.FilePath = source;
            Destination.FilePath = destination;
            TabName = tabName;
        }

        public P4Path Source
        {
            get => _source;
            set => _source = value;
        }

        public P4Path Destination
        {
            get => _destination;
            set => _destination = value;
        }

        public TabType TabName
        {
            get => _tabName;
            set => _tabName = value;
        }

        public P4WorkSpace SourceWorkSpace
        {
            get => _sourceWorkSpace;
            set => _sourceWorkSpace = value;
        }

        public P4WorkSpace DestinationWorkSpace
        {
            get => _destinationWorkSpace;
            set => _destinationWorkSpace = value;
        }

        public P4WorkSpace CommonWorkSpace { get => _commonWorkSpace; set => _commonWorkSpace = value; }
    }
}
