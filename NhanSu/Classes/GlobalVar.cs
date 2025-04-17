using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using DevExpress.Utils.Paint;
using MaterialDesignThemes.Wpf;

namespace NhanSu.Classes
{
    public class GlobalVar
    {
        internal static string ConnString = @"Server=./;Database=iHRM;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        public static clAppInfo MainApp = new clAppInfo();
        public static clUser MainUser = new clUser();

        public static SnackbarMessageQueue MainSnackBarMess = new SnackbarMessageQueue();
    }

    public class clAppInfo
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }

        public clAppInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AppName = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)))?.Title;
            AppVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
        }
    }
}
