using Submission_Tracker.Model;
using Submission_Tracker.View;
using Submission_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Cursors = System.Windows.Forms.Cursors;
using MessageBox = System.Windows.MessageBox;

namespace Submission_Tracker
{
    /// <summary>
    /// Interaction logic for SubmissionTrackerWindow.xaml
    /// This is the main window/UI of the application.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITabView _tab;
        private static object locker = new object();
        private Filters _filters = new Filters();
        Nullable<bool> fileresult;
        private SignInWindow _SignIn;
        public P4User _sessionUser = new P4User();
        public Workspace workspaceDisplay;
        public P4WorkSpace _selectedWorkspace = new P4WorkSpace();
        RowResultSubscriber<ResultItem> rowResultSubscriber;
        RowResultSubscriber<string> logMessageSubscriber;
        public event EventHandler _WorkSpaceEvent;

        public MainWindow()
        {
            InitializeComponent();

            _WorkSpaceEvent += (sender, args) => this._tab = (ITabView)sender;
            _SignIn = new SignInWindow(this);
            workspaceDisplay = new Workspace(this);
            var allFilters = this.FilterContainer.Children;
            AllFiles.IsChecked = true;
            FindMatch.IsChecked = true;
            

            DataGridTable.AutoGenerateColumns = false;
            //int rowNum = 1;
            DataGridTable.Items.Clear();
            DataGridTable.Columns.Clear();

            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Filename";
            c1.Binding = new System.Windows.Data.Binding("Filename1");
            c1.Width = 80;
            c1.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c1);

            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "FilePath";
            c2.Binding = new System.Windows.Data.Binding("FilePath1");
            c2.Width = 80;
            c2.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c2);

            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "CurrentVersion";
            c3.Binding = new System.Windows.Data.Binding("CurrentVersion1");
            c3.Width = 80;
            c3.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c3);

            DataGridTextColumn c4 = new DataGridTextColumn();
            c4.Header = "State";
            c4.Binding = new System.Windows.Data.Binding("State1");
            c4.Width = 80;
            c4.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c4);

            DataGridTextColumn c6 = new DataGridTextColumn();
            c6.Header = "Filename";
            c6.Binding = new System.Windows.Data.Binding("Filename2");
            c6.Width = 80;
            c6.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c6);

            DataGridTextColumn c7 = new DataGridTextColumn();
            c7.Header = "FilePath";
            c7.Binding = new System.Windows.Data.Binding("FilePath2");
            c7.Width = 80;
            c7.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c7);

            DataGridTextColumn c8 = new DataGridTextColumn();
            c8.Header = "CurrentVersion";
            c8.Binding = new System.Windows.Data.Binding("CurrentVersion2");
            c8.Width = 80;
            c8.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c8);

            DataGridTextColumn c9 = new DataGridTextColumn();
            c9.Header = "State";
            c9.Binding = new System.Windows.Data.Binding("State2");
            c9.Width = 80;
            c9.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c9);

            DataGridTextColumn c10 = new DataGridTextColumn();
            c10.Header = "Comment";
            c10.Binding = new System.Windows.Data.Binding("Comment");
            c10.Width = 80;
            c10.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c10);

            DataGridTextColumn c5 = new DataGridTextColumn();
            c5.Header = "Status";
            c5.Binding = new System.Windows.Data.Binding("Status");
            c5.Width = 80;
            c5.Visibility = Visibility.Visible;
            DataGridTable.Columns.Add(c5);

            Messages msg = new Messages(this);
        }

        /// <summary>
        /// Listener method to listen to message publisher.
        /// </summary>
        /// <param name="msg"></param>
        public void ListenToPublisher(SendMessage msg)
        {
            SendMessage sndmsg = msg;
            logMessageSubscriber = new RowResultSubscriber<string>(sndmsg);
            logMessageSubscriber.RowPublisher.RowPublisherHandler += RowResultPublisherEvent;
        }

        /// <summary>
        /// Method to capture change of filters as made by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiltersStatusChanged(object sender, RoutedEventArgs e)
        {
            string checkedName = ((System.Windows.Controls.RadioButton)sender).Name;
            if (checkedName == "AllFiles")
            {
                _filters.FilesDisplayType = FilesToDisplay.AllFiles;
            }
            else if (checkedName == "OnlyIntegratedFiles")
            {
                _filters.FilesDisplayType = FilesToDisplay.OnlyIntegratedFiles;
            }
            else
            {
                _filters.FilesDisplayType = FilesToDisplay.OnlyNonIntegratedFiles;
            }
        }

        /// <summary>
        /// Window Minimize event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeWindow_OnClick(object sender, EventArgs e)
        {

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                // label1.FontSize = 10;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
                // label1.FontSize = 8;
            }
        }

        /// <summary>
        /// Window Maximize event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeWindow_OnClick(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.MaxHeight = System.Windows.SystemParameters.WorkArea.Height;
                this.MaxWidth = System.Windows.SystemParameters.WorkArea.Width;
                this.WindowState = WindowState.Maximized;
                //   label1.FontSize = 10;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //  label1.FontSize = 8;
            }
        }

        /// <summary>
        /// Window Closing Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Event to handle Choose workspace button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseWorkspace_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button).Name.ToString() == chooseFile1.Name)
            {
                if (PathViewTab.IsSelected)
                    SelectWorkSpace(sender, _tab as PathView, "Source");
                else
                    SelectWorkSpace(sender, _tab as ChangelistView, "Source");
            }
            else
            {

                if (PathViewTab.IsSelected)
                    SelectWorkSpace(sender, _tab as PathView, "Destination");
                else
                    SelectWorkSpace(sender, _tab as ChangelistView, "Destination");
            }
        }

        /// <summary>
        /// Method to call get workspace information from perforce server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tabView"></param>
        /// <param name="field"></param>
        private void SelectWorkSpace(object sender, ITabView tabView, string field)
        {
            P4Execute executor = new P4Execute();
            P4ExecutingParameters exeParameters;
            string data= null;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            });

            if (!string.IsNullOrEmpty(_sessionUser.Username) && !string.IsNullOrWhiteSpace(_sessionUser.Username))
            {
                exeParameters = new P4ExecutingParameters("CMD.exe", "workspaces -u", null,
                new List<string> { _sessionUser.Username.Trim() });
                data = executor.ExecuteCommand(exeParameters);
            }
            else
            {
                exeParameters = new P4ExecutingParameters("CMD.exe", "user -o", null,
                null);
                data = executor.ExecuteCommand(exeParameters);
                if (!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data))
                {
                    data = data.Remove(data.LastIndexOf("Email:"));
                    data = data.Substring(data.LastIndexOf("User:") + 5).Trim();
                    exeParameters = new P4ExecutingParameters("CMD.exe", "workspaces -u", null,
                new List<string> { data });
                    data = executor.ExecuteCommand(exeParameters);
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Mouse.OverrideCursor = null;
                    });
                    return;
                }
            }
            if(workspaceDisplay.IsActive || workspaceDisplay.IsLoaded)
                workspaceDisplay.PopulateTable(data, sender, tabView, field, _WorkSpaceEvent);
            else
            {
                workspaceDisplay = new Workspace(this);
                workspaceDisplay.PopulateTable(data, sender, tabView, field, _WorkSpaceEvent);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        /// <summary>
        /// Event to handle Run button click.
        /// This event starts actual business logic of the tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            //check if user is logged in
            TabProcessor tabProcessor = new TabProcessor();
            _tab = tabProcessor.CreateTabData(_tab, SourcePath.Text, DestinationPath.Text, changelistBox.Text);
            if (tabProcessor._tabDataState.Equals("Source Empty") || tabProcessor._tabDataState.Equals("Destination Empty")
                || tabProcessor._tabDataState.Equals("Changelist ID Empty/Incorrect"))
                return;
            
            //Use this code if one need the tool to set the common workspace otherwise if
            //workspace is already chosen in perforce and you don't want the tool to do it, comment this para
            PerforceAdvancedDiffToolViewModel stProcessor = new PerforceAdvancedDiffToolViewModel(_tab, _filters);

            bool workspaceStatus = false;
            //Setting common WS
            if (_tab.TabName == TabType.ChangelistView)
            {
                workspaceStatus = stProcessor.SetCommonWorkspace(false);
            }
            else
            {
                workspaceStatus = stProcessor.SetCommonWorkspace();
            }
            if(!workspaceStatus)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            });

            Task<string> sdataCreator = Task.Factory.StartNew(() => stProcessor.CreateSourceData());
            Task<string> ddataCreator = Task.Factory.StartNew(() => stProcessor.CreateDestinationData(_tab.Destination));
            ProgressBar.Value += 1;
            await Task.WhenAll(sdataCreator, ddataCreator);
            Task<List<P4File>> sFiles = Task<List<P4File>>.Factory.StartNew(() => stProcessor.ParseLeftPartialRow(sdataCreator.Result));
            Task<List<P4File>> dFiles = Task<List<P4File>>.Factory.StartNew(() => stProcessor.ParseLeftPartialRow(ddataCreator.Result));
            ProgressBar.Value += 1;
            await Task.WhenAll(sFiles, dFiles);
            Task<Dictionary<string, Dictionary<string, FileCluster>>> leftHash = Task<Dictionary<string, Dictionary<string, FileCluster>>>.Factory.StartNew(() => stProcessor.CreateHashTable(sFiles.Result).Result);
            Task<Dictionary<string, Dictionary<string, FileCluster>>> rightHash = Task<Dictionary<string, Dictionary<string, FileCluster>>>.Factory.StartNew(() => stProcessor.CreateHashTable(dFiles.Result).Result);
            ProgressBar.Value += 1; 
            Task.WaitAll(leftHash, rightHash);
            #region old code
            //string sourceData = stProcessor.CreateSourceData();
            //List<P4File> sourceFiles = stProcessor.ParseLeftPartialRow(sourceData);
            //Dictionary<string, Dictionary<string, FileCluster>> leftHash = stProcessor.CreateHashTable(sourceFiles);
            // Get workspace for the destination location by asking user to select workspace for destination
            //string destData = stProcessor.CreateDestinationData(_tab.Destination);
            //List<P4File> destFiles = stProcessor.ParseRightPartialRow(destData);
            //Dictionary<string, Dictionary<string,FileCluster>> rightHash = stProcessor.CreateHashTable(destFiles);
            #endregion
            if (leftHash.Result != null && rightHash.Result != null)
                DataGridTable.Items.Clear();
            else if (leftHash.Result == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                return;
            }

            ResultIterator generator = new ResultIterator(leftHash.Result, rightHash.Result, leftHash.Result.Count, _filters);
            rowResultSubscriber = new RowResultSubscriber<ResultItem>(generator);
            rowResultSubscriber.RowPublisher.RowPublisherHandler += RowResultPublisherEvent;
            ProgressBar.Value += 1;
            Dispatcher.Invoke(() =>
            {
                PopulateTable(generator);
            }
            );

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });

            #region Old code
            //Task dataDisplay = Task.Factory.StartNew(() => );
            //PopulateTable(iterator);
            #endregion
        }

        /// <summary>
        /// Method to call the ResultIterator object's GenerateAndPublishRow method.
        /// </summary>
        /// <param name="generator"></param>
        private void PopulateTable(ResultIterator generator)
        {
            Task ptask = Task.Factory.StartNew(() => generator.GenerateAndPublishRow());

            #region old code
            //foreach (var result in iterator)
            //{
            //    if (result == null)
            //        continue;
            //    string comment = result.Comment.SourceVersion + " " + result.Comment.CommentState + " ";
            //    comment += result.Comment.DestinationVersion != null ? (Int32.Parse(result.Comment.DestinationVersion) > 0 ? result.Comment.DestinationVersion : "" ) : "";

            //    Dispatcher.Invoke(() =>
            //    {
            //        AddDisplayRowItem(result, comment);
            //    }
            //    );
            //    //Task dataDisplay = Task.Factory.StartNew(() => AddDisplayRowItem(result, comment));
            //    //DataGridTable.Items.Add(new ResultItem(result.SourceResult.Filename, result.SourceResult.FilePath, result.SourceResult.CurrentVersion,
            //    //                                        result.SourceResult.Status.ToString(), "", result.DestinationResult.Filename, result.DestinationResult.FilePath,
            //    //                                        result.DestinationResult.CurrentVersion, result.DestinationResult.Status.ToString(), comment));
            //}
            #endregion
        }

        /// <summary>
        /// Adds ResultItem to DataGridTable in main UI.
        /// </summary>
        /// <param name="item"></param>
        private void AddDisplayRowItem(ResultItem item)
        {
            DataGridTable.Items.Add(item);
        }

        /// <summary>
        /// Adds logging messages to Log area in Main UI.
        /// </summary>
        /// <param name="msg"></param>
        private void AddLogMessage(string msg)
        {
            MessageLog.AppendText(msg);
            MessageLog.ScrollToEnd();
        }

        /// <summary>
        /// Method to handle switching of tabs in Main UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PathViewTab.IsSelected)
            {
                _tab = new PathView();
                _tab.TabName = TabType.PathView;
                SourcePath.IsEnabled = true;
            }
            else if (ChangelistViewTab.IsSelected)
            {
                _tab = new ChangelistView();
                _tab.TabName = TabType.ChangelistView;
                SourcePath.IsEnabled = false;
            }
        }

        /// <summary>
        ///Event captured when Find Match button state changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindMatch_Click(object sender, RoutedEventArgs e)
        {
            _filters.FindMatch = FindMatch.IsChecked == true ? true : false;
        }

        /// <summary>
        /// Event handled when P4Login button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P4Login_Click(object sender, RoutedEventArgs e)
        {
            if(_SignIn.IsActive == false)
                _SignIn = new SignInWindow(this);
            _SignIn.Show();
        }

        /// <summary>
        /// Event handled when RowResult of type ResultItem publisher publishes. This method invokes
        /// a thread to add the result to main ui.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RowResultPublisherEvent(object sender, RowResultSender<ResultItem> e)
        {
            Dispatcher.Invoke(() =>
                {
                    AddDisplayRowItem(e.ResultRow);
                    ProgressBar.Value += 1;
                }
                );
        }

        /// <summary>
        /// Event handled when RowResult of type string publisher publishes. This method invokes
        /// a thread to add the result to main ui.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RowResultPublisherEvent(object sender, RowResultSender<string> e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                AddLogMessage(e.ResultRow);
                ProgressBar.Value += 1;
            })
                );
        }
        
        /// <summary>
        /// Event handled when P4Logout is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P4Logout_Click(object sender, RoutedEventArgs e)
        {
            P4Execute executer = new P4Execute();
            P4ExecutingParameters exePar = new P4ExecutingParameters("CMD.exe", "logout", null, null);
            string data = executer.ExecuteCommand(exePar);
            //Message
        }

        /// <summary>
        /// Event handled when About is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("GNU General Public License:\r\n\r\n" +
                            "This program is free software;" +
                            " \r\nyou can redistribute it and/or modify it " +
                            "\r\nunder the terms of the GNU General Public \r\n" +
                            "License as published by the Free Software \r\nFoundation; " +
                            "either version 1 of the License, \r\n" +
                            "or (at your option) any later version.\r\n\r\n" +
                            "This program is distributed in the hope that \r\n" +
                            "it will be useful, but WITHOUT ANY WARRANTY; \r\n" +
                            "without even the implied warranty of \r\n" +
                            "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  \r\n" +
                            "See the GNU General Public License for more details. \r\n" +
                            "\r\n Version 1.0 \r\n " +
                            "Designed and Developed by Siddhartha Ray",
                            "Licence information!", MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        /// <summary>
        /// Event Handled when progress bar value changes.
        /// Currently it's hidden and functionality is in-complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
