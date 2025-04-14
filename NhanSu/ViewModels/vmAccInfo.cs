using K009Libs.MVVM;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Classes;
using NhanSu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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





        private ActionCommand cmd_SaveData;

        public ICommand Cmd_SaveData
        {
            get
            {
                if (cmd_SaveData == null)
                {
                    cmd_SaveData = new ActionCommand(PerformCmd_SaveData);
                }

                return cmd_SaveData;
            }
        }

        private async void PerformCmd_SaveData(object par)
        {
            try
            {
                var kq = await mUser.SaveUserData(User);
                if (kq.IsSuccess)
                {
                    //Đóng cửa sổ
                    (par as Window).Close();
                }
            }
            catch
            {

            }
        }

        private ActionCommand cmd_ChangePhoto;

        public ICommand Cmd_ChangePhoto
        {
            get
            {
                if (cmd_ChangePhoto == null)
                {
                    cmd_ChangePhoto = new ActionCommand(PerformCmd_ChangePhoto);
                }

                return cmd_ChangePhoto;
            }
        }

        private void PerformCmd_ChangePhoto()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Ảnh đại diện|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Chọn ảnh đại diện";

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    User.Photo = K009Libs.Imaging.Resize.ResizeImage(filePath,80,80);
                }

            }
            catch
            {

            }
        }
    }
}
