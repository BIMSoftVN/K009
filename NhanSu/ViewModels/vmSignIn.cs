using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Mvvm;
using K009Libs.MVVM;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NhanSu.ViewModels
{
    public class vmSignIn : PropertyChangedBase
    {

        private string _Email = Properties.Settings.Default.Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }


        private string _Password = Properties.Settings.Default.Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }


        private SnackbarMessageQueue _MessageQueue = new SnackbarMessageQueue();
        public SnackbarMessageQueue MessageQueue
        {
            get
            {
                return _MessageQueue;
            }
            set
            {
                _MessageQueue = value;
                OnPropertyChanged();
            }
        }




        private ActionCommand cmd_SignIn;

        public ICommand Cmd_SignIn
        {
            get
            {
                if (cmd_SignIn == null)
                {
                    cmd_SignIn = new ActionCommand(PerformCmd_SignIn);
                }

                return cmd_SignIn;
            }
        }

        private async void PerformCmd_SignIn(object parameter)
        {
            try
            {
                var kq = await mUser.GetUserByEmail(Email, Password);

                if (kq.IsSuccess == true)
                {
                    Properties.Settings.Default.Email = Email;
                    Properties.Settings.Default.Password = Password;
                    Properties.Settings.Default.Save();

                    (parameter as Window).Hide();
                }

                MessageQueue?.Enqueue(kq.Message, null, null, null, false, true, TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                MessageQueue?.Enqueue(ex.Message, null, null, null, false, true, TimeSpan.FromSeconds(1));
            }
            
        }
    }
}
