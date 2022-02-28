using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telephone_Contact_Application_Eron.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string AdiSoyadi { get; set; }
        public string Telefon { get; set; }
        public Kategori kategori { get; set; }
        public int CategoryID { get; set; }
        public string KategoriAdi { get; set; }
    }
}