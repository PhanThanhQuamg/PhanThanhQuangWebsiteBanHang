using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using static WebsiteBanHang.Common;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();
        // GET: Admin/Brand
        public ActionResult Index()
        {
            var lstUsers = objWebsiteBanHangEntities.Users.ToList();
            return View(lstUsers);
        }
        public ActionResult Details(int Id)
        {
            var lstUsers = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(lstUsers);
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.loadData();

            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Users objusers)
        {
            //if (ModelState.IsValid)
            //{

                //this.loadData();
                //try
                //{
                //    if (objusers.ImageUpload != null)
                //    {
                //        string fileName = Path.GetFileNameWithoutExtension(objusers.ImageUpload.FileName);
                //        //tenhinh
                //        string extension = Path.GetExtension(objusers.ImageUpload.FileName);
                //        //png
                //        fileName = fileName + extension;
                //        //tenhinh.png
                //        objusers.Avatar = fileName;
                //        objusers.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
                //    }
                //    objusers.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanHangEntities.Users.Add(objusers);
                    objWebsiteBanHangEntities.SaveChanges();

                    return RedirectToAction("Index");
            //    }
            //    catch
            //    {
            //        return View();
            //    }
            //}
            //var lstUsers = objWebsiteBanHangEntities.Users.ToList();
            //return View(objusers);
        }
        public ActionResult Edit(int Id)
        {
            this.loadData();
            var objusers = objWebsiteBanHangEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objusers);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Users objusers)
        {

            //if (objusers.ImageUpload != null)
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(objusers.ImageUpload.FileName);
            //    //tenhinh
            //    string extension = Path.GetExtension(objusers.ImageUpload.FileName);
            //    //png
            //    fileName = fileName + extension;
            //    //tenhinh.png
            //    objusers.Avatar = fileName;
            //    objusers.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
            //}
            objWebsiteBanHangEntities.Entry(objusers).State = EntityState.Detached;
            objWebsiteBanHangEntities.Entry(objusers).State = EntityState.Modified;
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            var objusers = objWebsiteBanHangEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objusers);
        }
        [HttpPost]
        public ActionResult Delete(Users objuse)
        {
            var objusers = objWebsiteBanHangEntities.Users.Where(n => n.Id == objuse.Id).FirstOrDefault();
            objWebsiteBanHangEntities.Users.Remove(objusers);
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        void loadData()
        {
            Common objCommon = new Common();
            //get data category to DB
            var listCat = objWebsiteBanHangEntities.Category.ToList();
            //convert to select list type value,text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(listCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //get data brand to DB
            var listBrand = objWebsiteBanHangEntities.Brand.ToList();
            DataTable dtBrand = converter.ToDataTable(listBrand);
            //convert to select list type value,text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm
            List<ProductType> listProductType = new List<ProductType>();

            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            listProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            listProductType.Add(objProductType);

            //get data brand to DB
            DataTable dtProductType = converter.ToDataTable(listProductType);
            //convert to select list type value,text
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }

    }
}