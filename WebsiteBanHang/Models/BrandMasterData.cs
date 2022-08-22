using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public partial class BrandMasterData
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }
        [Display(Name = "Hình đại diện")]
        public string Avatar { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Hiện thị")]
        public Nullable<bool> ShowOnHomePage { get; set; }
        public Nullable<int> DisPlayOrder { get; set; }
        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> UpdatedOnUtc { get; set; }
        public Nullable<bool> Deleted { get; set; }
    }
}