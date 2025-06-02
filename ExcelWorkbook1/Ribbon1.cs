using AutoCAD;
using ETABSv1;
using ExcelWorkbook1.Models;
using ExcelWorkbook1.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

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

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                //myETABSObject = myHelper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
                //ret = myETABSObject.ApplicationStart();
                //string ModelPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ETABS_Model.edb");
                //ret = mySapModel.InitializeNewModel();
                //ret = mySapModel.File.NewSteelDeck(4, 12, 12, 4, 4, 24, 24);
                ////ret = mySapModel.File.Save(ModelPath);
                //ret = mySapModel.Analyze.RunAnalysis();


                cSapModel mySapModel = myETABSObject.SapModel;


                int ret = -1;

                ret = mySapModel.SetPresentUnits(eUnits.kN_m_C);


                int NumberStories = -1;
                string[] StoryNames = new string[] {};
                double[] StoryHeights = new double[] { };
                double[] StoryElevations = new double[] { };
                bool[] IsMasterStory = new bool[] { };
                string[] SimilarToStory = new string[] { };
                bool[] SpliceAbove = new bool[] { };
                double[] SpliceHeight = new double[] { };


                ret = mySapModel.Story.GetStories(ref NumberStories, ref StoryNames, ref StoryHeights, ref StoryElevations,
                    ref IsMasterStory, ref SimilarToStory, ref SpliceAbove, ref SpliceHeight);




                if (ret == 0)
                {
                    if (NumberStories > 0)
                    {
                        var xlApp = Globals.ThisWorkbook.Application;
                        Range acell = (Range)xlApp.ActiveCell;
                        Worksheet ws = (Worksheet)Globals.ThisWorkbook.ActiveSheet;

                        long id = -1;
                        dynamic[,] arr = new dynamic[NumberStories, 3];


                        for (int i = 0; i < NumberStories; i++)
                        {
                            id++;
                            arr[id, 0] = StoryNames[i];
                            arr[id, 1] = StoryElevations[i];
                            arr[id, 2] = StoryHeights[i];
                        }

                        acell.Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;
                    }
                    else
                    {
                        MessageBox.Show("Không có tầng nào trong mô hình.");
                    }
                }

                mySapModel = null;
                myETABSObject = null;

            }
            catch (Exception Ex)
            {

            }
        }

        private void button5_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                cSapModel mySapModel = myETABSObject.SapModel;
                int ret = -1;

                ret = mySapModel.SetPresentUnits(eUnits.kN_m_C);

                ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();

                ret = mySapModel.Results.Setup.SetComboSelectedForOutput("Comb1", true);
                ret = mySapModel.Results.Setup.SetComboSelectedForOutput("Comb2", true);
                ret = mySapModel.Results.Setup.SetComboSelectedForOutput("Comb3", false);

                int NumberNames = -1;
                string[] MyName = new string[] { };
                string[] PropName = new string[] { };
                string[] StoryName = new string[] { };
                string[] StoryName_F = new string[] { };
                string[] PointName1 = new string[] { };
                string[] PointName2 = new string[] { };
                double[] Point1X = new double[] { };
                double[] Point1Y = new double[] { };
                double[] Point1Z = new double[] { };
                double[] Point2X = new double[] { };
                double[] Point2Y = new double[] { };
                double[] Point2Z = new double[] { };
                double[] Angle = new double[] { };
                double[] Offset1X = new double[] { };
                double[] Offset2X = new double[] { };
                double[] Offset1Y = new double[] { };
                double[] Offset2Y = new double[] { };
                double[] Offset1Z = new double[] { };
                double[] Offset2Z = new double[] { };
                int[] CardinalPoint = new int[] { };
                string csys = "Global";

                ret = mySapModel.FrameObj.GetAllFrames(ref NumberNames, ref MyName,
                                            ref PropName,
                                            ref StoryName,
                                            ref PointName1, ref PointName2, ref Point1X, ref Point1Y, ref Point1Z,
                                            ref Point2X, ref Point2Y, ref Point2Z,
                                            ref Angle,
                                            ref Offset1X, ref Offset2X, ref Offset1Y, ref Offset2Y, ref Offset1Z,
                                            ref Offset2Z,
                                            ref CardinalPoint, csys);

                if (ret == 0 && NumberNames>0)
                {
                    var xlApp = Globals.ThisWorkbook.Application;
                    Range acell = (Range)xlApp.ActiveCell;
                    long id = -1;
                    


                    foreach (var name in MyName)
                    {
                        var huong = eFrameDesignOrientation.Null;
                        ret = mySapModel.FrameObj.GetDesignOrientation(name, ref huong);
                        if (ret == 0 && huong == eFrameDesignOrientation.Beam) 
                        {
                            int NumberResults = -1;
                            string[] Obj = new string[] { };
                            double[] ObjSta = new double[] { };
                            string[] Elm = new string[] { };
                            double[] ElmSta = new double[] { };
                            string[] LoadCase = new string[] { };
                            string[] StepType = new string[] { };
                            double[] StepNum = new double[] { };
                            double[] P = new double[] { };
                            double[] V2 = new double[] { };
                            double[] V3 = new double[] { };
                            double[] T = new double[] { };
                            double[] M2 = new double[] { };
                            double[] M3 = new double[] { };

                            ret = mySapModel.Results.FrameForce(name, eItemTypeElm.Element, ref NumberResults, ref Obj, ref ObjSta, ref Elm,
                                                                ref ElmSta,
                                                                ref LoadCase, ref StepType,
                                                                ref StepNum, ref P, ref V2, ref V3, ref T, ref M2,
                                                                ref M3);

                            string Label = null;
                            string Story = null;
                            ret = mySapModel.FrameObj.GetLabelFromName(name, ref Label, ref Story);


                            if (ret == 0 && NumberResults > 0)
                            {
                                for (int i = 0; i < NumberResults; i++)
                                {
                                    id++;
                                    acell.Offset[id, 0].Value = name;
                                    acell.Offset[id, 1].Value = Label;
                                    acell.Offset[id, 2].Value = Story;

                                    acell.Offset[id, 3].Value = LoadCase[i];
                                    acell.Offset[id, 4].Value = P[i];
                                    acell.Offset[id, 5].Value = M2[i];
                                    acell.Offset[id, 6].Value = M3[i];
                                }
                            }
                        }
                    }
                }    
            }
            catch
            {

            }
        }

        private void button6_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var xlApp = Globals.ThisWorkbook.Application;
                var ws = (Worksheet)Globals.ThisWorkbook.ActiveSheet;

                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                cSapModel mySapModel = myETABSObject.SapModel;
                int ret = -1;

                ret = mySapModel.SetPresentUnits(eUnits.kN_m_C);


                for (int i = 2; i <= 4; i++)
                {
                    double X1 = (double)ws.Range["J" + i].Value;
                    double Y1 = (double)ws.Range["K" + i].Value;
                    double Z1 = (double)ws.Range["L" + i].Value;
                    double X2 = (double)ws.Range["M" + i].Value;
                    double Y2 = (double)ws.Range["N" + i].Value;
                    double Z2 = (double)ws.Range["O" + i].Value;

                    string Name = null;
                    ret = mySapModel.FrameObj.AddByCoord(X1, Y1, Z1, X2, Y2, Z2, ref Name);
                    if (ret == 0)
                    {
                        ws.Range["I" + i].Value = Name;
                    }
                    


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
