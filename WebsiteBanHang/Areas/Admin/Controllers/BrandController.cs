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
    public class BrandController : Controller
    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();
        // GET: Admin/Brand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            List<Brand> brand;
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
                brand = objWebsiteBanHangEntities.Brand.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                brand = objWebsiteBanHangEntities.Brand.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            brand = brand.OrderByDescending(n => n.Id).ToList();
            return View(brand.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int Id)
        {
            var lstBrand = objWebsiteBanHangEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(lstBrand);
        }
        [HttpGet]
        public ActionResult Create()
        {
            this.loadData();

            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Brand objbrand)
        {
            if (ModelState.IsValid)
            {

                this.loadData();
                try
                {
                    if (objbrand.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objbrand.ImageUpload.FileName);
                        //tenhinh
                        string extension = Path.GetExtension(objbrand.ImageUpload.FileName);
                        //png
                        fileName = fileName + extension;
                        //tenhinh.png
                        objbrand.Avatar = fileName;
                        objbrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
                    }
                    objbrand.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanHangEntities.Brand.Add(objbrand);
                    objWebsiteBanHangEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objbrand);
        }
        public ActionResult Edit(int Id)
        {
            this.loadData();
            var objBrand = objWebsiteBanHangEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Brand objBrand)
        {
           
            if (objBrand.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objBrand.Avatar = fileName;
                objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/thuonghieu"), fileName));
            }
            objWebsiteBanHangEntities.Entry(objBrand).State = EntityState.Detached;
            objWebsiteBanHangEntities.Entry(objBrand).State = EntityState.Modified;
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            var objBrand = objWebsiteBanHangEntities.Brand.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Delete(Brand objBra)
        {
            var objBrands = objWebsiteBanHangEntities.Brand.Where(n => n.Id == objBra.Id).FirstOrDefault();
            objWebsiteBanHangEntities.Brand.Remove(objBrands);
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