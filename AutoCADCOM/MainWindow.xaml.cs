using AutoCAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace AutoCADCOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                var aCadApp = new AcadApplication();
                aCadApp.Visible = true;
                var aDoc = aCadApp.ActiveDocument;

                var model = aDoc.ModelSpace;

                var Sp = new double[3] { 0, 0, 0 };
                var Ep = new double[3] { 10, 10, 0 };
                var oLine = model.AddLine(Sp, Ep);

                aCadApp.ZoomExtents();
            }
            catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var aCadApp = Marshal.GetActiveObject("AutoCAD.Application") as AcadApplication;
                var aDoc = aCadApp.ActiveDocument;

                var model = aDoc.ModelSpace;

                var Center = new double[3] { 0, 0, 0 };
                double Radius = 500;

                var oCircle = model.AddCircle(Center, Radius);
                oCircle.color = ACAD_COLOR.acGreen;

                aCadApp.ZoomExtents();
                aDoc.Regen(AcRegenType.acAllViewports);
            }
            catch
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var aCadApp = Marshal.GetActiveObject("AutoCAD.Application") as AcadApplication;
                var aDoc = aCadApp.ActiveDocument;

                var model = aDoc.ModelSpace;

                var goc = new double[3] { 0, 0, 0 };

                var StartPoint = (double[])(aDoc.Utility.GetPoint(Type.Missing, "Chọn một điểm"));


                DrawSection(model, StartPoint, 300 , 500, 30, 2, 25, 3, 22);

                aCadApp.ZoomExtents();
                aDoc.Regen(AcRegenType.acAllViewports);
            }
            catch
            {

            }
        }

        private void DrawSection(AcadModelSpace model, double[] StartPoint , double b, double h,
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

                double[] ExPoint1 = new double[3] { StartPoint[0], StartPoint[1] -50, 0 };
                double[] ExPoint2 = new double[3] { StartPoint[0] +b, StartPoint[1] - 50, 0 };
                double[] TextLoc = new double[3] { StartPoint[0] + b/2, StartPoint[1] - 200, 0 };

                var Dim1 = model.AddDimAligned(ExPoint1, ExPoint2, TextLoc);
                Dim1.StyleName = "DIM_01";

                ExPoint1 = new double[3] { StartPoint[0] - 50, StartPoint[1], 0 };
                ExPoint2 = new double[3] { StartPoint[0]-50, StartPoint[1]+h, 0 };
                TextLoc = new double[3] { StartPoint[0] -150, StartPoint[1]+h/2, 0 };

                var Dim2 = model.AddDimAligned(ExPoint1, ExPoint2, TextLoc);
                Dim2.StyleName = "DIM_01";
            }
            catch
            {

            }
        }
    }
}
