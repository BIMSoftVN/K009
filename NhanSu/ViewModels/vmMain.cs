using K009Libs.MVVM;
using Microsoft.Xaml.Behaviors.Core;
using NhanSu.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NhanSu.ViewModels
{
    public class vmMain : PropertyChangedBase
    {
        private Page _MFContent;
        public Page MFContent
        {
            get { return _MFContent; }
            set
            {
                _MFContent = value;
                OnPropertyChanged();
            }
        }




        private ActionCommand cmd_DangXuat;

        public ICommand Cmd_DangXuat
        {
            get
            {
                if (cmd_DangXuat == null)
                {
                    cmd_DangXuat = new ActionCommand(PerformCmd_DangXuat);
                }

                return cmd_DangXuat;
            }
        }

        private void PerformCmd_DangXuat()
        {
            Properties.Settings.Default.Password = null;
            Properties.Settings.Default.Save();

            Process.Start(Assembly.GetExecutingAssembly().Location);
            Environment.Exit(0);
        }

        private ActionCommand cmd_OpenAccInfo;

        public ICommand Cmd_OpenAccInfo
        {
            get
            {
                if (cmd_OpenAccInfo == null)
                {
                    cmd_OpenAccInfo = new ActionCommand(PerformCmd_OpenAccInfo);
                }

                return cmd_OpenAccInfo;
            }
        }

        private void PerformCmd_OpenAccInfo()
        {
            var win = new vAccInfo();
            ((vmAccInfo)win.DataContext).User = Classes.GlobalVar.MainUser;
            win.ShowDialog();
        }





        private ActionCommand cmd_OpenPage;
        public ICommand Cmd_OpenPage
        {
            get
            {
                if (cmd_OpenPage == null)
                {
                    cmd_OpenPage = new ActionCommand(PerformCmd_OpenPage);
                }

                return cmd_OpenPage;
            }
        }

        private void PerformCmd_OpenPage(object parameter)
        {
            string pageName = parameter as string;
            switch (pageName) 
            {
                case "pQuanTri":
                    {
                        MFContent = new pQuanTri();
                    }
                    break;

                case "pNhanSu":
                    {
                        MFContent = new pNhanSu();
                    }
                    break;

                default:
                    break;
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

        private void PerformCmd_LoadAll()
        {
            MFContent = new pNhanSu();
        }
    }
}
