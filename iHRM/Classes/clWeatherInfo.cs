using K009Libs.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace iHRM.Classes
{
    public class clWeatherInfo : PropertyChangedBase
    {
        private DateTime? _wDate = null;
        public DateTime? wDate
        {
            get
            {
                return _wDate;
            }
            set
            {
                _wDate = value;
                OnPropertyChanged();
            }
        }

        private string _wImage =null;
        public string wImage
        {
            get
            {
                return _wImage;
            }
            set
            {
                _wImage = value;
                OnPropertyChanged();
            }
        }

        private string _wMoTa = null;
        public string wMoTa
        {
            get
            {
                return _wMoTa;
            }
            set
            {
                _wMoTa = value;
                OnPropertyChanged();
            }
        }

        private double? _wNhietDo = null;
        public double? wNhietDo
        {
            get
            {
                return _wNhietDo;
            }
            set
            {
                _wNhietDo = value;
                OnPropertyChanged();
            }
        }

        private double? _wGio = null;
        public double? wGio
        {
            get
            {
                return _wGio;
            }
            set
            {
                _wGio = value;
                OnPropertyChanged();
            }
        }

    }
}
