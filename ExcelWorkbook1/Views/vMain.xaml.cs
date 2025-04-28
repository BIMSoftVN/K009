using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelWorkbook1.Views
{
    /// <summary>
    /// Interaction logic for vMain.xaml
    /// </summary>
    public partial class vMain : Window
    {
        public vMain()
        {
            InitializeComponent();
        }



        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Globals.ThisWorkbook.Application.Interactive = true;
            }
            catch
            {

            }
            // Vô hiệu hóa đầu vào Excel khi chuột di chuyển vào cửa sổ
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Globals.ThisWorkbook.Application.Interactive = false;
            }
            catch
            {

            }
        }


        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Globals.ThisWorkbook.Application.Interactive = false;
            }
            catch
            {

            }
        }
    }
}
