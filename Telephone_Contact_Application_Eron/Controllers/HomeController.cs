using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telephone_Contact_Application_Eron.Models;

namespace Telephone_Contact_Application_Eron.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            persons.Clear();
            var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KISI_LISTESI";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @""" " + @"""";

            var data = @"{""e_kategori_id"":" + $"{0}" + @",""e_adi_soyadi"":" + $"{str}" + "}"; //böyle olursa seçili olanı getirir id si 0 olursa hepsini getirir.

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            dynamic user = JsonConvert.DeserializeObject<List<dynamic>>(result);

            foreach (var item in user)
            {
                Person b = new Person();
                b.ID = item["e_id"] != null ? Convert.ToInt32(item["e_id"]) : 0;
                b.KategoriAdi = item["e_kategori_adi"];
                b.AdiSoyadi = item["e_adi_soyadi"];
                b.Telefon = item["e_telefon"];
                persons.Add(b);
            }
           
            return View(persons);
        }
        [HttpPost]
        public ActionResult Index(string searchterm)
        {
            List<Person> searchedStudents = new List<Person>();
            var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KISI_LISTESI";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @"""" + $"{searchterm}" + @"""";

            var data = @"{""e_kategori_id"":" + $"{0}" + @",""e_adi_soyadi"":" + $"{str}" + "}"; //search işlemi de eklendi

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            dynamic user = JsonConvert.DeserializeObject<List<dynamic>>(result);

            foreach (var item in user)
            {
                Person b = new Person();
                b.ID = item["e_id"] != null ? Convert.ToInt32(item["e_id"]) : 0;
                b.KategoriAdi = item["e_kategori_adi"];
                b.AdiSoyadi = item["e_adi_soyadi"];
                b.Telefon = item["e_telefon"];
                searchedStudents.Add(b);
            }

           
            return View(searchedStudents);
        }
        public ActionResult Login(string returnUrl = "/Home/Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string nickname,string password)
        {
            var url = "http://eronsoftware.com:55301/KULLANICI/authentication/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "LOGIN";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.ContentType = "text";
            var str = @"""" + $"{nickname}" + @"""";
            var str2 = @"""" + $"{password}" + @"""";
            var data = @"{""e_kullanici_adi"":" + $"{str}" + @",""e_sifre"":" + $"{str2}" + "}";
            

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);
            ViewBag.response = user.UTOKEN;
            UToken = user.UTOKEN;
            ViewBag.mesaj = user.MESAJ;
            if (result.Contains("Kullanıcı Adı - Şifre Hatalı")) // nesne boş ise
            {
                ViewBag.ErrorMessage = "UserName or Password is wrong";
                return View("Login");
            }
            else
            {
            return RedirectToAction("Index");
            }
            
        }

        public ActionResult About()
        {
        ViewBag.Message = "Your application description page.";

        return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static string UToken;
       
        public dynamic APILogout()
        {

            var url = "http://eronsoftware.com:55301/KULLANICI/authentication/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "LOGOUT";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";
            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);
            return user;
        }
      
        static List<Person> persons = new List<Person>();
     
        //public ActionResult PersonList(int id, string name)
        //{
        //    persons.Clear();
        //    var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

        //    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        //    httpRequest.Method = "POST";

        //    httpRequest.Headers["islem"] = "KISI_LISTESI";
        //    httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
        //    httpRequest.Headers["utoken"] = UToken;
        //    httpRequest.ContentType = "text";

        //    var str = @""" " + @"""";
            
        //    var data = @"{""e_kategori_id"":" + $"{0}" + @",""e_adi_soyadi"":" + $"{str}" + "}"; //böyle olursa seçili olanı getirir id si 0 olursa hepsini getirir.
            
        //    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
        //    {
        //        streamWriter.Write(data);
        //    }
        //    var result = "";
        //    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        result = streamReader.ReadToEnd();
        //    }
        //    dynamic user = JsonConvert.DeserializeObject<List<dynamic>>(result);

        //    foreach (var item in user)
        //    {
        //        Person b = new Person();
        //        b.ID = item["e_id"] != null ? Convert.ToInt32(item["e_id"]) : 0;
        //        b.KategoriAdi = item["e_kategori_adi"];
        //        b.AdiSoyadi = item["e_adi_soyadi"];
        //        b.Telefon = item["e_telefon"];
        //        persons.Add(b);
        //    }
        //    ViewBag.Persons = persons;
        //    return View(persons);
        //}
        public ActionResult PersonDelete(int id)
        {
            Person person = persons.Find(c => c.ID == id);
            return View(person);
        }

        [HttpPost]
        public ActionResult PersonDelete(Person person)
        {

            var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KISI_LISTESI_SIL";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var data = @"{""ESKI_ID"":" + $"{person.ID}" + "}";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            var m = user.MESAJ;
            return Redirect("/Home/Index");
        }
        static Person person = new Person();
        public ActionResult PersonAdd()
        {
            List<SelectListItem> categories = kategoris.ToList().OrderBy(n => n.KategoriAdi)
                .Select(n =>
                        new SelectListItem
                        {
                            Value = n.ID.ToString(),
                            Text = n.KategoriAdi
                        }).ToList();
            ViewBag.categories = categories;
            ViewBag.kisi = person;
            return View(person);
        }

        [HttpPost]
        public ActionResult PersonAdd(Person person)
        {
           
            var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KISI_LISTESI_EKLE";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @""" " + $"{person.AdiSoyadi}" + @"""";
            var str2 = @""" " + $"{person.Telefon}" + @"""";
            var data = @"{""e_kategori_id"":" + $"{person.CategoryID}" + @",""e_adi_soyadi"":" + $"{str}" + @",""e_telefon"":" + $"{str2}" + "}";
            //{ "e_kategori_adi":"kategori.KategoriAdi"};

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            ViewBag.m = user.MESAJ;
           
            
            return Redirect("/Home/Index");
        }
        public ActionResult PersonEdit(int id)
        {
            
            List<SelectListItem> categories = kategoris.ToList().OrderBy(n => n.KategoriAdi)
               .Select(n =>
                       new SelectListItem
                       {
                           Value = n.ID.ToString(),
                           Text = n.KategoriAdi
                       }).ToList();
            ViewBag.categories = categories;
            Person person = persons.Find(c => c.ID == id);
            return View(person);
        }

        [HttpPost]
        public ActionResult PersonEdit(Person person)
        {
           
            var url = "http://eronsoftware.com:55301/KULLANICI/kisi/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";


            httpRequest.Headers["islem"] = "KISI_LISTESI_DUZENLE";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @""" " + $"{person.AdiSoyadi}" + @"""";

            var str2 = @""" " + $"{person.Telefon}" + @"""";
            var data = @"{""e_kategori_id"":" + $"{person.CategoryID}" + @",""e_adi_soyadi"":" + $"{str}" + @",""e_telefon"":" + $"{str2}" + @",""e_telefon"":" + $"{person.ID}" + "}";


            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            ViewBag.m = user.MESAJ;
            return Redirect("/Home/Index");
        }

        #region category list
        static List<Kategori> kategoris = new List<Kategori>();
        public ActionResult CategoryList()
        {
             kategoris.Clear();
            var url = "http://eronsoftware.com:55301/KULLANICI/kategori/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KATEGORI_LISTESI";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            dynamic user = JsonConvert.DeserializeObject<List<dynamic>>(result);

            foreach (var item in user)
            {
                Kategori a = new Kategori();
                a.ID = item["e_id"] != null ? Convert.ToInt32(item["e_id"]) : 0;
                a.KategoriAdi = item["e_kategori_adi"];
                kategoris.Add(a);
            }
           
            return View(kategoris);
        }

        #endregion 
        public ActionResult CategoryDelete(int id)
        {
            Kategori kategori = kategoris.Find(c => c.ID == id);
            return View(kategori);
        }

        [HttpPost]
        public ActionResult CategoryDelete(Kategori kategori)
        {
           
            var url = "http://eronsoftware.com:55301/KULLANICI/kategori/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KATEGORI_LISTESI_SIL";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var data = @"{""ESKI_ID"":" + $"{kategori.ID}"+"}";
            
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            var m = user.MESAJ;
            return Redirect("/Home/Index");
        }
        static Kategori kategori = new Kategori();
        public ActionResult CategoryAdd()
        {
            return View(kategori);
        }

        [HttpPost]
        public ActionResult CategoryAdd(Kategori kategori)
        {

            var url = "http://eronsoftware.com:55301/KULLANICI/kategori/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KATEGORI_LISTESI_EKLE";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @""" " + $"{kategori.KategoriAdi}" + @"""";
            var data = @"{""e_kategori_adi"":"+$"{str}"+"}";
            //{ "e_kategori_adi":"kategori.KategoriAdi"};

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            ViewBag. m = user.MESAJ;
            return Redirect("/Home/Index");
        }
        public ActionResult CategoryEdit(int id)
        {
            Kategori kategori = kategoris.Find(c => c.ID == id);
            return View(kategori);
        }

        [HttpPost]
        public ActionResult CategoryEdit(Kategori kategori)
        {

            var url = "http://eronsoftware.com:55301/KULLANICI/kategori/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";


            httpRequest.Headers["islem"] = "KATEGORI_LISTESI_DUZENLE";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var str = @""" " + $"{kategori.KategoriAdi}" + @"""";

            var data = @"{""ESKI_ID"":" + $"{kategori.ID}" + @",""e_kategori_adi"":" + $"{str}" + "}";


            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var result = "";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            result = result.Trim('[');
            result = result.Trim(']');
            dynamic user = JsonConvert.DeserializeObject(result);

            ViewBag.m = user.MESAJ;
            return Redirect("/Home/Index");
        
        }
    }
   
}
