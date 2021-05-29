using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUygulama.Models.Entity;

namespace WebUygulama.Controllers
{
   // [Authorize]
    public class KategoriController : Controller
    {
        // GET: Kategori
        UygulamaEntities2 db = new UygulamaEntities2();
        public ActionResult Index()
        {
            var listele = db.tbl_kategori.ToList();
            return View(listele);
        }

        [HttpGet]
        public ActionResult YeniKategori()
        {
            return View();
        }


        [HttpPost]
        public ActionResult YeniKategori(tbl_kategori k1)
        {
            db.tbl_kategori.Add(k1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sil (int id)
        {
            var kategori = db.tbl_kategori.Find(id);
            db.tbl_kategori.Remove(kategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}