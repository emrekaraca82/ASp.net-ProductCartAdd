using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using WebUygulama.Models.Entity;

namespace WebUygulama.Controllers
{
    public class SatislarController : Controller
    {
        UygulamaEntities2 db = new UygulamaEntities2();
        // GET: Satislar
        public ActionResult Index()
        {
            var model = db.tbl_satis.ToList();
            return View(model);
        }

        public ActionResult SatinAl(int id)
        {
            var model = db.tbl_sepet.FirstOrDefault(x => x.ID == id);
            return View(model);

        }

        [HttpPost]

        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.tbl_sepet.FirstOrDefault(x => x.ID == id);
                    var satis = new tbl_satis
                    {
                        kullanici_id = model.kullanici_id,
                        urun_id = model.urun_id,
                        sepet_id = model.ID,
                        //barkod_no = model.tbl_urunler.urun_barkod,
                        birim_fiyat = model.birim_fiyat,
                        adet = model.adet,
                        toplam_tutar = model.toplam_tutar,
                        kdv = model.tbl_urunler.kdv,
                        tarih = DateTime.Now
                    };
                    db.tbl_sepet.Remove(model);
                    db.tbl_satis.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satin Alma İşlemi Başarlıyala Gerçekleşti";
                }
            }
            catch (Exception)
            {

                ViewBag.islem = "Satin Alma İşlemi Başarısız";
            }


            return View("islem");
        }

        public ActionResult Sil(int id)
        {
            var sts = db.tbl_satis.Find(id);
            db.tbl_satis.Remove(sts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.tbl_login.FirstOrDefault(x => x.kul_ad == kullaniciadi);
                var model = db.tbl_sepet.Where(x => x.kullanici_id == kullanici.ID).ToList();
                var kid = db.tbl_sepet.FirstOrDefault(x => x.kullanici_id == kullanici.ID);
                if (model != null)
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

        public ActionResult HepsiniSatinAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.tbl_login.FirstOrDefault(x => x.kul_ad == username);
            var model = db.tbl_sepet.Where(x => x.kullanici_id == kullanici.ID).ToList();

            int row = 0;

            foreach (var item in model)
            {
                var satis = new tbl_satis
                {
                    kullanici_id = model[row].kullanici_id,
                    urun_id = model[row].urun_id,
                    sepet_id = model[row].ID,
                    barkod_no = model[row].tbl_urunler.urun_barkod,
                    adet = model[row].adet,
                    birim_fiyat = model[row].birim_fiyat,
                    toplam_tutar = model[row].toplam_tutar,
                    tarih = DateTime.Now
                };
                db.tbl_satis.Add(satis);
                row++;

            }

            db.tbl_sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }

        public JsonResult ExcelExport()
        {
            try
            {
                var liste = db.tbl_satis.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Kullanici Adi";
                worksheet.Cells[1, 3] = "Urun Adi";
                worksheet.Cells[1, 4] = "Barkod No";
                worksheet.Cells[1, 5] = "Fiyat";
                worksheet.Cells[1, 6] = "Adet";
                worksheet.Cells[1, 7] = "KDV";
                worksheet.Cells[1, 8] = "Tarih";

                int row = 2;
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.ID;
                    worksheet.Cells[row, 2] = item.tbl_login.kul_ad;
                    worksheet.Cells[row, 3] = item.tbl_urunler.urun_adi;
                    worksheet.Cells[row, 4] = item.tbl_urunler.urun_barkod;
                    worksheet.Cells[row, 5] = item.birim_fiyat;
                    worksheet.Cells[row, 6] = item.adet;
                    worksheet.Cells[row, 7] = item.kdv;
                    worksheet.Cells[row, 8] = item.tarih;
                    row++;
                }

                workbook.SaveAs("d:\\Satislar.xlsx");
                workbook.Close();
                application.Quit();

                ViewBag.mesaj = "Başarlı";
            }
            catch (Exception ex)
            {

                ViewBag.mesaj = ex.Message;
            }
            return Json(ViewBag.mesaj, JsonRequestBehavior.AllowGet);
        }
    }
}