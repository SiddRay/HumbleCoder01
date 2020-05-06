using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// File state enum is used to identify if the file is latest w.r.t the compared file in the trunk or not.
    /// If not latest, it says the state of file is in conflict.
    /// </summary>
    public enum FileState
    {
        Latest = 0,
        Conflict =1
    } 
}
