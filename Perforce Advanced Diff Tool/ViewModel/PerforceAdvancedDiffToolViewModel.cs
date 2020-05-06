using Submission_Tracker.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// All parsing logic of Submission tracker Window.
    /// </summary>
    class PerforceAdvancedDiffToolViewModel
    {
        private ITabView _tab;
        private Filters _filters;

        public PerforceAdvancedDiffToolViewModel(ITabView tab, Filters filter)
        {
            _tab = tab;
            _filters = filter;
        }

        /// <summary>
        /// Method to call perforce server and recieve source and destination data.
        /// </summary>
        /// <returns></returns>
        public string CreateSourceData()
        {
            //check if p1 and p2 have common parent, select workspace
            P4Execute executer = new P4Execute();
            P4ExecutingParameters exeParameters;
            string data = null;
            if (_tab.TabName == TabType.ChangelistView)
            {
                exeParameters = new P4ExecutingParameters("CMD.exe", "describe", null,
                    new List<string> { (_tab as ChangelistView).Changelist.ChangelistId.ToString() });
                data = executer.ExecuteCommand(exeParameters);
                if (ValidateReceivedData(data, new string[] { "@", "on" }))
                    (_tab as ChangelistView).SourceWorkSpace.WorkspacePath = data.Substring((data.IndexOf("@") + 1),
                    ((data.IndexOf("on") - 1) - (data.IndexOf("@") + 1)));
                else
                {
                    return null;
                }
            }
            else
                data = CreateDestinationData(_tab.Source);

            return data;
        }

        /// <summary>
        /// Parses source/destination data received from server.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<P4File> ParseLeftPartialRow(string data)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data))
                return null;
            List<P4File> allFiles = null;
            string one = null;
            string final = null;
            if (_tab.TabName == TabType.ChangelistView)
            {
                if (data.Contains("Moved files ..."))
                {
                    one = data.Remove(data.IndexOf("Moved files ..."));
                    final = one.Substring(one.IndexOf("Affected files ..."));
                }
                else if (data.Contains("Differences ..."))
                {
                    one = data.Remove(data.IndexOf("Differences ..."));
                    final = one.Substring(one.IndexOf("Affected files ..."));
                }
                else if (data.Contains("Affected files ..."))
                    final = data.Substring(data.IndexOf("Affected files ..."));
                else
                    final = data;

                string[] affectedFiles = null;
                if (final.Contains("..."))
                    affectedFiles = final.Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);
                else
                {
                    if (final.Contains("\\"))
                    {
                        affectedFiles = final.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        affectedFiles = CorrectFormat(affectedFiles, "\\");
                    }
                    else if (final.Contains("//"))
                    {
                        affectedFiles = final.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                        affectedFiles = CorrectFormat(affectedFiles, "//");
                    }
                }

                P4FileBuilder fileBuilder = new P4FileBuilder();
                allFiles = fileBuilder.BuildFiles(affectedFiles);
            }
            else
                allFiles = ParseRightPartialRow(data);

            return allFiles;
        }

        /// <summary>
        /// Corrects the format of files.
        /// </summary>
        /// <param name="affectedFiles"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private string[] CorrectFormat(string[] affectedFiles, string s)
        {
            for (int i = 0; i < affectedFiles.Length; i++)
            {
                string line = affectedFiles[i];
                line = s + line;
                //line = line.Remove(line.IndexOf("#") + 2).Trim();
                affectedFiles[i] = line;
            }

            return affectedFiles;
        }

        /// <summary>
        /// Parses destination data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<P4File> ParseRightPartialRow(string data)
        {
            List<P4File> allFiles;
            string[] affectedFiles = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            P4FileBuilder fileBuilder = new P4FileBuilder();
            allFiles = fileBuilder.BuildFiles(affectedFiles);
            return allFiles;
        }

        /// <summary>
        /// Calls perforce server to recieve destination data.
        /// </summary>
        /// <param name="p2"></param>
        /// <returns></returns>
        public string CreateDestinationData(P4Path p2)
        {
            P4Execute executer = new P4Execute();
            P4ExecutingParameters exePar;

            //Use below code if source and destination are not of depot but are local workspace locations.
            exePar = new P4ExecutingParameters("CMD.exe", "where", "\"" + p2.FilePath + "\"", new List<string> { "\"" + @p2.FilePath.ToString() + "\"" });
            //Parse output from where and then do p4 files.
            string data = executer.ExecuteCommand(exePar);
            string location = "";
            if (ValidateReceivedData(data, new[] { "..." }))
                location = GetDepotLocation(data, p2.FilePath);
            else
            {
                return null;
            }

            location = ParseLocation(p2);
            exePar = new P4ExecutingParameters("CMD.exe", "files", "\"" + p2.FilePath + "\"", new List<string> { "\"" + location + "\"" });
            data = executer.ExecuteCommand(exePar);
            if (ValidateReceivedData(data, new[] { "change" }))
                return data;
            return null;
        }

        /// <summary>
        /// Parses path values.
        /// </summary>
        /// <param name="p2"></param>
        /// <returns></returns>
        private string ParseLocation(P4Path p2)
        {
            if (p2.FilePath.EndsWith("/"))
                return string.Concat(p2.FilePath, "...");
            return p2.FilePath;
        }

        /// <summary>
        /// Gets Perforce depot location by parsing where data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string GetDepotLocation(string data, string filepath)
        {
            string[] locations = data.TrimEnd().Split(new string[] { "..." }, StringSplitOptions.RemoveEmptyEntries);

            if (locations.Length >= 2)
                return locations[1];
            else
                return locations[0];
        }

        /// <summary>
        /// Method sets the workspace in perforce server.
        /// </summary>
        /// <param name="source"></param>
        public bool SetCommonWorkspace(bool source = true)
        {
            P4Execute executer = new P4Execute();
            P4ExecutingParameters exePar = null;
            string workspaceNotSelectedmasg = "You might not have selected a common workspace" +
                                              " for source and destination." +
                                              "\n Please select a common workspace common to both" +
                                              " source and destination." +
                                              "\n Use the folder icon next to Source or Destination Entry bar" +
                                              " in the UI to make a selection." +
                                              "\n Please note selecting a workspace for either for source" +
                                              " or destination is sufficient.";
            if (source)
            {
                if (!string.IsNullOrEmpty((_tab as PathView).SourceWorkSpace.WorkspaceName))
                    exePar = new P4ExecutingParameters("CMD.EXE", "set", null, new List<string> { "P4CLIENT=", "\"" + (_tab as PathView).SourceWorkSpace.WorkspaceName + "\"" });
                else if (!string.IsNullOrEmpty((_tab as PathView).DestinationWorkSpace.WorkspaceName))
                    exePar = new P4ExecutingParameters("CMD.EXE", "set", null, new List<string> { "P4CLIENT=", "\"" + (_tab as PathView).DestinationWorkSpace.WorkspaceName + "\"" });
                else
                {
                    MessageBox.Show(workspaceNotSelectedmasg, "Workspace not selected!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty((_tab as ChangelistView).DestinationWorkSpace.WorkspaceName))
                    exePar = new P4ExecutingParameters("CMD.EXE", "set", null, new List<string> { "P4CLIENT=", "\"" + (_tab as ChangelistView).DestinationWorkSpace.WorkspaceName + "\"" });
                else if (!string.IsNullOrEmpty((_tab as ChangelistView).SourceWorkSpace.WorkspaceName))
                    exePar = new P4ExecutingParameters("CMD.EXE", "set", null, new List<string> { "P4CLIENT=", "\"" + (_tab as ChangelistView).SourceWorkSpace.WorkspaceName + "\"" });
                else
                {
                    MessageBox.Show(workspaceNotSelectedmasg, "Workspace not selected!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            executer.ExecuteCommand(exePar);
            string syncMsg = "Your ClientSpec is set to your selected Workspace." +
                             "\n It is required to sync the workspace before continuing further." +
                             "\n\n Please note syncing workspace is slow and would increase your wait time." +
                             "\n Optionally you can manually sync your selected common workspace in perforce before continuing further." +
                             "\n Or you can allow the tool to sync and wait for the whole operation to finish." +
                             "\n\n\n Warning: If you select not to sync and also don't manually sync your workspace," +
                             "\n there may be a failure to switch your ClientSpec!" +
                             "\n\n\n Do you want to continue syncing your selected workspace using this tool?";
            if (MessageBox
                .Show(syncMsg, "Time Consuming operation!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                });

                exePar = new P4ExecutingParameters("CMD.EXE", "sync", null, null, null);
                executer.ExecuteCommand(exePar);

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });

                Messages msg = new Messages("Perforce Sync Complete! Now starting processing.");
                msg.PublishMessage();
                return true;
            }
            else
            {
                MessageBox.Show("Workspace not synced!" +
                                " Please sync manually in perforce to complete your" +
                                " switching of workspace!", "Information!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return true;
            }
        }

        /// <summary>
        /// Async method to create hash table to store processed files data.
        /// </summary>
        /// <param name="filesData"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Dictionary<string, FileCluster>>> CreateHashTable(List<P4File> filesData/*, bool source*/)
        {
            if (filesData == null)
                return null;
            Dictionary<string, Dictionary<string, FileCluster>> hashTable = new Dictionary<string, Dictionary<string, FileCluster>>();

            IEnumerable<Task<P4FileTrunkRevisions>> filesDataEnumerable =
                from file in filesData select ProcessHistoryCompares(file);

            List<Task<P4FileTrunkRevisions>> fstatData = filesDataEnumerable.ToList();

            while (fstatData.Count > 0)
            {
                Task<P4FileTrunkRevisions> finishedTask = await Task.WhenAny(fstatData);
                fstatData.Remove(finishedTask);

                foreach (P4File file in finishedTask.Result.OlderRevisions)
                {
                    Dictionary<string, FileCluster> d = new Dictionary<string, FileCluster>();
                    if (!hashTable.ContainsKey(file.FileMetaData.Digest))
                    {
                        d.Add(file.FileMetaData.Digest, new FileCluster(file, finishedTask.Result.CurrentFile));
                        hashTable.Add(file.FileMetaData.Digest, d);
                    }
                }
            }

            return hashTable;
        }

        /// <summary>
        /// Async method to get meta data of files from perforce server.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        private async Task<P4FileTrunkRevisions> ProcessHistoryCompares(P4File fileData)
        {
            P4FileTrunkRevisions revs = null;
            if (fileData == null)
                return null;
            try
            {
                revs = new P4FileTrunkRevisions(fileData);
            }
            catch (Exception e)
            {
                return null;
            }

            P4Execute executer = new P4Execute();
            string filePathNoVersion = Path.Combine(fileData.CurrentPath.FilePath.ToString(), fileData.FileName);
            int highestVersion = Int32.Parse(fileData.FileMetaData.HeadRevision);
            for (int version = highestVersion; version > 0; version--)
            {
                P4ExecutingParameters exePar = new P4ExecutingParameters("CMD.EXE", "fstat", null, new List<string> { "-Osl", "\"" + filePathNoVersion.ToString() + "#" + version + "\"" });
                string data = executer.ExecuteCommand(exePar);
                if (ValidateReceivedData(data, new[] { "...", "depotFile" }))
                    revs = UpdateFileData(revs, data);
                if (version == highestVersion)
                    revs.CurrentFile = revs.OlderRevisions[0];

                if (!_filters.FindMatch)
                    break;
                //Put to dictionary
            }
            return revs != null ? revs : null;
        }

        /// <summary>
        /// Stores metadata in meta data object related to a p4File.
        /// </summary>
        /// <param name="revs"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private P4FileTrunkRevisions UpdateFileData(P4FileTrunkRevisions revs, string data)
        {
            P4File oldFile = new P4File();
            P4Path path = new P4Path();
            P4FileMetaData metaData = new P4FileMetaData();
            string[] dataArray = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i].Contains("depotFile"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("/"));
                    metaData.DepotFile = p.Trim();
                    path.FilePath = Path.GetDirectoryName(p);
                    oldFile.CurrentPath = path;
                    oldFile.FileName = Path.GetFileName(p);
                    continue;
                }
                if (dataArray[i].Contains("headAction"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headAction") + 10);
                    metaData.HeadAction = p.Trim();
                }
                if (dataArray[i].Contains("headType"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headType") + 8);
                    metaData.HeadType = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("headTime"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headTime") + 8);
                    metaData.HeadTime = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("headRev"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headRev") + 7);
                    metaData.HeadRevision = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("headChange"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headChange") + 10);
                    metaData.HeadChange = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("headModTime"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("headModTime") + 11);
                    metaData.HeadModTime = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("haveRev"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("haveRev") + 7);
                    metaData.HaveRevision = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("fileSize"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("fileSize") + 8);
                    metaData.Filesize = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("digest"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("digest") + 6);
                    metaData.Digest = p.Trim();
                    continue;
                }
                if (dataArray[i].Contains("isMapped"))
                {
                    string p = dataArray[i].Substring(dataArray[i].IndexOf("isMapped") + 8);
                    metaData.IsMapped = p.Trim();
                    continue;
                }

                metaData.ExtraHeadProperties = dataArray[i];
            }
            if (string.IsNullOrEmpty(metaData.Digest))
            {
                P4Execute executer = new P4Execute();
                P4ExecutingParameters exePar = new P4ExecutingParameters("CMD.EXE", "fstat", null, new List<string> { "-T digest -Ol ", "\"" + metaData.DepotFile + "#" + metaData.HeadRevision + "\"" });
                string digest = executer.ExecuteCommand(exePar);
                if (digest.Contains("digest"))
                {
                    string p = digest.Substring(digest.IndexOf("digest") + 6);
                    metaData.Digest = p.Trim();
                }
            }
            oldFile.CurrentPath = path;
            oldFile.FileMetaData = metaData;
            revs.OlderRevisions.Add(oldFile);
            return revs;
        }

        /// <summary>
        /// Validates received data from the called process.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="selectorString"></param>
        /// <returns></returns>
        private bool ValidateReceivedData(string data, string[] selectorString)
        {
            foreach (var str in selectorString)
            {
                if (!data.Contains(str))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
