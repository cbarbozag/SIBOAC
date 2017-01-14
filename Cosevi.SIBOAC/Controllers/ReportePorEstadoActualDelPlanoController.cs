using Cosevi.SIBOAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReportePorEstadoActualDelPlanoController : Controller
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