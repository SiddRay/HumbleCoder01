using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.ViewModel
{
    /// <summary>
    /// A event handler class to publish ResultRow of type string.
    /// </summary>
    public class SendMessage : IResultRowPublisher<string>
    {
        public event EventHandler<RowResultSender<string>> RowPublisherHandler;

        private void OnRowPublish(RowResultSender<string> args)
        {
            var handler = RowPublisherHandler;
            if (handler != null)
                handler(this, args);
        }
        public void PublishRowItem(string data)
        {
            RowResultSender<string> row = (RowResultSender<string>)Activator.CreateInstance(typeof(RowResultSender<string>), new object[] { data });
            OnRowPublish(row);
        }
    }
}
