using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReportePorDescargaDeParteOficialController : Controller
    {
        [SessionExpire]
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View();
        }
    }
}