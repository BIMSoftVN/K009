using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Model
{
    internal class mGoogle
    {
        public static async Task<string> DichAsync(string NoiDung)
        {
            string KetQuaDich = string.Empty;

            string URL = @"https://translate.google.com/m?sl=auto&tl=en&ie=UTF-8&prev=_m&q=" + NoiDung;
            string DIV_RESULT = "<div class=\"result-container\">";

            using (HttpClient client = new HttpClient())
            {
                string resp = await client.GetStringAsync(URL);
                int p1 = resp.IndexOf(DIV_RESULT);
                if (p1!=-1)
                {
                    p1 += DIV_RESULT.Length;
                    int p2 = resp.IndexOf("</div>", p1);
                    if (p2!=-1)
                    {
                        KetQuaDich = resp.Substring(p1, p2 - p1);
                    }    
                }    
            }    

           return KetQuaDich;
        }
    }
}
