﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers
{
    //[Authorize(Roles = "SuperAdministrador,Administrador,Oficial,GeneradorDeReportes,Digitador")]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(string tipo)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            if (tipo != null)               
                ViewBag.opcion = tipo;
            else
                ViewBag.opcion = null;
            ViewBag.HomeImagePath = ConfigurationManager.AppSettings["HomeImagePath"].ToString();
            return View();
        }

        public ActionResult Denied()
        {
            TempData["Type"] = "error";
            TempData["Message"] = "No tiene el rol indicado para acceder a esa funcionalidad.";
            return RedirectToAction("Index");
        }
    }
}