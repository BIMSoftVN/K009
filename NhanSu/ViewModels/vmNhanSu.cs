using ClosedXML.Excel;
using DevExpress.Data.XtraReports.Native;
using DevExpress.Xpf.Grid;
using K009Libs.MVVM;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Classes;
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

        private ActionCommand cmd_EditRow;

        public ICommand Cmd_EditRow
        {
            get
            {
                if (cmd_EditRow == null)
                {
                    cmd_EditRow = new ActionCommand(PerformCmd_EditRow);
                }

                return cmd_EditRow;
            }
        }

        private async void PerformCmd_EditRow(object par)
        {
            try
            {
                var user = (clUser)((par as RowEventArgs).Row);
                var kq = await mUser.SaveUserData(user);
                PerformCmd_LoadAll();
                Classes.GlobalVar.MainSnackBarMess?.Enqueue(kq.Message, null, null,null, false, true, TimeSpan.FromSeconds(2));

                //MessageBox.Show(kq.Message);
            }
            catch
            {

            }
        }

        private ActionCommand cmd_EditRow2;

        public ICommand Cmd_EditRow2
        {
            get
            {
                if (cmd_EditRow2 == null)
                {
                    cmd_EditRow2 = new ActionCommand(PerformCmd_EditRow2);
                }

                return cmd_EditRow2;
            }
        }

        private void PerformCmd_EditRow2()
        {
            try
            {
                if (UserItem!=null)
                {
                    var win = new Views.vAccInfo();
                    ((vmAccInfo)win.DataContext).User = UserItem;
                    win.ShowDialog();
                }    
            }
            catch
            {

            }
        }


        private ActionCommand cmd_AddUser;

        public ICommand Cmd_AddUser
        {
            get
            {
                if (cmd_AddUser == null)
                {
                    cmd_AddUser = new ActionCommand(PerformCmd_AddUser);
                }

                return cmd_AddUser;
            }
        }

        private void PerformCmd_AddUser()
        {
            try
            {
                var newUser = new clUser
                {
                    Name = "[ Vui lòng nhập ]",
                };

                UserList.Insert(0,newUser);
            }
            catch
            {
            }
        }

        private ActionCommand cmd_AddUserWindow;

        public ICommand Cmd_AddUserWindow
        {
            get
            {
                if (cmd_AddUserWindow == null)
                {
                    cmd_AddUserWindow = new ActionCommand(PerformCmd_AddUserWindow);
                }

                return cmd_AddUserWindow;
            }
        }

        private void PerformCmd_AddUserWindow()
        {
            var newUser = new clUser
            {
                Name = "[ Vui lòng nhập ]",
            };

            var win = new Views.vAccInfo();
            ((vmAccInfo)win.DataContext).User = newUser;
            win.ShowDialog();

        }

        private ActionCommand cmd_DeleteUsers;

        public ICommand Cmd_DeleteUsers
        {
            get
            {
                if (cmd_DeleteUsers == null)
                {
                    cmd_DeleteUsers = new ActionCommand(PerformCmd_DeleteUsers);
                }

                return cmd_DeleteUsers;
            }
        }

        private async void PerformCmd_DeleteUsers()
        {
            try
            {
                var mes = MessageBox.Show(messageBoxText: "Bạn có muốn xóa " + UserSelect.Count + " người dùng?",
                    caption: "Cảnh báo xóa", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Question);

                if (mes == MessageBoxResult.Yes)
                {
                    var kq = await mUser.DeleteUsers(UserSelect.ToList());
                    Classes.GlobalVar.MainSnackBarMess?.Enqueue(kq.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
                }     
            }
            catch
            {

            }
        }

        private ActionCommand cmd_ExcelExport;

        public ICommand Cmd_ExcelExport
        {
            get
            {
                if (cmd_ExcelExport == null)
                {
                    cmd_ExcelExport = new ActionCommand(PerformCmd_ExcelExport);
                }

                return cmd_ExcelExport;
            }
        }

        private void PerformCmd_ExcelExport()
        {
            try
            {
                var saveFile = new SaveFileDialog();
                saveFile.Filter = "Excel file|*.xlsx";
                saveFile.Title = "Xuất danh sách nhân viên";
                saveFile.FileName = "DanhSachNhanVien.xlsx";
                if (saveFile.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("DanhSach");
                        ws.Cell("A1").Value = "Họ và tên";
                        ws.Cell("B1").Value = "Email";
                        ws.Cell("C1").Value = "Điện thoại";
                        ws.Cell("D1").Value = "Địa chỉ";

                        long id = 1;
                        foreach (var user in UserList)
                        {
                            id++;
                            ws.Cell("A" + id).Value = user.Name;
                            ws.Cell("B" + id).Value = user.Email;
                            ws.Cell("C" + id).Value = user.PhoneNumber;
                            ws.Cell("D" + id).Value = user.Address;
                        }

                        workbook.SaveAs(saveFile.FileName);

                        Classes.GlobalVar.MainSnackBarMess?.Enqueue("Đã xuất dữ liệu", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }    
                }
            }
            catch
            {

            }
        }
    }
}
