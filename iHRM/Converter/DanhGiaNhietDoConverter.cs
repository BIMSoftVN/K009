using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iHRM.Converter
{
    public class DanhGiaNhietDoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double NhietDo)
            {
                if (NhietDo > 20)
                {
                    return "Nóng";
                }
                else
                {
                    return "Lạnh";
                }    
            }   
            return "Chưa đánh giá";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
