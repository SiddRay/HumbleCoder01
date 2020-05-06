using Submission_Tracker.Model;
using Submission_Tracker.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace Submission_Tracker.View
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// Sign-in window is used to make user login to perforce.
    /// </summary>
    public partial class SignInWindow : Window
    {
        private MainWindow _mainWindow;

        public SignInWindow(MainWindow window)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _mainWindow = window;
            this.Closing += OnWindowClosingEvent;
        }

       
        public SignInWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _mainWindow = new MainWindow();
            this.Closing += OnWindowClosingEvent;
        }

        /// <summary>
        /// Event for the Cancel button in SingInWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Messages msg = new Messages(_mainWindow);
            _mainWindow.Show();
            _mainWindow.Activate();
        }

        /// <summary>
        /// Closing Event of the current window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowClosingEvent(object sender, CancelEventArgs e)
        {
            if (_mainWindow.IsActive || this.IsLoaded)
            {
                _mainWindow.Show();
                _mainWindow.Activate();
            }
        }

        /// <summary>
        /// Event for Submit button in the SingInWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Message.Visibility = Visibility.Hidden;
            Message.Content = "";
            string username = UserName.Text.ToLower().Trim();
            string password = Password.Password;
            if(ValidateUsernameAndPassword(username,password))
            {
                P4Execute executer = new P4Execute();
                P4ExecutingParameters exePar = new P4ExecutingParameters("CMD.exe", "set P4USER=", null, new System.Collections.Generic.List<string> { username });
                string data = executer.ExecuteCommand(exePar);
                exePar = new P4ExecutingParameters("CMD.exe", "login", null, null, $"echo {password}|");
                data = executer.ExecuteCommand(exePar);
                password = null;
                if (data.Contains("User"))
                    data = !System.String.IsNullOrEmpty(data) ? data.Substring(data.IndexOf("User")) : "";
                else
                {
                    Message.Visibility = Visibility.Visible;
                    Message.Content = "UserName or Password incorrect. Try Again!";
                    return;
                }
                _mainWindow._sessionUser.Username = username;
                Messages msg = new Messages(data.Trim(), "", _mainWindow);
                msg.PublishMessage();
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrWhiteSpace(username))
                    _mainWindow.mast.Content += $" -  (P4V user: {username}* logged-in)";
                this.Close();
                _mainWindow.Show();
                _mainWindow.Activate();
            }
            else
            {
                MessageBox.Show("User Name or Password cannot be blank! \n\nPlease Enter the same or click Cancel.");
            }
        }

        /// <summary>
        /// Validator method to validate presence of username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;
            else
            {
                return true;
            }
        }
    }
}
