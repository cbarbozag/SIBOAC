using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            var model = TempData["ExceptionHandleErrorInfo"] as HandleErrorInfo;
            return View(model);
        }
    }
}