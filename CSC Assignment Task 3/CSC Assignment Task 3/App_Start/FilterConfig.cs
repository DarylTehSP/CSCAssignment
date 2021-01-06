using System.Web;
using System.Web.Mvc;

namespace CSC_Assignment_Task_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new RequireHttpsAttribute());
        }
    }
}
