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
using System.Windows.Shapes;

namespace iHRM.View
{
    /// <summary>
    /// Interaction logic for vSignIn.xaml
    /// </summary>
    public partial class vSignIn : Window
    {
        public vSignIn()
        {
            InitializeComponent();
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }    
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
