using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIns
{
    [Transaction(TransactionMode.Manual)]
    public class AssignBIMId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View activeView = doc.ActiveView;

            FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id).WhereElementIsNotElementType();

            var ActiveElement = collector.ToList();

            if (ActiveElement == null || ActiveElement.Count==0)
            {
                TaskDialog.Show("Error", "Không có phần tử nào trong View này.");
                return Result.Failed;
            }

            long count = 0;
            
            using (Transaction tx = new Transaction(doc, "Gán Id vào biến BIMId"))
            {
                tx.Start();

                foreach (var element in ActiveElement)
                {

                    Parameter bimIDPara = element.LookupParameter("BIMId");
                    if (bimIDPara != null && bimIDPara.IsReadOnly == false)
                    {
                        bimIDPara.Set(element.Id.Value.ToString());
                        count++;
                    }
                }

                tx.Commit();
            }    

            

            TaskDialog.Show("Thông báo", "Đã gán BIMId cho " + count + " cấu kiện");

            return Result.Succeeded;
        }
    }
}
