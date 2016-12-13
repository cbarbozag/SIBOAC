using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class DescargaParteOficialController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: DescargaParteOficial
        public ActionResult Index()
        {
            var categories = db.DELEGACION.Select(c => new {
                Id = c.Id,
                Descripcion = c.Descripcion
            }).ToList();
            ViewBag.Categories = new MultiSelectList(categories, "Id", "Descripcion", new[] { 1, 13, 14 });
            return View();
        }
    }
}