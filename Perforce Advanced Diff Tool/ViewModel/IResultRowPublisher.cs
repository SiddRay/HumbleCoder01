using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A contract defining a publisher's generic event handler and publishing method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResultRowPublisher<T>
    {
        event EventHandler<RowResultSender<T>> RowPublisherHandler;
        void PublishRowItem(T data);
    }
}
