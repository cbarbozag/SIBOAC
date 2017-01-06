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

                int rolID = user.SIBOACRolesDeUsuarios.FirstOrDefault().IdRol;

                var menuOptions = sdb.SIBOACMenuOpciones.Where(m => m.SIBOACRoles.Any(r => r.Id == rolID) && m.Estado).ToList();
                return menuOptions;
            }
        }
    }
}