using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

namespace ApiClient.Controllers
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

            var data = @"{""e_kullanici_adi"":""networkAkademi029"",""e_sifre"":""sifre029""}";

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
        public ActionResult ApiList()
        {
            List<Kategori> list = new List<Kategori>();
            var url = "http://eronsoftware.com:55301/KULLANICI/kategori/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["islem"] = "KATEGORI_LISTESI";
            httpRequest.Headers["ptoken"] = "OPp60lBs9vqqNiAvzM2QPsgVuzHvld4ZShVGqlYqEcEgi2BGFt";
            httpRequest.Headers["utoken"] = UToken;
            httpRequest.ContentType = "text";

            var data = "";

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
            //dynamic user = JsonConvert.DeserializeObject(result);
            JObject user = JsonConvert.DeserializeObject(result) as JObject;
          

            foreach (var item in user)
            {
                Kategori a = new Kategori();
                a.ID = Convert.ToInt32(item["e_id"]);
                a.KategoriAdi = item["e_kategori_adi"].ToString();
                list.Add(a);
            }
            return View(list);
        }
    }
}
