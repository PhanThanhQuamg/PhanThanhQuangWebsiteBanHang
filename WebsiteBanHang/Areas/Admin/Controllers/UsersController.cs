using Newtonsoft.Json.Bson;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using static WebsiteBanHang.Common;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        WebsiteBanHangEntities objWebsiteBanHangEntities = new WebsiteBanHangEntities();
        // GET: Admin/Users
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            List<Users> users;
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
                users = objWebsiteBanHangEntities.Users.Where(p => p.FirstName.Contains(SearchString)).ToList();
            }
            else
            {
                users = objWebsiteBanHangEntities.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            users = users.OrderByDescending(n => n.Id).ToList();
            return View(users.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(int Id)
        {
            var lstUsers = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(lstUsers);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Users objusers)
        {
            if (ModelState.IsValid)
            {
                var check = objWebsiteBanHangEntities.Users.FirstOrDefault(s => s.Email == objusers.Email);
                if (check == null)
                {
                    objusers.Password = GetMD5(objusers.Password);
                    objusers.IsAdmin = false; //IsAdmin = false = người dùng
                    objWebsiteBanHangEntities.Configuration.ValidateOnSaveEnabled = false;
                    // add user
                    objWebsiteBanHangEntities.Users.Add(objusers);
                    //lưu thông tin lại
                    objWebsiteBanHangEntities.SaveChanges();
                    // trả về trang chủ
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Edit(int Id)
        {
            
            var objusers = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
            return View(objusers);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Users objusers)
        {

        
            objWebsiteBanHangEntities.Entry(objusers).State = EntityState.Modified;
            objWebsiteBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            var objusers = objWebsiteBanHangEntities.Users.Where(n => n.Id == Id).FirstOrDefault();
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
      
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
  
}