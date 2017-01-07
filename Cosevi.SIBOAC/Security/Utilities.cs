using Cosevi.SIBOAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Security
{
    public static class Utilities
    {
        public static List<SIBOACMenuOpciones> GetMenuOptions(string userName)
        {
            using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
            {
                var user = sdb.SIBOACUsuarios.Where(a => a.Usuario.Equals(userName)).FirstOrDefault();

                List<int> rolIDs = user.SIBOACRolesDeUsuarios.Select(r => r.IdRol).Distinct().ToList();

                var menuOptions = sdb.SIBOACMenuOpciones.Where(m => m.SIBOACRoles.Any(r => rolIDs.Contains(r.Id) && m.Estado)).ToList();
                return menuOptions;
            }
        }

        public static List<SIBOACMenuOpciones> GetMenuParentOptions(List<int> parentIDs)
        {
            using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
            {
                var menuOptions = sdb.SIBOACMenuOpciones.Where(m => parentIDs.Contains(m.MenuOpcionesID) && m.Estado).ToList();
                return menuOptions;
            }
        }
    }
}