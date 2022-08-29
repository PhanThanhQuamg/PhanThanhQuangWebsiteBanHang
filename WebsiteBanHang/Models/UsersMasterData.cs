using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebsiteBanHang.Context
{
    internal class UsersMasterData
    {
        public int Id { get; set; }
        [Display(Name = "Tên đầu")]

        public string FirstName { get; set; }
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name = "Thư điện tử")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
    }
}