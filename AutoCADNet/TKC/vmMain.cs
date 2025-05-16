using AutoCADNet.Classes;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using DevExpress.Data.XtraReports.Native;
using DevExpress.Mvvm;
using K009Libs.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoCADNet.TKC
{
    public class vmMain : PropertyChangedBase
    {
        private ObservableRangeCollection<clBlockInfo> _BlockSource = new ObservableRangeCollection<clBlockInfo>();
        public ObservableRangeCollection<clBlockInfo> BlockSource
        {
            get
            {
                return _BlockSource;
            }
            set
            {
                _BlockSource = value;
                OnPropertyChanged();
            }
        }

        private clBlockInfo _BlockTitle = new clBlockInfo();
        public clBlockInfo BlockTitle
        {
            get
            {
                return _BlockTitle;
            }
            set
            {
                _BlockTitle = value;
                OnPropertyChanged();
            }
        }


        private clBlockInfo _BlockBody = new clBlockInfo();
        public clBlockInfo BlockBody
        {
            get
            {
                return _BlockBody;
            }
            set
            {
                _BlockBody = value;
                OnPropertyChanged();
            }
        }



        private ObservableRangeCollection<clCocInfo> _cocSource = new ObservableRangeCollection<clCocInfo>();
        public ObservableRangeCollection<clCocInfo> cocSource
        {
            get
            {
                return _cocSource;
            }
            set
            {
                _cocSource = value;
                OnPropertyChanged();
            }
        }

        private DelegateCommand drawTableCommand;

        public ICommand DrawTableCommand
        {
            get
            {
                if (drawTableCommand == null)
                {
                    drawTableCommand = new DelegateCommand(DrawTable);
                }

                return drawTableCommand;
            }
        }

        private void DrawTable()
        {
            try
            {
                if (BlockTitle == null || BlockBody == null)
                {
                    System.Windows.MessageBox.Show("Vui lòng chọn block tiêu đề và body");
                    return;
                }

                var aDoc = Application.DocumentManager.MdiActiveDocument;

                using (var LockDoc = aDoc.LockDocument())
                {
                    var aDocEd = aDoc.Editor;
                    var aDocDb = aDoc.Database;

                    using (var tr = aDocDb.TransactionManager.StartTransaction())
                    {
                        try
                        {
                            var blockTable = tr.GetObject(aDocDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                            if (blockTable != null)
                            {
                                var modelSpace = blockTable[BlockTableRecord.ModelSpace];
                                var modelSpaceRecord = tr.GetObject(modelSpace, OpenMode.ForWrite) as BlockTableRecord;

                                var PromptPointResult =  aDocEd.GetPoint("\nChọn điểm đặt tiêu đề: ");
                                if (PromptPointResult.Status == PromptStatus.OK)
                                {
                                    var InsPoint = PromptPointResult.Value;
                                    var blockRefTitle = new BlockReference(InsPoint, BlockTitle.Id);

                                    modelSpaceRecord.AppendEntity(blockRefTitle);
                                    tr.AddNewlyCreatedDBObject(blockRefTitle, true);

                                    var bBox = blockRefTitle.GeometricExtents;
                                    InsPoint = new Autodesk.AutoCAD.Geometry.Point3d(InsPoint.X, InsPoint.Y - (bBox.MaxPoint.Y - bBox.MinPoint.Y), 0);

                                    foreach (var coc in cocSource)
                                    {
                                        using (var BlockRefBody = new BlockReference(InsPoint, BlockBody.Id))
                                        {
                                            modelSpaceRecord.AppendEntity(BlockRefBody);
                                            tr.AddNewlyCreatedDBObject(BlockRefBody, true);

                                            var bt = aDocDb.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
                                            var blockTableRecord = bt[BlockBody.Name].GetObject(OpenMode.ForRead) as BlockTableRecord;
                                            foreach (var id in blockTableRecord)
                                            {
                                                var ent = id.GetObject(OpenMode.ForRead);
                                                if (ent != null)
                                                {
                                                    var attDef = ent as AttributeDefinition;
                                                    if (attDef!=null)
                                                    {
                                                        if (attDef.Tag.ToUpper() == "NAME")
                                                        {
                                                            using (var attRef = new AttributeReference())
                                                            {
                                                                attRef.SetAttributeFromBlock(attDef, BlockRefBody.BlockTransform);
                                                                if (string.IsNullOrEmpty(coc.Name))
                                                                {
                                                                    coc.Name = "";
                                                                }    
                                                                attRef.TextString = coc.Name;

                                                                BlockRefBody.AttributeCollection.AppendAttribute(attRef);
                                                                tr.AddNewlyCreatedDBObject(attRef, true);
                                                            }
                                                        }

                                                        if (attDef.Tag.ToUpper() == "DK")
                                                        {
                                                            using (var attRef = new AttributeReference())
                                                            {
                                                                attRef.SetAttributeFromBlock(attDef, BlockRefBody.BlockTransform);
                                                                if (coc.Diameter ==null)
                                                                {
                                                                    attRef.TextString = "";
                                                                }
                                                                else
                                                                {
                                                                    attRef.TextString = Math.Round(coc.Diameter.Value, 0).ToString();
                                                                }    
                                                                
                                                                BlockRefBody.AttributeCollection.AppendAttribute(attRef);
                                                                tr.AddNewlyCreatedDBObject(attRef, true);
                                                            }
                                                        }

                                                        if (attDef.Tag.ToUpper() == "X")
                                                        {
                                                            using (var attRef = new AttributeReference())
                                                            {
                                                                attRef.SetAttributeFromBlock(attDef, BlockRefBody.BlockTransform);
                                                                if (coc.Orgin == null || coc.Orgin.X == null)
                                                                {
                                                                    attRef.TextString = "";
                                                                }
                                                                else
                                                                {
                                                                    attRef.TextString = Math.Round(coc.Orgin.X, 2).ToString();
                                                                }

                                                                BlockRefBody.AttributeCollection.AppendAttribute(attRef);
                                                                tr.AddNewlyCreatedDBObject(attRef, true);
                                                            }
                                                        }

                                                        if (attDef.Tag.ToUpper() == "Y")
                                                        {
                                                            using (var attRef = new AttributeReference())
                                                            {
                                                                attRef.SetAttributeFromBlock(attDef, BlockRefBody.BlockTransform);
                                                                if (coc.Orgin == null || coc.Orgin.Y == null)
                                                                {
                                                                    attRef.TextString = "";
                                                                }
                                                                else
                                                                {
                                                                    attRef.TextString = Math.Round(coc.Orgin.Y, 2).ToString();
                                                                }

                                                                BlockRefBody.AttributeCollection.AppendAttribute(attRef);
                                                                tr.AddNewlyCreatedDBObject(attRef, true);
                                                            }
                                                        }
                                                    }    
                                                }
                                            }


                                            var bBox2 = BlockRefBody.GeometricExtents;
                                            InsPoint = new Autodesk.AutoCAD.Geometry.Point3d(InsPoint.X, InsPoint.Y - (bBox2.MaxPoint.Y - bBox2.MinPoint.Y), 0);
                                        }    
                                    }    


                                    tr.Commit();
                                    aDocEd.Regen();
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
