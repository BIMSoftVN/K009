using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWorkbook1.Classes
{
    public class clWeatherInfo
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
            }
        }

        private string _wImage = null;
        public string wImage
        {
            get
            {
                return _wImage;
            }
            set
            {
                _wImage = value;

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
            }
        }

    }
}
