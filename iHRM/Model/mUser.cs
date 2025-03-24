using iHRM.Classes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
            string ConnString = @"Server=./;Database=iHRM;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            var user = new clUserEF6()
            {
                Id = "125416",
                Name = "Đỗ Lê Xuân Trung",
                PhoneNumber = "0901",
                DateOfBirth = DateTime.Now
            };

            var kq = await UpdateUsers(ConnString, user);
            //var kq = await GetAllUsersSQLServer(ConnString);
            //await GetAllUsers(@"C:\Users\kysudo\Desktop\K009.db");

            //var user = new clUser()
            //{
            //    Id = 1,
            //    Name = "Kysudo",
            //    PhoneNumber = "123456789",
            //    DateOfBirth = DateTime.Now,
            //    Address = "Hanoi"
            //};

            //var kq = await CheckPassword(@"C:\Users\kysudo\Desktop\K009.db", user);
            //if (kq == true)
            //{
            //    Console.WriteLine("Login success");
            //}
            //else
            //{
            //    Console.WriteLine("Login fail");
            //}
        }

        public static async Task<List<clUserEF6>> GetAllUsers(string ConnectionString)
        {
            var uList = new List<clUserEF6>();

            using (var context = new EF6(ConnectionString))
            {
                uList = await context.Users.AsNoTracking().ToListAsync();
            }


            return uList;
        }

        public static async Task<clUserEF6> AddUsers(string ConnectionString, clUserEF6 user)
        {
            using (var context = new EF6(ConnectionString))
            {
                context.Users.Add(user);
                var kq = await context.SaveChangesAsync();
            }
            return user;
        }

        public static async Task<clUserEF6> UpdateUsers(string ConnectionString, clUserEF6 user)
        {
            using (var context = new EF6(ConnectionString))
            {
                var userUpdate = await context.Users.Where(p=>p.Id == user.Id).FirstOrDefaultAsync();
                if (userUpdate != null)
                {
                    userUpdate.Name = user.Name;
                    userUpdate.PhoneNumber = user.PhoneNumber;
                    userUpdate.DateOfBirth = user.DateOfBirth;
                    userUpdate.IsMan = user.IsMan;
                    var kq = await context.SaveChangesAsync();
                }
            }
            return user;
        }



        //Học SqLite
        //public static async Task<bool> CheckPassword(string SqlFilePath, clUser user)
        //{
        //    bool res = false;

        //    string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


        //    using (var conn = new SQLiteConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.Parameters.AddWithValue("@Name", user.Name);
        //            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        //            cmd.CommandText = "SELECT * FROM [Users] WHERE [Name]=@Name AND [PhoneNumber]=@PhoneNumber";
        //            var reader = await cmd.ExecuteReaderAsync();
        //            var dt = new DataTable();
        //            dt.Load(reader);

        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                res = true;
        //                var uList = JToken.FromObject(dt).ToObject<List<clUser>>();
        //            }
        //        }

        //        Console.WriteLine(SqlFilePath);
        //    }

        //    return res;
        //}


        //public static async Task<List<clUser>> GetAllUsers(string SqlFilePath)
        //{
        //    var uList = new List<clUser>();

        //    string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


        //    using (var conn = new SQLiteConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT * FROM [Users] ";
        //            var reader = await cmd.ExecuteReaderAsync();
        //            var dt = new DataTable();
        //            dt.Load(reader);

        //            if (dt!=null && dt.Rows.Count>0)
        //            {
        //                uList = JToken.FromObject(dt).ToObject<List<clUser>>();
        //            }    
        //        }

        //        Console.WriteLine(SqlFilePath);
        //    }

        //    return uList;
        //}

        //public static async Task<bool> AddUsers(string SqlFilePath, clUser user)
        //{
        //    bool res = false;

        //    string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


        //    using (var conn = new SQLiteConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (var cmd = conn.CreateCommand())
        //                {
        //                    cmd.Transaction = trans;

        //                    cmd.Parameters.AddWithValue("@Name", user.Name);
        //                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        //                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
        //                    cmd.Parameters.AddWithValue("@Address", user.Address);

        //                    cmd.CommandText = "INSERT INTO [Users]([Name], [PhoneNumber], [DateOfBirth], [Address]) VALUES (@Name, @PhoneNumber, @DateOfBirth, @Address)";
        //                    var ktInt = await cmd.ExecuteNonQueryAsync();
        //                    if (ktInt > 0)
        //                    {
        //                        res = true;
        //                    }
        //                    trans.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //            }
        //        }    



        //        Console.WriteLine(SqlFilePath);
        //    }

        //    return res;
        //}

        //public static async Task<List<clUser>> DeleteUsers(string SqlFilePath, List<clUser> userList)
        //{
        //    List<clUser> userOutList = new List<clUser>();

        //    string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


        //    using (var conn = new SQLiteConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (var user in userList)
        //                {
        //                    using (var cmd = conn.CreateCommand())
        //                    {
        //                        cmd.Transaction = trans;

        //                        cmd.Parameters.AddWithValue("@Id", user.Id);

        //                        cmd.CommandText = "DELETE FROM [Users] WHERE [Id]=@Id";
        //                        var ktInt = await cmd.ExecuteNonQueryAsync();
        //                        if (ktInt > 0)
        //                        {
        //                            userOutList.Add(user);
        //                        }

        //                    }
        //                }

        //                trans.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //            }
        //        }



        //        Console.WriteLine(SqlFilePath);
        //    }

        //    return userOutList;
        //}

        //public static async Task<bool> UpdateUser(string SqlFilePath, clUser user)
        //{
        //    bool res = false;

        //    string ConnString = @"Data Source=" + SqlFilePath + ";Version=3;";


        //    using (var conn = new SQLiteConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (var cmd = conn.CreateCommand())
        //                {
        //                    cmd.Transaction = trans;

        //                    cmd.Parameters.AddWithValue("@Id", user.Id);
        //                    cmd.Parameters.AddWithValue("@Name", user.Name);
        //                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        //                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
        //                    cmd.Parameters.AddWithValue("@Address", user.Address);

        //                    cmd.CommandText = "UPDATE [Users] SET [Name]=@Name, [PhoneNumber]=@PhoneNumber, [DateOfBirth]=@DateOfBirth, [Address]=@Address WHERE [Id]=@Id";
        //                    var ktInt = await cmd.ExecuteNonQueryAsync();
        //                    if (ktInt > 0)
        //                    {
        //                        res = true;
        //                    }
        //                    trans.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //            }
        //        }



        //        Console.WriteLine(SqlFilePath);
        //    }

        //    return res;
        //}

        //public static async Task<List<clUserSqlServer>> GetAllUsersSQLServer(string ConnString)
        //{
        //    var uList = new List<clUserSqlServer>();

        //    using (var conn = new SqlConnection(ConnString))
        //    {
        //        conn.Open();

        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT * FROM [Users] ";
        //            var reader = await cmd.ExecuteReaderAsync();
        //            var dt = new DataTable();
        //            dt.Load(reader);

        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                uList = JToken.FromObject(dt).ToObject<List<clUserSqlServer>>();
        //            }
        //        }
        //    }

        //    return uList;
        //}


    }
}
