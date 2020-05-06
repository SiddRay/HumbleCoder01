
namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class which defines perforce workspace wrapper.
    /// </summary>
    public class P4WorkSpace
    {
        private string _workspacePath;
        private string _workspaceName;
        private P4WorkspaceData _workspaceData = new P4WorkspaceData();

        public P4WorkSpace()
        { }

        public P4WorkSpace(string workspaceName, P4WorkspaceData workspaceData)
        {
            WorkspaceName = workspaceName;
            WorkspaceData = workspaceData;
        }

        public string WorkspacePath { get=> _workspacePath; set=> _workspacePath = value; }
        public P4WorkspaceData WorkspaceData { get=> _workspaceData; set=> _workspaceData=value; }
        public string WorkspaceName { get => _workspaceName; set => _workspaceName = value; }
    }
}