using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// The interface defines a Tab View contract of the Submission Tracker Tool.
    /// Defines the most basic fields which any tab should contain.
    /// However currently each tab is a one execution of the data it contains and is
    /// independent of other tabs.
    /// If a dependency is planned or required in the future between tabs, this interface
    /// would require to capture the same.
    /// </summary>
    public interface ITabView
    {
        P4Path Source
        {
            get;set;
        }


        P4Path Destination
        {
            get;
            set;
        }

        TabType TabName
        {
            get;
            set;
        }
    }
}
