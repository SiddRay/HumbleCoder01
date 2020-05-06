using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// Generic Implementation class of IResultRowPublisher.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Publisher<T> : IResultRowPublisher<T>
    {
        public event EventHandler<RowResultSender<T>> RowPublisherHandler;

        private void OnDataPublisher(RowResultSender<T> args)
        {
            var handler = RowPublisherHandler;
            if (handler != null)
                handler(this, args);
        }

        public void PublishRowItem(T data)
        {
            throw new NotImplementedException();
        }
    }
}
