using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Models
{
    public class ProductSearch
    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();
        public List<Product> SearchByKey(string key)
        {
            return objWebsiteBanHangEntities.Product.SqlQuery("Select * From Product Where Name like N'%" + key + "%'").ToList();
        }
    }


}