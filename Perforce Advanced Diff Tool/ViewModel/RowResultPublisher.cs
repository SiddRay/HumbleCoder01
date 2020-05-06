using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A generic publisher class for publishing generic ResultRow type result using EventArgs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RowResultSender<T> : EventArgs
    {
        public T ResultRow { get; private set; }
        public RowResultSender(T result)
        {
            ResultRow = result;
        }
    }
}
