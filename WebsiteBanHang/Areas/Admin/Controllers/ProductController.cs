using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebsiteBanHangEntities dbObj = new WebsiteBanHangEntities();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var lstProduct = dbObj.Product.ToList();
            return View(lstProduct);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //this.loadData();
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
                    dbObj.Product.Add(objProduct);
                    dbObj.SaveChanges();

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
        public ActionResult Details(int id)
        {
            var objProduct = dbObj.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]

        public ActionResult Delete(int id)
        {
            var objProduct = dbObj.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]

        public ActionResult Delete(Product objPro)
        {
            var objProduct = dbObj.Product.Where(n => n.Id == objPro.Id).FirstOrDefault();
            dbObj.Product.Remove(objProduct);
            dbObj.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]

        public ActionResult Edit(int id)
        {
            var objProduct = dbObj.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Edit(int id, Product objProduct)
        {
                if(objProduct.ImageUpload != null)
                {
                    String fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    String extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objProduct.Avatar = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images"), fileName));
                 }
                dbObj.Entry(objProduct).State = EntityState.Modified;
                dbObj.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}