using DevExpress.Data.XtraReports.Native;
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
    public class vmNhanSu : PropertyChangedBase
    {
        private ObservableRangeCollection<clUser> _UserList = new ObservableRangeCollection<clUser>();
        public ObservableRangeCollection<clUser> UserList
        {
            get
            {
                return _UserList;
            }
            set
            {
                _UserList = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<clUser> _UserSelect = new ObservableRangeCollection<clUser>();
        public ObservableRangeCollection<clUser> UserSelect
        {
            get
            {
                return _UserSelect;
            }
            set
            {
                _UserSelect = value;
                OnPropertyChanged();
            }
        }

        private clUser _UserItem = new clUser();
        public clUser UserItem
        {
            get
            {
                return _UserItem;
            }
            set
            {
                _UserItem = value;
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
                UserList.Clear();
                var kq = await mUser.GetAllUser();
                if (kq.IsSuccess)
                {
                    UserList.AddRange(kq.UserList);
                }
            }
            catch
            {

            }
        }
    }
}
