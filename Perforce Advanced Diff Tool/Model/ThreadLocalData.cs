using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Submission_Tracker.Model;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// Currently Un-used.
    /// This class should be used to store thread local data for processing multiple threads
    /// which do similar tasks.
    /// </summary>
    public class ThreadLocalData
    {
        Dictionary<string, Dictionary<string, FileCluster>> hashTable = new Dictionary<string, Dictionary<string, FileCluster>>();

    }
}
