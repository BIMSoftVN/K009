using K009Libs.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Classes
{
    public class clProject : PropertyChangedBase
    {
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

        private string _ProjectCode = null;
        public string ProjectCode
        {
            get
            {
                return _ProjectCode;
            }
            set
            {
                _ProjectCode = value;
                OnPropertyChanged();
            }
        }

        private double? _KinhDo = null;
        public double? KinhDo
        {
            get
            {
                return _KinhDo;
            }
            set
            {
                _KinhDo = value;
                OnPropertyChanged();
            }
        }

        private double? _ViDo = null;
        public double? ViDo
        {
            get
            {
                return _ViDo;
            }
            set
            {
                _ViDo = value;
                OnPropertyChanged();
            }
        }

    }
}
