using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A worker class to process files returned from perforce.
    /// This class defines methods to parse correct files in right format to be stored in hash.
    /// </summary>
    public class P4FileBuilder
    {
       /// <summary>
       /// Parses the output of a process to get Affected files.
       /// </summary>
       /// <param name="files"></param>
       /// <returns></returns>
        public List<P4File> BuildFiles(string[] files)
        {
            List<P4File> createdFiles = new List<P4File>();
            foreach (string file in files)
            {
                if (file.Contains("#") && !file.Contains(@"delete"))
                {
                    createdFiles.Add(CreateAFile(file));
                }
            }
            return (createdFiles);
        }

        /// <summary>
        /// Parses each processed file of BuildFiles in the right format for processing.
        /// </summary>
        /// <param name="fData"></param>
        /// <returns></returns>
        private P4File CreateAFile(string fData)
        {
            string[] fileData = fData.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            string fname = Path.GetFileName(fileData[0]);
            string fpath = Path.GetDirectoryName(fileData[0]);
            P4File p4File = new P4File(fname, fpath);
            P4FileMetaData metaData = new P4FileMetaData(fileData[1]);
            p4File.FileMetaData = metaData;
            return p4File;
        }
    }
}
