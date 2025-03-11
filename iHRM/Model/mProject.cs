using iHRM.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Model
{
    public class mProject
    {
        public async static Task<List<clProject>> LoadAdd()
        {
            var plist = new List<clProject>();

            plist.Add(new clProject
            {
                Name = "Dự án Hà Nội",
                ProjectCode = "HN",
                ViDo = 21.028151190245786,
                KinhDo = 105.8509986536461,
            });

            plist.Add(new clProject
            {
                Name = "Dự án Đà Nẵng",
                ProjectCode = "DN",
                ViDo = 16.051260782084135,
                KinhDo = 108.19853440579374,
            });

            plist.Add(new clProject
            {
                Name = "Dự án Sài Gòn",
                ProjectCode = "SG",
                ViDo = 10.772242698812647,
                KinhDo = 106.72373582735602
            });

            return plist;
        }
    }
}
