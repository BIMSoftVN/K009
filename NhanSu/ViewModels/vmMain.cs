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
using System.Windows.Input;

namespace NhanSu.ViewModels
{
    public class vmMain : PropertyChangedBase
    {

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
            win.ShowDialog();
        }
    }
}
