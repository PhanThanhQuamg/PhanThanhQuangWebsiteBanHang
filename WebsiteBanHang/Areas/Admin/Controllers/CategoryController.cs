using Newtonsoft.Json.Bson;
using PagedList;
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
    public class CategoryController : Controller
    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();
        // GET: Admin/Brand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            List<Category> category;
            if (SearchString != null)
            {
                page = 1;

            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))

            {
                category = objWebsiteBanHangEntities.Category.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                category = objWebsiteBanHangEntities.Category.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            category = category.OrderByDescending(n => n.Id).ToList();
            return View(category.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int Id)
        {
            var lstCategory = objWebsiteBanHangEntities.Category.Where(n => n.Id == Id).FirstOrDefault();
            return View(lstCategory);
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.loadData();

            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Category objcategory)
        {
            if (ModelState.IsValid)
            {

                this.loadData();
                try
                {
                    if (objcategory.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objcategory.ImageUpload.FileName);
                        //tenhinh
                        string extension = Path.GetExtension(objcategory.ImageUpload.FileName);
                        //png
                        fileName = fileName + extension;
                        //tenhinh.png
                        objcategory.Avatar = fileName;
                        objcategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
                    }
                    objcategory.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanHangEntities.Category.Add(objcategory);
                    objWebsiteBanHangEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objcategory);
        }
        public ActionResult Edit(int Id)
        {
            this.loadData();
            var objcategory = objWebsiteBanHangEntities.Category.Where(n => n.Id == Id).FirstOrDefault();
            return View(objcategory);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Category objcategory)
        {

            if (objcategory.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objcategory.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(objcategory.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objcategory.Avatar = fileName;
                objcategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
            }
            objWebsiteBanHangEntities.Entry(objcategory).State = EntityState.Detached;
            objWebsiteBanHangEntities.Entry(objcategory).State = EntityState.Modified;
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            var objcategory = objWebsiteBanHangEntities.Category.Where(n => n.Id == Id).FirstOrDefault();
            return View(objcategory);
        }
        [HttpPost]
        public ActionResult Delete(Category objCate)
        {
            var objcategory = objWebsiteBanHangEntities.Category.Where(n => n.Id == objCate.Id).FirstOrDefault();
            objWebsiteBanHangEntities.Category.Remove(objcategory);
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