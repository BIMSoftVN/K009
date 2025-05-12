using AutoCADNet.Classes;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AutoCADNet.TKC
{
    public class Commands
    {
        [CommandMethod("TKC")]
        public static void ThongKeCoc()
        {
            try
            {
                Document aDoc = Application.DocumentManager.MdiActiveDocument;
                Editor aEdit = aDoc.Editor;
                Database aDb = aDoc.Database;

                using (Transaction tr = aDb.TransactionManager.StartTransaction())
                {
                    TypedValue[] Tval = new TypedValue[4];

                    Tval.SetValue(new TypedValue((int)DxfCode.Operator, "<OR"), 0);
                    Tval.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 1);
                    Tval.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 2);
                    Tval.SetValue(new TypedValue((int)DxfCode.Operator, "OR>"), 3);


                    SelectionFilter filter = new SelectionFilter(Tval);

                    var res = aEdit.GetSelection(filter);
                    if (res.Status == PromptStatus.OK)
                    {
                        var sset = res.Value;

                        List<clCocInfo> cocList = new List<clCocInfo>();

                        foreach (SelectedObject e in sset)
                        {
                            var oCir = tr.GetObject(e.ObjectId, OpenMode.ForRead) as Circle;
                            if (oCir != null)
                            {
                                var coc = new clCocInfo
                                {
                                    Id = oCir.ObjectId,
                                    Layer = oCir.Layer,
                                    Diameter = oCir.Diameter,
                                    Orgin = oCir.Center
                                };

                                cocList.Add(coc);
                            }
                        }    
                    }    
                }    
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

            }
        }
    }
}
