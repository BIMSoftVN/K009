using NhanSu.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhanSu.Models
{
    internal class mUser
    {
        public static async Task<(bool IsSuccess, string Message, clUser User)> GetUserByEmail(string Email, string Password)
        {
            bool IsSuccess = false;
            string message = null;
            clUser user_Out = null;


            using (var context = new EF6(GlobalVar.ConnString))
            {
                var user = await context.Users.Where(u=>u.Email == Email && u.Password == Password).AsNoTracking().FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.Password == Password)
                    {
                        IsSuccess = true;
                        message = "Đăng nhập thành công";
                        user_Out = user;
                    }
                    else
                    {
                        IsSuccess = false;
                        message = "Mật khẩu không đúng";
                    }    
                }
                else
                {
                    IsSuccess = false;
                    message = "Email không tồn tại";
                }    
            }


            return (IsSuccess, message, user_Out);
        }
        public static async Task<(bool IsSuccess, string Message, clUser User)> GetUserById(string UserId)
        {
            bool IsSuccess = false;
            string message = null;
            clUser user_Out = null;


            using (var context = new EF6(GlobalVar.ConnString))
            {
                var user = await context.Users.Where(u => u.Id == UserId).AsNoTracking().FirstOrDefaultAsync();
                if (user != null)
                {
                    IsSuccess = true;
                    user_Out = user;
                }
                else
                {
                    IsSuccess = false;
                    message = "Tài khoản không tồn tại";
                }
            }


            return (IsSuccess, message, user_Out);
        }

        public static async Task<(bool IsSuccess, string Message)> SaveUserData(clUser user)
        {
            bool IsSuccess = false;
            string message = null;


            using (var context = new EF6(GlobalVar.ConnString))
            {
                var userServer = await context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

                if (userServer != null)
                {
                    userServer.PhoneNumber = user.PhoneNumber;
                    userServer.DateOfBirth = user.DateOfBirth;
                    userServer.Address = user.Address;
                    userServer.Email = user.Email;
                    userServer.Name = user.Name;
                    userServer.Photo = user.Photo;

                    var kq = await context.SaveChangesAsync();
                    if (kq > 0)
                    {
                        IsSuccess = true;
                        message = "Cập nhật thành công";
                    }
                    else
                    {
                        IsSuccess = false;
                        message = "Không thể sửa thông tin";
                    }
                }
                else
                {
                    IsSuccess = false;
                    message = "Người dùng không tồn tại";
                }
            }


            return (IsSuccess, message);
        }


        public static async Task<(bool IsSuccess, string Message, List<clUser> UserList)> GetAllUser()
        {
            bool IsSuccess = false;
            string message = null;
            List<clUser> user_Out = null;


            using (var context = new EF6(GlobalVar.ConnString))
            {
                var user = await context.Users.AsNoTracking().ToListAsync();
                if (user != null)
                {
                    IsSuccess = true;
                    message = "Đã lấy dữ liệu";
                    user_Out = user;
                }
                else
                {
                    IsSuccess = false;
                    message = "Không có dữ liệu";
                }
            }


            return (IsSuccess, message, user_Out);
        }


    }
}
