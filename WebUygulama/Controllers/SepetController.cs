using WebUygulama.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebUygulama.Controllers
{
    public class SepetController : Controller
    { 
    UygulamaEntities2 db = new UygulamaEntities2();
    // GET: Sepet
    public ActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.tbl_login.FirstOrDefault(x => x.kul_ad == kullaniciadi);
                var model = db.tbl_sepet.Where(x => x.kullanici_id == kullanici.ID).ToList();
                var kid = db.tbl_sepet.FirstOrDefault(x => x.kullanici_id == kullanici.ID);
                if(model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmuyor";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.tbl_sepet.Where(x => x.kullanici_id == kid.kullanici_id).Sum(x => x.toplam_tutar);                      
                        ViewBag.Tutar = "Toplam Tutar  =" + Tutar + "TL";
                    }
                    return View(model);
                }   
                
            }
            return HttpNotFound();
        }

    public ActionResult SepeteEkle(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.tbl_login.FirstOrDefault(x => x.kul_ad == kullaniciadi);
                var u = db.tbl_urunler.Find(id);
                var sepet = db.tbl_sepet.FirstOrDefault(x => x.kullanici_id == model.ID && x.urun_id == id);
                if (model!= null)
                {
                    if (sepet!=null)
                    {
                        sepet.adet++;
                        sepet.toplam_tutar =u.urun_fiyat * sepet.adet;                      
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new tbl_sepet
                    {                  
                        kullanici_id = model.ID,
                        urun_id = u.urun_id,
                        adet = 1,
                        birim_fiyat = u.urun_fiyat,
                        toplam_tutar = u.urun_fiyat,                         
                        tarih = DateTime.Now,
                        saat = DateTime.Now
                    };
                    db.Entry(s).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            return HttpNotFound();
       }

    public ActionResult TotalCount (int? count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.tbl_login.FirstOrDefault(x => x.kul_ad == User.Identity.Name);
                count = db.tbl_sepet.Where(x => x.kullanici_id == model.ID).Count();
                ViewBag.Count = count;
                if (count == 0)
                {
                    ViewBag.Count = "";
                }

                return PartialView();
            }
            return HttpNotFound();
        }

    public ActionResult Arttir (int? id)
        {
            var model = db.tbl_sepet.Find(id);
            model.adet++;
            model.toplam_tutar = model.birim_fiyat * model.adet * model.tbl_urunler.kdv;          
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    public ActionResult Azalt(int? id)
        {
            var model = db.tbl_sepet.Find(id);
            if (model.adet == 1)
            {
                db.tbl_sepet.Remove(model);
                db.SaveChanges();
            }
            model.adet--;
            model.toplam_tutar = model.birim_fiyat * model.adet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    public ActionResult Sil (int id)
        {
            var model = db.tbl_sepet.Find(id);
            db.tbl_sepet.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.tbl_login.FirstOrDefault(x => x.kul_ad.Equals(kullaniciadi));
                var sil = db.tbl_sepet.Where(x => x.kullanici_id.Equals(model.ID));
                db.tbl_sepet.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}