using Cosevi.SIBOAC.Security;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new InitializeSimpleMembershipAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
