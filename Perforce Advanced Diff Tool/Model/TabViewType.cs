using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// Enum class to define tab type to identify tabs uniquely.
    /// This Enum requires to be modified when new tabs are added.
    /// </summary>
        public enum TabType
        {
            PathView = 1,
            ChangelistView = 2
        }
}
