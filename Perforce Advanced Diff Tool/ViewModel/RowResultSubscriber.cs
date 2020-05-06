using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A generic subscriber class for subscribing to RowResult publisher.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RowResultSubscriber<T>
    {
        public IResultRowPublisher<T> RowPublisher { get; private set; }
        public RowResultSubscriber(IResultRowPublisher<T> publisher)
        {
            RowPublisher = publisher;
        }
    }
}