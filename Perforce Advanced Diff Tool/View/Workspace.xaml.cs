using Submission_Tracker.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Submission_Tracker.View
{
    /// <summary>
    /// Interaction logic for Workspace.xaml
    /// This window displays the result of all the current user workspaces.
    /// </summary>
    public partial class Workspace : Window
    {
        MainWindow _mainWindow = null;
        public List<WorkspaceItems> workspaceItemList = new List<WorkspaceItems>();
        public P4WorkSpace _selectedWorkspace = new P4WorkSpace();
        object _caller;
        ITabView _tabView;
        string _field;
        private event EventHandler _WorkSpaceEvent;
        public Workspace(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// An interface method called from a consumer of this class which is capable of
        /// sending workspace data to be populated to Workspace window.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sender"></param>
        /// <param name="tabView"></param>
        /// <param name="field"></param>
        /// <param name="workspaceEvent"></param>
        public void PopulateTable(string data, object sender, ITabView tabView, string field, EventHandler workspaceEvent)
        {
            _WorkSpaceEvent = workspaceEvent;
            _caller = sender;
            _tabView = tabView;
            _field = field;
            ParseData(data, workspaceItemList);
            
            if(!workspaceItemList.Equals(null) && workspaceItemList.Count >=1)
            {
                WorkspaceTable.ItemsSource = workspaceItemList;
                this.Activate();
                this.Show();
            }
            else
            {
                MessageBox.Show("Unable to retrieve WorkSpaces! Try Again");
            }
        }

        /// <summary>
        /// Parses the workspace data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="workspaceItemList"></param>
        private void ParseData(string data, List<WorkspaceItems> workspaceItemList)
        {
            string[] tableData = null;
            if (data.Contains("Client"))
             tableData = data.Split(new string[] { "Client" }, StringSplitOptions.RemoveEmptyEntries);
            else 
              return;
            foreach(string row in tableData)
            {
                string wsn = row.Substring(0, row.IndexOf("root") - 1).Trim();
                string wsd = wsn.Substring(wsn.Length - 10);
                wsn = wsn.Remove(wsn.Length - 10);
                string wsr = row.Substring(row.IndexOf("root") + 4, (row.IndexOf("Created")-1) - (row.IndexOf("root") + 4)).Trim();
                workspaceItemList.Add(new WorkspaceItems { WorkspaceName = wsn.Trim(), WorkspaceRoot = wsr.Trim(), WorkspaceDate = wsd.Trim()});
            }
        }

        /// <summary>
        /// Internal data container class to map the parsed data to Work space Item attributes.
        /// </summary>
        public class WorkspaceItems
        {
            string workspaceName;
            string workspaceRoot;
            string workspaceDate;

            public string WorkspaceName { get => workspaceName; set => workspaceName = value; }
            public string WorkspaceRoot { get => workspaceRoot; set => workspaceRoot = value; }
            public string WorkspaceDate { get => workspaceDate; set => workspaceDate = value; }
        }

        /// <summary>
        /// Event to handle Cancel click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Event to handle Submit click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceItems item = null;
            foreach (WorkspaceItems items in WorkspaceTable.SelectedItems)
            {
                _selectedWorkspace.WorkspaceData = new P4WorkspaceData();
                _selectedWorkspace.WorkspaceName = items.WorkspaceName.Trim();
                _selectedWorkspace.WorkspacePath = items.WorkspaceRoot.Trim();
                _selectedWorkspace.WorkspaceData.DateCreated= items.WorkspaceDate.Trim();
                item = items;
                break;
            }
            string chosenWorkspace = _selectedWorkspace.WorkspaceName;
            if (!string.IsNullOrEmpty(chosenWorkspace) && !string.IsNullOrWhiteSpace(chosenWorkspace))
            {
                _mainWindow._selectedWorkspace.WorkspaceName = item.WorkspaceName;
                _mainWindow._selectedWorkspace.WorkspacePath = item.WorkspaceRoot;
                _mainWindow._selectedWorkspace.WorkspaceData.DateCreated = item.WorkspaceDate;
                if (_field == "Source")
                {
                    //SourcePath.Text = chosenWorkspace;
                    if (_tabView is PathView)
                    {
                        (_tabView as PathView).SourceWorkSpace.WorkspaceName = chosenWorkspace;
                        (_tabView as PathView).SourceWorkSpace.WorkspacePath = item.WorkspaceRoot; 
                        (_tabView as PathView).SourceWorkSpace.WorkspaceData.DateCreated = item.WorkspaceDate;

                    }
                    else
                    {
                        (_tabView as ChangelistView).SourceWorkSpace.WorkspaceName = chosenWorkspace;
                        (_tabView as ChangelistView).SourceWorkSpace.WorkspacePath = item.WorkspaceRoot;
                        (_tabView as ChangelistView).SourceWorkSpace.WorkspaceData.DateCreated = item.WorkspaceDate;

                    }
                }
                else if (_field == "Destination")
                {
                    //DestinationPath.Text = chosenWorkspace;
                    if (_tabView is PathView)
                    {
                        (_tabView as PathView).DestinationWorkSpace.WorkspaceName = chosenWorkspace;
                        (_tabView as PathView).DestinationWorkSpace.WorkspacePath = item.WorkspaceRoot;
                        (_tabView as PathView).DestinationWorkSpace.WorkspaceData.DateCreated = item.WorkspaceDate;
                    }
                    else
                    {
                        (_tabView as ChangelistView).DestinationWorkSpace.WorkspaceName = chosenWorkspace;
                        (_tabView as ChangelistView).DestinationWorkSpace.WorkspacePath = item.WorkspaceRoot;
                        (_tabView as ChangelistView).DestinationWorkSpace.WorkspaceData.DateCreated = item.WorkspaceDate;
                    }
                }

                _WorkSpaceEvent.Invoke(_tabView, EventArgs.Empty);
            }
            else
            {
                //Workspace selection error
            }
            this.Hide();
        }
    }
}
