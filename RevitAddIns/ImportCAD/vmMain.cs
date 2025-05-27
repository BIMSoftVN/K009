using Autodesk.Revit.DB;
using DevExpress.Mvvm;
using RevitAddIns.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RevitAddIns.Helper;
using System.Windows.Input;
using System.Drawing.Drawing2D;
using netDxf;
using DevExpress.Xpf.Core.Native;
using Autodesk.Revit.UI;

namespace RevitAddIns.ImportCAD
{
    public class vmMain : PropertyChangedBase
    {
        private ImportInstance _LinkCAD = null;
        public ImportInstance LinkCAD
        {
            get { return _LinkCAD; }
            set
            {
                _LinkCAD = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<clFamilyCocInfo> _FamilyList = new Helper.ObservableRangeCollection<clFamilyCocInfo>();
        public ObservableRangeCollection<clFamilyCocInfo> FamilyList
        {
            get { return _FamilyList; }
            set
            {
                _FamilyList = value;
                OnPropertyChanged();
            }
        }

        private clFamilyCocInfo _FamilySelect = new clFamilyCocInfo();
        public clFamilyCocInfo FamilySelect
        {
            get { return _FamilySelect; }
            set
            {
                _FamilySelect = value;
                OnPropertyChanged();
            }
        }


        private ObservableRangeCollection<clCADCircleInfo> _CADList = new Helper.ObservableRangeCollection<clCADCircleInfo>();
        public ObservableRangeCollection<clCADCircleInfo> CADList
        {
            get { return _CADList; }
            set
            {
                _CADList = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<clCADCircleInfo> _CADSelect = new Helper.ObservableRangeCollection<clCADCircleInfo>();
        public ObservableRangeCollection<clCADCircleInfo> CADSelect
        {
            get { return _CADSelect; }
            set
            {
                _CADSelect = value;
                OnPropertyChanged();
            }
        }



        private DelegateCommand loadAllCmd;

        public ICommand LoadAllCmd
        {
            get
            {
                if (loadAllCmd == null)
                {
                    loadAllCmd = new DelegateCommand(PerformLoadAllCmd);
                }

                return loadAllCmd;
            }
        }

        private async void PerformLoadAllCmd()
        {
            try
            {
                FamilyList.Clear();

               var  flist = new List<clFamilyCocInfo>();

                var doc = LinkCAD.Document;
                FilteredElementCollector collector = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_StructuralFoundation)
                    .OfClass(typeof(FamilySymbol));

                var CocTypes = collector.ToElements();

                var DisplayLengthUnitType = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();


                foreach (FamilySymbol item in CocTypes)
                {
                    clFamilyCocInfo family = new clFamilyCocInfo();
                    family.Id = item.Id;
                    family.Family = item.FamilyName;
                    family.Type = item.Name;
                    var par = item.LookupParameter("Width");
                    if (par != null)
                    {
                        var diameter = par.AsDouble();
                        family.Diameter = UnitUtils.ConvertFromInternalUnits(diameter, DisplayLengthUnitType);
                    }
                    else
                    {
                        family.Diameter = 0;
                    }

                    flist.Add(family);
                }

                FamilyList.AddRange(flist);


               var clist = await DocCADtoThuVien(LinkCAD);

                CADList.AddRange(clist);

            }
            catch (Exception ex)
            {
            }
        }


        private async Task<List<clCADCircleInfo>> DocCADtuRevit(Document doc)
        {
            //Cách 1, đọc dữ liệu CAD từ Revit

            var clist = new List<clCADCircleInfo>();

            var DisplayLengthUnitType = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();


            var cadList = new List<clCADCircleInfo>();
            var Opts = doc.Application.Create.NewGeometryOptions();
            var GeoEle1 = LinkCAD.get_Geometry(Opts);
            if (GeoEle1 != null)
            {
                foreach (var GeoObj1 in GeoEle1)
                {
                    var GI1 = GeoObj1 as GeometryInstance;
                    if (GI1 != null)
                    {
                        var GeoEle2 = GI1.GetInstanceGeometry();
                        if (GeoEle2 != null)
                        {
                            foreach (var GeoObj2 in GeoEle2)
                            {
                                if (GeoObj2 is Arc)
                                {
                                    var Arc = GeoObj2 as Arc;
                                    if (Arc.IsClosed)
                                    {
                                        var cir = new clCADCircleInfo();
                                        cir.Diameter = UnitUtils.ConvertFromInternalUnits(Arc.Radius * 2, DisplayLengthUnitType);
                                        cir.X = UnitUtils.ConvertFromInternalUnits(Arc.Center.X, DisplayLengthUnitType);
                                        cir.Y = UnitUtils.ConvertFromInternalUnits(Arc.Center.Y, DisplayLengthUnitType);
                                        cir.Layer = (doc.GetElement(GeoObj2.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory.Name;

                                        clist.Add(cir);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return clist;
        }
    
        private async Task<List<clCADCircleInfo>> DocCADtoThuVien(ImportInstance LinkCAD)
        {
            var clist = new List<clCADCircleInfo>();    
            try
            {
                var doc = LinkCAD.Document;
                var DisplayLengthUnitType = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();

                var typeId = LinkCAD.GetTypeId();
                var type = doc.GetElement(typeId) as ElementType;
                var extRef = type.GetExternalFileReference();
                string filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(extRef.GetAbsolutePath());

                var cadDoc = DxfDocument.Load(filePath);
                var cList = cadDoc.Entities.Circles;

                foreach (var c in cList) 
                {
                    var cir = new clCADCircleInfo();
                    cir.Diameter = c.Radius * 2;
                    cir.X = c.Center.X;
                    cir.Y = c.Center.Y;
                    cir.Layer = c.Layer.Name;

                    clist.Add(cir);
                }
            }
            catch
            {

            }

            return clist;

        }

        private DelegateCommand importCADCommand;

        public ICommand ImportCADCommand
        {
            get
            {
                if (importCADCommand == null)
                {
                    importCADCommand = new DelegateCommand(ImportCAD);
                }

                return importCADCommand;
            }
        }

        private void ImportCAD()
        {
            try
            {
                var doc = LinkCAD.Document;
                var DisplayLengthUnitType = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();

                var CadTrans = LinkCAD.GetTotalTransform();



                using (Transaction tr = new Transaction(doc, "Vẽ cọc"))
                {
                    tr.Start();

                    //foreach (var family in FamilyList)
                    //{
                    //    var faSymbol = doc.GetElement(FamilySelect.Id) as FamilySymbol;
                    //    if (!faSymbol.IsActive)
                    //    {
                    //        faSymbol.Activate();
                    //        doc.Regenerate();
                    //    }
                    //}    


                    foreach (var coc in CADSelect)
                    {
                        var family = FamilyList.Where(o => Math.Abs(o.Diameter.Value - coc.Diameter.Value) < 0.1 && o.Family == FamilySelect.Family).FirstOrDefault();
                        if (family!=null)
                        {
                            var faSymbol = doc.GetElement(family.Id) as FamilySymbol;
                            if (!faSymbol.IsActive)
                            {
                                faSymbol.Activate();
                                doc.Regenerate();
                            }

                            var CadLoc = new XYZ(
                                UnitUtils.ConvertToInternalUnits(coc.X.Value, DisplayLengthUnitType),
                                UnitUtils.ConvertToInternalUnits(coc.Y.Value, DisplayLengthUnitType),
                            0);

                            var revitLoc = CadTrans.OfPoint(CadLoc);

                            var faIns = doc.Create.NewFamilyInstance(revitLoc, faSymbol, Autodesk.Revit.DB.Structure.StructuralType.Footing);
                        }    
                    }
                    tr.Commit();
                    TaskDialog.Show("Thông báo", "Đã vẽ xong cọc từ CAD sang Revit");
                }    
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Thông báo", ex.Message);
            }
        }
    }
}
