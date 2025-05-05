using ExcelWorkbook1.Models;
using ExcelWorkbook1.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
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

                var xRange = (Range)ws.Range["A2"].Resize[arr.GetLength(0), 1];
                var yRange = (Range)ws.Range["B2"].Resize[arr.GetLength(0), 1];

                var charts = (ChartObjects)ws.ChartObjects();
                if (charts.Count >0)
                {
                    var chartObject = (ChartObject)charts.Item("BieuDo1");
                    var chart = chartObject.Chart;

                    var seri = (Series)chart.SeriesCollection(1);
                    seri.XValues = xRange;
                    seri.Values = yRange;


                    var vtBt = ws.Range["F1:K16"];
                    chartObject.Left = (double)vtBt.Left;
                    chartObject.Top = (double)vtBt.Top;
                    chartObject.Width = (double)vtBt.Width;
                    chartObject.Height = (double)vtBt.Height;
                }    
               

                //id = 1;
                //foreach (var w in wList)
                //{
                //    id++;
                //    Range wRng = (Range)ws.Range["E" + id];
                //    float wLeft = (float)wRng.Left;
                //    float wTop = (float)wRng.Top;
                //    float wWidth = (float)wRng.Width;
                //    float wHeight = (float)wRng.Height;

                //    var TestLink = @"C:\Users\kysudo\Desktop\Test.jpg";

                //    var Pic = ws.Shapes.AddPicture(TestLink,
                //        Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue,
                //        wLeft, wTop, wWidth, wHeight);
                //}



                //xlApp.Calculation = XlCalculation.xlCalculationAutomatic;
                //xlApp.ScreenUpdating = true;
            }
            catch (Exception ea)
            {

            }
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            var win = new vMain();
            win.Show();

        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
