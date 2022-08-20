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
    public class ProductController : Controller

    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();

        // GET: Admin/Product
        public ActionResult Index (string SearchString)
        {
            List<Product>  product;

            if (!string.IsNullOrEmpty(SearchString))

            {
                product = objWebsiteBanHangEntities.Product.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                product = objWebsiteBanHangEntities.Product.ToList();
            }
            return View(product);
        }
        public ActionResult Create()
        {
            //Common objCommon = new Common();
            //// Láy dữ liệu danh mục dưới DB
            //var lstCat  = objWebsiteBanHangEntities.Category.ToList();
            //// Convert sang select list dạng value , text
            //ListtoDataTableConverter converter = new ListtoDataTableConverter ();
            //DataTable dtCategory = converter.ToDataTable(lstCat);
            //ViewBag.ListBrand =  objCommon.ToSelectList(dtCategory, "Id", "Name");

            ////Lấy dữ liệu thương hiệu dưới  DB
            //var lstBrand = objWebsiteBanHangEntities.Brand.ToList();
            //DataTable dtBrand = converter.ToDataTable(lstBrand);
            ////convert sang select list  dạng value , text 
            //ViewBag.ListBrand = objCommon.ToSelectList(dtBrand,"Id", "Name");

            this.loadData();

            return View();
        }
        [ValidateInput(false)]
        [HttpPost]

        public ActionResult Create(Product objProduct)
        {

            if (ModelState.IsValid)
            {

                this.loadData();
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        //tenhinh
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                        //png
                        fileName = fileName + extension;
                        //tenhinh.png
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
                    }
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanHangEntities.Product.Add(objProduct);
                    objWebsiteBanHangEntities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objProduct);

        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            var objProduct = objWebsiteBanHangEntities.Product.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objProduct = objWebsiteBanHangEntities.Product.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objWebsiteBanHangEntities.Product.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objWebsiteBanHangEntities.Product.Remove(objProduct);
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objProduct = objWebsiteBanHangEntities.Product.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Edit( Product objProduct)
        {

            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
            }
            objWebsiteBanHangEntities.Entry(objProduct).State=EntityState.Modified;
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