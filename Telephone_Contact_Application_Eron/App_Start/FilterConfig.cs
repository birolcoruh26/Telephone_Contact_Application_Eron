using System.Web;
using System.Web.Mvc;

namespace Telephone_Contact_Application_Eron
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
