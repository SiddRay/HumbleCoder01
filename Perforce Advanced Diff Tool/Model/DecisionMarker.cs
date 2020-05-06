using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// Enum containing Decision maker tags for each row in the result.
    /// Helps to identify which direction merge should tak place.
    /// </summary>
    public enum DecisionMarker
    {
        MergeLeft = 0,
        MergeRight = 1,
        ResolveConflict = 2
    } 
    
}
