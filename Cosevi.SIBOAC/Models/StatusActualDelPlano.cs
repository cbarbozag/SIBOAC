using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Models
{
    public class StatusActualDelPlano
    {
        //public string codigoDelegacion { get; set; }

        public string descripcionDelegacion { get; set; }

        public IEnumerable<string> SelectedDelegaciones { get; set; }

        public IEnumerable<SelectListItem> StatusActual { get; set; }

    }
}