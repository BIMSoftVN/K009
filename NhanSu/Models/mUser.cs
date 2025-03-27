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
    }
}
