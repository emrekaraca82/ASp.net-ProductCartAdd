//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebUygulama.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_sepet
    {
        public int ID { get; set; }
        public Nullable<int> kullanici_id { get; set; }
        public Nullable<int> urun_id { get; set; }
        public Nullable<decimal> birim_fiyat { get; set; }
        public Nullable<decimal> adet { get; set; }
        public Nullable<decimal> toplam_tutar { get; set; }
        public System.DateTime tarih { get; set; }
        public System.DateTime saat { get; set; }
    
        public virtual tbl_login tbl_login { get; set; }
        public virtual tbl_urunler tbl_urunler { get; set; }
    }
}