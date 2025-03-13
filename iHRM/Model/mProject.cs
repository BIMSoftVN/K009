using iHRM.Classes;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Model
{
    public class mProject
    {
        public async static Task<List<clProject>> LoadFromSetting()
        {
            var plist = new List<clProject>();

            string json = Properties.Settings.Default.ProjectListJson;

            plist = JToken.Parse(json).ToObject<List<clProject>>();

            
            return plist;
        }

        public async static Task<List<clProject>> LoadFromTextFile(string filePath)
        {
            var plist = new List<clProject>();

            string json = File.ReadAllText(filePath);

            plist = JToken.Parse(json).ToObject<List<clProject>>();


            return plist;
        }
    }
}
