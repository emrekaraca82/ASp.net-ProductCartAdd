using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebUygulama.Models.Entity;
using Excel = Microsoft.Office.Interop.Excel;
namespace WebUygulama.Controllers
{
   // [Authorize]
    public class UrunlerController : Controller
    {
        UygulamaEntities2 db = new UygulamaEntities2();

        // GET: Urunler
        public ActionResult Index(string u)
        {
            var urunler = from d in db.tbl_urunler select d;
            if(!string.IsNullOrEmpty(u))
            {
                urunler = urunler.Where(x => x.urun_barkod.Contains(u));
            }
            return View(urunler.ToList());
           // var listele = db.tbl_urunler.ToList();
           // return View(listele);
        }

        [HttpGet]
        public ActionResult UrunEkle()
        {
        
            List<SelectListItem> listele = (from i in db.tbl_kategori.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.kategori_adi,
                                                Value = i.kategori_id.ToString()
                                            }).ToList();
            ViewBag.lst = listele;
            return View ();
        }

        [HttpPost]
        public ActionResult UrunEkle(tbl_urunler u1) 
        {      
            var urn = db.tbl_kategori.Where(x => x.kategori_id == u1.tbl_kategori.kategori_id).FirstOrDefault();
            u1.tbl_kategori = urn;
            db.tbl_urunler.Add(u1);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sil (int id)
        {
            var urun = db.tbl_urunler.Find(id);          
            db.tbl_urunler.Remove(urun);         
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExcelExport()
        {
            try
            {
                var liste = db.tbl_urunler.ToList();
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "Barkod No";
                worksheet.Cells[1, 3] = "Urun Adi";
                worksheet.Cells[1, 4] = "Kategori";
                worksheet.Cells[1, 5] = "Fiyat";
                worksheet.Cells[1, 6] = "Stok";
                worksheet.Cells[1, 7] = "KDV";

                int row = 2;
                foreach (var item in liste)
                {
                    worksheet.Cells[row, 1] = item.urun_id;
                    worksheet.Cells[row, 2] = item.urun_barkod;
                    worksheet.Cells[row, 3] = item.urun_adi;
                    worksheet.Cells[row, 4] = item.tbl_kategori.kategori_adi;
                    worksheet.Cells[row, 5] = item.urun_fiyat;
                    worksheet.Cells[row, 6] = item.urun_stok;
                    worksheet.Cells[row, 7] = item.kdv;
                    row++;
                }

                workbook.SaveAs("d:\\Urunler.xlsx");
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