using Cosevi.SIBOAC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        public PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        public void Bitacora(T entidadNueva, string operacion, string nombreTabla, T entidadAnterior = null)
        {
            string nombreUsuario = TempData["nombreUsuario"].ToString();

            if (operacion.ToLower() == "i")
            {
                db.BitacoraSIBOAC.Add(new BitacoraSIBOAC()
                {
                    CodigoUsuario = nombreUsuario,
                    FechaHora = DateTime.Now,
                    NombreTabla = nombreTabla,
                    Operacion = operacion,
                    ValorDespues = GetValues(entidadNueva)
                });
            }
            else
            {
                db.BitacoraSIBOAC.Add(new BitacoraSIBOAC()
                {
                    CodigoUsuario = nombreUsuario,
                    FechaHora = DateTime.Now,
                    NombreTabla = nombreTabla,
                    Operacion = operacion,
                    ValorAntes = GetValues(entidadAnterior),
                    ValorDespues = GetValues(entidadNueva)
                });
            }
            db.SaveChanges();
        }

        private string GetValues(T entidad)
        {
            if (entidad == null)
            {
                return null;
            }

            Type type = typeof(T);

            List<string> values = new List<string>();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                string name = propertyInfo.Name;

                object value = propertyInfo.GetValue(entidad, null);

                values.Add(String.Format("{0}:{1}", name, value));
            }

            return string.Join(",", values);
        }

        public T ObtenerCopia(T entidad)
        {
            string cloneString = JsonConvert.SerializeObject(entidad);
            T clone = JsonConvert.DeserializeObject<T>(cloneString);
            return clone;
        }
    }
}