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
            var response = APILogin();
            ViewBag.response = response.UTOKEN;
            UToken = response.UTOKEN;
            ViewBag.mesaj = response.MESAJ;
            return View(response);
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


        public dynamic APILogin()
        {

            var url = "http://eronsoftware.com:55301/KULLANICI/authentication/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "LOGIN";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.ContentType = "text";

            var data = @"{""e_kullanici_adi"":""networkAkademi028"",""e_sifre"":""sifre028""}";

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
            return user;
        } 
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

        public ActionResult CategoryAdd()
        {

           
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

            var data = @"{""e_kategori_adi"":" + $"{kategori.KategoriAdi}" + "}";

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
    }
}
