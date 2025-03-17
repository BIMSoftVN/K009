using iHRM.Classes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Model
{
    [TestFixture]
    public class mUser
    {
        [Test]
        public async Task Test()
        {
            await GetAllUsers(@"C:\Users\kysudo\Desktop\K009.db");
        }


        public static async Task<List<clUser>> GetAllUsers(string SqlFilePath)
        {
            var uList = new List<clUser>();

            string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


            using (var conn = new SQLiteConnection(ConnString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [Users]";
                    var reader = await cmd.ExecuteReaderAsync();
                    var dt = new DataTable();
                    dt.Load(reader);

                    if (dt!=null && dt.Rows.Count>0)
                    {
                        uList = JToken.FromObject(dt).ToObject<List<clUser>>();
                    }    
                }

                Console.WriteLine(SqlFilePath);
            }

            return uList;
        }
    }
}
