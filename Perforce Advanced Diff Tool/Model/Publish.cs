using Submission_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// An Executor class Publish publishes messages in its queue.
    /// Implementation is similar to consumer producer model but currently made lazy since it's not multithreaded yet.
    /// Changes to this class should be made to follow a true consumer producer model.
    /// </summary>
    public static class Publish
    {
        public static Queue<Messages> MessagesQueue = new Queue<Messages>();
        private static int LineNumber =0;
        public static void PublishMessages(MainWindow CurrentWindow, SendMessage sm)
        {
            Object obj = new object();
            try
            {
                Monitor.Enter(obj);
                {
                    while(MessagesQueue.Count > 0)
                    {
                        Messages msg = MessagesQueue.Dequeue();
                        sm.PublishRowItem(LineNumber++ + ".  " + msg.Message + "\r\n");
                        //(CurrentWindow as MainWindow).MessageLog.Text = LineNumber++ + ".\t" + msg.Message + "\r\n";
                    }
                }
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }

        public static void EnqueueMessage(Messages msg, MainWindow wd, SendMessage sM)
        {
            Object obj = new object();
            try
            {
                Monitor.Enter(obj);
                {
                    MessagesQueue.Enqueue(msg);
                }
            }
            finally
            {
                Monitor.Exit(obj);
                PublishMessages(wd, sM);
            }
        }

    }
}
