using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReportePorUsuarioController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: ReportePorUsuario
        public ActionResult Index()
        {
            
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();

            foreach (ParteOficial parteOfi in db.PARTEOFICIAL)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    //Text = city.Name,
                    //Value = city.ID.ToString(),
                    //Selected = city.IsSelected
                    //Value = parteOfi.NumeroParte,
                    
                    Text = parteOfi.usuario_entregaPlano
                };
                listSelectListItems.Add(selectList);
            }

            ParteOficial parteoficial = new ParteOficial()
            {
                UsuariosLis = listSelectListItems
            };

            return View(parteoficial);
        }

        [HttpPost]
        public string Index(IEnumerable<string> SelectedUsuario)
        {
            if (SelectedUsuario == null)
            {
                return "No cities are selected";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("You selected – " +string.Join(",", SelectedUsuario));
                return sb.ToString();
            }
        }
    }
}