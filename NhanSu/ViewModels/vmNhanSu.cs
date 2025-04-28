using ClosedXML.Excel;
using DevExpress.Data.XtraReports.Native;
using DevExpress.Xpf.Grid;
using DocumentFormat.OpenXml.Office2010.Excel;
using K009Libs.MVVM;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Classes;
using NhanSu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static DevExpress.XtraEditors.Mask.MaskSettings;
using Excel = Microsoft.Office.Interop.Excel;

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

        private ActionCommand cmd_ExcelImport;

        public ICommand Cmd_ExcelImport
        {
            get
            {
                if (cmd_ExcelImport == null)
                {
                    cmd_ExcelImport = new ActionCommand(PerformCmd_ExcelImport);
                }

                return cmd_ExcelImport;
            }
        }

        private void PerformCmd_ExcelImport()
        {
            try
            {
                var openFile = new OpenFileDialog();
                openFile.Filter = "Excel file|*.xlsx";
                openFile.Title = "Nhập danh sách nhân viên";
                openFile.Multiselect = false;

                if (openFile.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook(openFile.FileName))
                    {
                        var ws = workbook.Worksheet("DanhSach");
                        var lr = ws.LastRowUsed().RowNumber();

                        var uList = new List<clUser>();
                        for (long id = 2; id <= lr; id++)
                        {
                            var user = new clUser();
                            user.Name = ws.Cell("A" + id).Value.ToString();
                            user.Email = ws.Cell("B" + id).Value.ToString();
                            user.PhoneNumber = ws.Cell("C" + id).Value.ToString();
                            user.Address = ws.Cell("D" + id).Value.ToString();
                            uList.Add(user);
                        }

                        UserList.AddRange(uList);

                        Classes.GlobalVar.MainSnackBarMess?.Enqueue("Đã xuất xong dữ liệu", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }    
                        
                }    
            }
            catch (Exception ex)
            {
                Classes.GlobalVar.MainSnackBarMess?.Enqueue(ex.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
            }
        }

        private ActionCommand cmd_ExcelExportCOM;

        public ICommand Cmd_ExcelExportCOM
        {
            get
            {
                if (cmd_ExcelExportCOM == null)
                {
                    cmd_ExcelExportCOM = new ActionCommand(PerformCmd_ExcelExportCOM);
                }

                return cmd_ExcelExportCOM;
            }
        }

        private void PerformCmd_ExcelExportCOM()
        {
            try
            {
                var saveFile = new SaveFileDialog();
                saveFile.Filter = "Excel file|*.xlsx";
                saveFile.Title = "Xuất danh sách nhân viên";
                saveFile.FileName = "DanhSachNhanVien.xlsx";
                if (saveFile.ShowDialog() == true)
                {
                    var ExceApp = new Excel.Application();
                    ExceApp.Visible = true;
                    ExceApp.DisplayAlerts = false;

                    var wb = ExceApp.Workbooks.Add();
                    var ws = (Worksheet)wb.Worksheets.Add();
                    ws.Name = "DanhSach";
                    ws.Cells[1, "A"] = "Họ và tên";
                    ws.Cells[1,"A"].Value = "Họ và tên";
                    ws.Cells[1, "B"].Value = "Email";
                    ws.Cells[1, "C"].Value = "Điện thoại";
                    ws.Cells[1, "D"].Value = "Địa chỉ";

                    long id = 1;
                    foreach (var user in UserList)
                    {
                        id++;
                        ws.Cells[id, "A"].Value = user.Name;
                        ws.Cells[id, "B"].Value = user.Email;
                        ws.Cells[id, "C"].Value = user.PhoneNumber;
                        ws.Cells[id, "D"].Value = user.Address;
                    }

                    wb.SaveAs(saveFile.FileName);

                    Classes.GlobalVar.MainSnackBarMess?.Enqueue("Đã xuất dữ liệu", null, null, null, false, true, TimeSpan.FromSeconds(2));

                    wb.Close();
                    ExceApp.DisplayAlerts = true;
                    ExceApp.Quit();


                    //using (var workbook = new XLWorkbook())
                    //{
                    //    var ws = workbook.Worksheets.Add("DanhSach");
                    //    ws.Cell("A1").Value = "Họ và tên";
                    //    ws.Cell("B1").Value = "Email";
                    //    ws.Cell("C1").Value = "Điện thoại";
                    //    ws.Cell("D1").Value = "Địa chỉ";


                    //    long id = 1;
                    //    foreach (var user in UserList)
                    //    {
                    //        id++;
                    //        ws.Cell("A" + id).Value = user.Name;
                    //        ws.Cell("B" + id).Value = user.Email;
                    //        ws.Cell("C" + id).Value = user.PhoneNumber;
                    //        ws.Cell("D" + id).Value = user.Address;
                    //    }

                    //    workbook.SaveAs(saveFile.FileName);

                    //    Classes.GlobalVar.MainSnackBarMess?.Enqueue("Đã xuất dữ liệu", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    //}
                }
            }
            catch
            {

            }
        }

        private ActionCommand cmd_ExcelExportActive;

        public ICommand Cmd_ExcelExportActive
        {
            get
            {
                if (cmd_ExcelExportActive == null)
                {
                    cmd_ExcelExportActive = new ActionCommand(PerformCmd_ExcelExportActive);
                }

                return cmd_ExcelExportActive;
            }
        }

        private void PerformCmd_ExcelExportActive()
        {
            try
            {
                var ExceApp = Marshal.GetActiveObject("Excel.Application") as Excel.Application;
                if (ExceApp != null) 
                {
                    ExceApp.Calculation = XlCalculation.xlCalculationManual;

                    var wb = ExceApp.ActiveWorkbook;
                    var ws = (Worksheet)ExceApp.ActiveSheet;
                    if (ws == null) 
                    {
                        ws = wb.Worksheets.Add();
                    }

                    ws.Cells[1, "A"] = "Họ và tên";
                    ws.Cells[1, "A"].Value = "Họ và tên";
                    ws.Cells[1, "B"].Value = "Email";
                    ws.Cells[1, "C"].Value = "Điện thoại";
                    ws.Cells[1, "D"].Value = "Địa chỉ";

                    var cId = new List<int>();


                    var lr = ws.Cells[ws.Rows.Count, 1].End[XlDirection.xlUp].Row();
                    long id = lr;
                    foreach (var user in UserList)
                    {
                        id++;
                        ws.Cells[id, cId[0]).Value = user.Name;
                        ws.Cells[id, "B"].Value = user.Email;
                        ws.Cells[id, "C"].Value = user.PhoneNumber;
                        ws.Cells[id, "D"].Value = user.Address;
                    }

                    ws.Columns["A:D"].AutoFit();

                    ws.Range["A1"].Formula = "=B1+C1";

                    ws.Calculate();

                    ExceApp.Calculation = XlCalculation.xlCalculationAutomatic;
                }
                else
                {
                    Classes.GlobalVar.MainSnackBarMess?.Enqueue("Hãy bật Excel", null, null, null, false, true, TimeSpan.FromSeconds(2));
                }    
            }
            catch
            {

            }
        }
    }
}
