using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// Tab processor process the data present in a tab, validates and modifies in correct format
    /// before perforce calls.
    /// </summary>
    public class TabProcessor
    {
        public ITabView _tab;
        public string _tabDataState;
        private Messages msg;
        public TabProcessor()
        {
        }

        public ITabView CreateTabData(ITabView tab, string src, string dest, string change)
        {
            if (tab.TabName == TabType.PathView)
            {
                _tab = tab;
                _tab.TabName = TabType.PathView;
                PopulateSourceAndDestination(src,dest);
            }

            else if (tab.TabName == TabType.ChangelistView)
            {
                _tab = tab;
                _tab.TabName = TabType.ChangelistView;
                PopulateSourceAndDestination(src,dest, change);
            }
            return _tab;
        }

        private bool PopulateSourceAndDestination(string src, string dest, string change=null)
        {
            //To do:
            //Create Enums for the return message!

            if (_tab.TabName == TabType.PathView)
            {
                if (ValidatePath(src))
                {
                    src = NormalizePath(src);
                    _tab.Source.FilePath = src;
                }
                else
                {
                    //MessageBox.Show("Source Empty! Enter Perforce Source Path");
                    //throw new ArgumentNullException(src, "invalid source path");
                    msg = new Messages("Source path invalid! Enter a valid source path to continue.");
                    msg.PublishMessage();
                    _tabDataState = "Source Empty";
                    return false;
                }
            }

            if (ValidatePath(dest))
            {
                dest = NormalizePath(dest);
                _tab.Destination.FilePath = dest;
            }
            else
            {
                //MessageBox.Show("Destination Empty! Enter Perforce Destination Path");
                //throw new ArgumentNullException(src, "invalid source path");
                msg = new Messages("Destination path invalid! Enter a valid destination path to continue.");
                msg.PublishMessage();
                _tabDataState = "Destination Empty";
                return false;
            }

            if (_tab.TabName == TabType.ChangelistView)
            {
                if (ValidateChangelistID(change))
                    (_tab as ChangelistView).Changelist.ChangelistId = Int32.Parse(change);
                else
                {
                    //MessageBox.Show("Changelist ID Empty/Incorrect! Enter valid changelist ID");
                    msg = new Messages("Changelist ID invalid! Enter valid Changelist ID.");
                    msg.PublishMessage();
                    _tabDataState = "Changelist ID Empty/Incorrect";
                    return false;
                }
            }

            _tabDataState = "Data Validation Successful";
            return true;
        }

        private string NormalizePath(string src)
        {
            if (src.EndsWith("..."))
                return src;
            else if (src.EndsWith("/") || src.EndsWith("\\") || src.EndsWith("\\\\") || src.EndsWith("//"))
                return src.Trim() + "...";
            else
            {
                if (src.Contains("\\"))
                    return src.Trim() + "\\" + "...";
                else if (src.Contains("/"))
                    return src.Trim() + "/" + "...";
                else if (src.Contains("\\\\"))
                    return src.Trim() + "\\\\" + "...";
                else if (src.Contains("//"))
                    return src.Trim() + "//" + "...";
            }
            if(src.Contains("."))
            {
                string file = Path.GetExtension(src);
                if (!string.IsNullOrEmpty(file) && !string.IsNullOrWhiteSpace(file))
                    return src.Trim();
                else
                {
                    return src.Trim() + "\\" + "...";
                }
            }
            return src.Trim() + "\\" + "...";
        }

        public bool ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return false;
            return true;
        }

        public bool ValidateChangelistID(string changeID)
        {
            int result = 0;
            if (string.IsNullOrEmpty(changeID) || string.IsNullOrWhiteSpace(changeID))
                return false;
            Int32.TryParse(changeID, out result);
            if (result <= 0)
                return false;
            return true;
        }
    }
}
