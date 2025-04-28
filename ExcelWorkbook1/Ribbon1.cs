using ExcelWorkbook1.Models;
using ExcelWorkbook1.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelWorkbook1
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            
        }

        private async void button1_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var ViDo = double.Parse(this.eb_Lat.Text);
                var KinhDo = double.Parse(this.eb_Lon.Text);

                var wList = await mWeather.LoadInfoByLocation(ViDo, KinhDo);

                var xlApp = Globals.ThisWorkbook.Application;
                Worksheet ws = (Worksheet)Globals.ThisWorkbook.Sheets["Sheet1"];

                //xlApp.Calculation = XlCalculation.xlCalculationManual;
                //xlApp.ScreenUpdating = false;

                long id = -1;
                dynamic[,] arr = new dynamic[wList.Count,5];

                foreach (var w in wList)
                {
                    id++;
                    arr[id, 0] = w.wDate;
                    arr[id, 1] = w.wNhietDo;
                    arr[id, 2] = w.wGio;
                    arr[id, 3] = w.wMoTa;
                    arr[id, 4] = w.wImage;
                }

                ws.Range["A2"].Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;


                //xlApp.Calculation = XlCalculation.xlCalculationAutomatic;
                //xlApp.ScreenUpdating = true;
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            var win = new vMain();
            win.Show();

        }
    }
}
