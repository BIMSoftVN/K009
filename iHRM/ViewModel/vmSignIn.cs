using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iHRM.ViewModel
{
    internal class vmSignIn
    {
        public string Email { get; set; } = "anhdt@xuanmaicorp.vn";
        public string Password { get; set; } = "1234";


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

        private void PerformCmd_SignIn(object parameter)
        {
            if (this.Email == "anhdt")
            {
                (parameter as Window).Hide();
            }   
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }    
        }
    }
}
