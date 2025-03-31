using K009Libs.MVVM;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Classes;
using NhanSu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NhanSu.ViewModels
{
    public class vmAccInfo : PropertyChangedBase
    {
        private clUser _User = new clUser();
        public clUser User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                OnPropertyChanged();
            }
        }




        private ActionCommand cmd_LoadAll;

        public ICommand Cmd_LoadAll
        {
            get
            {
                if (cmd_LoadAll == null)
                {
                    cmd_LoadAll = new ActionCommand(PerformCmd_LoadAll);
                }

                return cmd_LoadAll;
            }
        }

        private async void PerformCmd_LoadAll()
        {
            try
            {
               var kq = await mUser.GetUserById(Classes.GlobalVar.MainUser.Id);
               if (kq.IsSuccess)
                {
                    User = kq.User;
                }    
            }
            catch
            {

            }
        }
    }
}
