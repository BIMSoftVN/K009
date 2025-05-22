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

        private void PerformLoadAllCmd()
        {
            try
            {
                FamilyList.Clear();

                List<clFamilyCocInfo> list = new List<clFamilyCocInfo>();

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

                    list.Add(family);
                }

                FamilyList.AddRange(list);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
