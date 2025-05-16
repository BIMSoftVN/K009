using AutoCADNet.Classes;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Linq;
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
                        List<DBText> textList = new List<DBText>();

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
                            else
                            {
                                var oText = tr.GetObject(e.ObjectId, OpenMode.ForRead) as DBText;
                                if (oText != null)
                                {
                                   textList.Add(oText);
                                }
                            }    

                        }    
                    
                        if (textList.Count>0)
                        {
                            foreach (var text in textList)
                            {
                                var cocList_NoName = cocList.Where(o => string.IsNullOrEmpty(o.Name));
                                if (cocList_NoName !=null && cocList_NoName.Count()>0)
                                {
                                    var Nested_Coc = cocList_NoName.OrderBy(o => o.Orgin.DistanceTo(text.Position)).First();
                                    Nested_Coc.Name = text.TextString;
                                    Nested_Coc.Distance = Nested_Coc.Orgin.DistanceTo(text.Position);
                                }   
                                else
                                {
                                    break;
                                }    
                            }    
                        }  
                        
                        var win = new vMain();
                        var winDataContext = (win.DataContext as vmMain);

                        winDataContext.BlockSource.Clear();
                        winDataContext.cocSource.Clear();

                        var blockTable = tr.GetObject(aDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                        if ( blockTable!=null)
                        {
                            foreach (var blockId in blockTable)
                            {
                                BlockTableRecord blockRec = tr.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;
                                if (blockRec!=null)
                                {
                                    if (!blockRec.Name.StartsWith("*"))
                                    {
                                        var block = new clBlockInfo
                                        {
                                            Id = blockRec.ObjectId,
                                            Name = blockRec.Name
                                        };
                                        winDataContext.BlockSource.Add(block);
                                    }    
                                }
                            }    
                        }

                        winDataContext.cocSource.AddRange(cocList);

                        win.Show();
                    }    
                }    
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

            }
        }
    }
}
