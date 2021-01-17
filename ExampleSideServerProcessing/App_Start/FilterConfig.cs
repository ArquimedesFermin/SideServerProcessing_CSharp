using System.Web;
using System.Web.Mvc;

namespace ExampleSideServerProcessing
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
