using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIns.ImportCAD
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWindow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDoc = commandData.Application.ActiveUIDocument;
            var doc = uiDoc.Document;

            var linkFilter = new linkCADFilter();
            var LinkCAD = uiDoc.Selection.PickObject(ObjectType.Element, linkFilter, "Chọn file LinkCAD");

            var win = new vMain();
            ((vmMain)win.DataContext).LinkCAD = doc.GetElement(LinkCAD.ElementId) as ImportInstance;

            win.ShowDialog();

            return Result.Succeeded;
        }
    }

    public class linkCADFilter : ISelectionFilter
    {
        public bool AllowElement(Element e)
        {
            if (e is ImportInstance cadLink && cadLink.IsLinked==true)
            {
                return true;
            }    
                
            return false;
        }
        public bool AllowReference(Reference r, XYZ p)
        {
            return false;
        }
    }
}
