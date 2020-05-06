using Submission_Tracker.View;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Submission_Tracker.Model
{
    public class Messages
    {
        private string _message;
        private string _errors;
        private int _lineNumber;
        private static Window _currentWindow;
        private ViewModel.SendMessage _sendMessage;

        public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public Window CurrentWindow { get => _currentWindow; set => _currentWindow = value; }

        public Messages()
        {
        }

        public Messages(MainWindow window)
        {
            _currentWindow = window;
        }

        public Messages(string message, string err, MainWindow window)
        {
            Message = message;
            Error = err;
            _currentWindow = window;
            _sendMessage = new ViewModel.SendMessage();
        }

        public Messages(string msg, string err = null)
        {
            Message = msg;
            _sendMessage = new ViewModel.SendMessage();
        }

        public string Message { get => _message; set => _message = value; }
        public string Error { get => _errors; set => _errors = value; }


        // public void RegisterMessages()

        public void PublishMessage()
        {
            (_currentWindow as MainWindow).ListenToPublisher(_sendMessage);
            //Task sdataCreator = Task.Factory.StartNew(() => Publish.EnqueueMessage(this, _currentWindow as MainWindow, _sendMessage));
            Publish.EnqueueMessage(this, _currentWindow as MainWindow, _sendMessage);
        }

        //private void ProcessMessage(string msg, string msgtype)
        //{

        //}
    }
}
