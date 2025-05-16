using Autodesk.AutoCAD.DatabaseServices;
using DevExpress.Data.XtraReports.Native;
using K009Libs.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADNet.Classes
{
    public class clBlockInfo : PropertyChangedBase
    {
        private ObjectId _Id = new ObjectId();
        public ObjectId Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }
        

        private string _Name = null;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
    }
}
