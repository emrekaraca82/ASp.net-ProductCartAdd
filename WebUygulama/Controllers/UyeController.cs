using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUygulama.Models.Entity;
using System.Web.Security;

namespace WebUygulama.Controllers
{
    [AllowAnonymous]
    public class UyeController : Controller
    {
        UygulamaEntities2 db = new UygulamaEntities2();
        // GET: Uye
        [HttpGet]
        public ActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(tbl_login log)
        {
            var login = db.tbl_login.FirstOrDefault(x => x.kul_ad == log.kul_ad && x.kul_password == log.kul_password);
            if(login!=null)
            {
                FormsAuthentication.SetAuthCookie(log.kul_ad, false);
                return RedirectToAction("UrunEkle", "Urunler");
            }
            ViewBag.hata = "Kullanici adi yada şifre hatalı";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("GirisYap");
        }

    }
}