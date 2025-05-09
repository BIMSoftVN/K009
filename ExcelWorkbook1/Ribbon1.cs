using AutoCAD;
using ExcelWorkbook1.Models;
using ExcelWorkbook1.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            var xlApp = Globals.ThisWorkbook.Application;
            Worksheet ws = (Worksheet)Globals.ThisWorkbook.Sheets["Sheet2"];

            var aCadApp = Marshal.GetActiveObject("AutoCAD.Application") as AcadApplication;
            var aDoc = aCadApp.ActiveDocument;

            var model = aDoc.ModelSpace;

            var goc = new double[3] { 0, 0, 0 };

            var StartPoint = (double[])(aDoc.Utility.GetPoint(Type.Missing, "Chọn một điểm"));


            for (long i=2; i <= ws.UsedRange.Rows.Count; i++)
            {
                double b = (double)ws.Range["A" + i].Value;
                double h = (double)ws.Range["B" + i].Value;
                double Cover = (double)ws.Range["C" + i].Value;
                double SLT = (double)ws.Range["D" + i].Value;
                double DKT = (double)ws.Range["E" + i].Value;
                double SLD = (double)ws.Range["F" + i].Value;
                double DKD = (double)ws.Range["G" + i].Value;

                DrawSection(model, StartPoint, b, h, Cover, SLT, DKT, SLD, DKD);

                StartPoint[0] = StartPoint[0] + b + 300;
            }
        }


        private void DrawSection(AcadModelSpace model, double[] StartPoint, double b, double h,
            double Cover,
            double SLT, double DKT,
            double SLD, double DKD)
        {
            try
            {

                double[] pPointList = new double[]
                {
                    StartPoint[0],StartPoint[1],
                    StartPoint[0] + b,StartPoint[1],
                    StartPoint[0] + b,StartPoint[1] + h,
                    StartPoint[0],StartPoint[1] + h,
                };

                var Bolder = model.AddLightWeightPolyline(pPointList);
                Bolder.Closed = true;
                Bolder.Layer = "Concrete";

                var ThepDai = Bolder.Offset(-Cover)[0] as AcadLWPolyline;
                ThepDai.Layer = "ThepDai";

                double KCT = (b - 2 * Cover - DKT) / (SLT - 1);
                for (int i = 0; i < SLT; i++)
                {
                    var x = StartPoint[0] + Cover + DKT / 2 + i * KCT;
                    var y = StartPoint[1] + h - Cover - DKT / 2;
                    //var Circle = model.AddCircle(new double[] { x, y, 0 }, DKT / 2);
                    //Circle.Layer = "Thep";

                    var Rebar = model.InsertBlock(new double[] { x, y, 0 }, "Rebar", DKT, DKT, DKD, 0);
                    Rebar.Layer = "Thep";


                }

                double KCD = (b - 2 * Cover - DKD) / (SLD - 1);
                for (int i = 0; i < SLD; i++)
                {
                    var x = StartPoint[0] + Cover + DKD / 2 + i * KCD;
                    var y = StartPoint[1] + Cover + DKD / 2;
                    //var Circle = model.AddCircle(new double[] { x, y, 0 }, DKD / 2);
                    //Circle.Layer = "Thep";

                    var Rebar = model.InsertBlock(new double[] { x, y, 0 }, "Rebar", DKD, DKD, DKD, 0);
                    Rebar.Layer = "Thep";

                    var pArray = new double[]
                    {
                        x,y, 0,
                        StartPoint[0] + b/2, y + 100, 0,
                        x+b+200, y +100, 0,
                    };
                    model.AddLeader(pArray, null, AcLeaderType.acLineWithArrow);
                }

                double[] ExPoint1 = new double[3] { StartPoint[0], StartPoint[1] - 50, 0 };
                double[] ExPoint2 = new double[3] { StartPoint[0] + b, StartPoint[1] - 50, 0 };
                double[] TextLoc = new double[3] { StartPoint[0] + b / 2, StartPoint[1] - 200, 0 };

                var Dim1 = model.AddDimAligned(ExPoint1, ExPoint2, TextLoc);
                Dim1.StyleName = "DIM_01";

                ExPoint1 = new double[3] { StartPoint[0] - 50, StartPoint[1], 0 };
                ExPoint2 = new double[3] { StartPoint[0] - 50, StartPoint[1] + h, 0 };
                TextLoc = new double[3] { StartPoint[0] - 150, StartPoint[1] + h / 2, 0 };

                var Dim2 = model.AddDimAligned(ExPoint1, ExPoint2, TextLoc);
                Dim2.StyleName = "DIM_01";
            }
            catch
            {

            }
        }
    }
}
