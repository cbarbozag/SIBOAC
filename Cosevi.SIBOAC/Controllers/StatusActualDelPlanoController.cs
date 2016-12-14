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
    public class StatusActualDelPlanoController : Controller
    {
        String valorRadioButton = "";

        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: StatusActualDelPlano

        [HttpGet]
        public ActionResult Index()
        {
            PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            foreach (Delegacion itemDelegacion in db.DELEGACION)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = itemDelegacion.Descripcion
                };

                listSelectListItems.Add(selectList);
            }

            StatusActualDelPlano statusActualModel = new StatusActualDelPlano()
            {
                StatusActual = listSelectListItems
            };

            return View(statusActualModel);
            
        }

        [HttpPost]
        public string Index(IEnumerable<string> SelectedDelegaciones)
        {
            ViewBag.Valor = null;

            if (SelectedDelegaciones == null)
            {
                return "No hay delegaciones seleccionadas";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append ("You selected – " + string.Join (",", SelectedDelegaciones));
                return sb.ToString();
            }
        }



        //[HttpGet]
        //public ActionResult _PlanoActualView()
        //{
        //    PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        //    List<SelectListItem> listSelectListItems2 = new List<SelectListItem>();

        //    foreach (Delegacion itemDelegacion2 in db.DELEGACION)
        //    {
        //        SelectListItem selectList = new SelectListItem()
        //        {
        //            Text = itemDelegacion2.Descripcion
        //        };

        //        listSelectListItems2.Add(selectList);
        //    }

        //    StatusActualDelPlano statusActualModel2 = new StatusActualDelPlano()
        //    {
        //        Autoridades = listSelectListItems2
        //    };

        //    return View(statusActualModel2);

        //}




        public ViewResult obtenerRadiobutton(String estadoPlano)
        {

            if (estadoPlano != null)

            {
                valorRadioButton = estadoPlano;
                //guardar el valor para después hacer un filtro con todos los datos

            }

            else
            {
                //debe seleccionar una opción

            }

            //var elementos = document.getElementsByName("estadoPlano");

            //for (var i = 0; i < elementos.length; i++)
            //{
            //    alert(" Elemento: " + elementos[i].value + "\n Seleccionado: " + elementos[i].checked);
            //}

            return View();
        }

        //public ActionResult llenarListaAutoridadesJudiciales()
        //{ }

    }
}