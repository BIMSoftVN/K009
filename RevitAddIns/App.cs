using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIns
{
    class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string TabName = "K009";
            string PanelName = "K009_Panel";

            string assemblyName = typeof(App).Assembly.Location;

            application.CreateRibbonTab(TabName);
            var panel = application.CreateRibbonPanel(TabName, PanelName);

            var btn = new PushButtonData("VeCoc", "Vẽ cọc", assemblyName, "RevitAddIns.AssignBIMId");
            btn.ToolTip = "Vẽ cọc";

            btn.LargeImage = Helper.ConvertResourcetoImgSource("RevitAddIns.Photos.SolidWorks.png", 32);
            btn.Image = Helper.ConvertResourcetoImgSource("RevitAddIns.Photos.SolidWorks.png", 16);

            panel.AddItem(btn);

            var btnWin = new PushButtonData("MoWindow", "Mở cửa sổ", assemblyName, "RevitAddIns.ImportCAD.OpenWindow");
            btnWin.ToolTip = "Vẽ cọc";

            btnWin.LargeImage = Helper.ConvertResourcetoImgSource("RevitAddIns.Photos.SolidWorks.png", 32);
            btnWin.Image = Helper.ConvertResourcetoImgSource("RevitAddIns.Photos.SolidWorks.png", 16);

            panel.AddItem(btnWin);

            return Result.Succeeded;
        }
    }
}
