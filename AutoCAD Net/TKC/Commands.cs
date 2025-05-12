using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoCAD_Net.TKC
{
    public class Commands
    {

        [CommandMethod("TKC")]
        public static void ThongKeCoc()
        {
            try
            {
                MessageBox.Show("Thông báo thống kê cọc");
            }
            catch
            {

            }
        }

    }
}
