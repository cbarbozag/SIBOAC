using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Cosevi.SIBOAC.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Cosevi.SIBOAC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
                   

            if (ModelState.IsValid)
            {
                var isValidUser = Membership.ValidateUser(model.Usuario, model.Contrasena);
                if (isValidUser)
                {
                    Session["username"] = model.Usuario;

                    FormsAuthentication.SetAuthCookie(model.Usuario, model.Recordarme);
                    //if (Url.IsLocalUrl(returnUrl))
                    //{
                    //    return Redirect(returnUrl);
                    //}
                    using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
                    {
                        var user = sdb.SIBOACUsuarios.Where(a => a.Usuario.ToLower().Equals(model.Usuario.ToLower())).FirstOrDefault();

                        DateTime oldDate = user.FechaDeActualizacionClave;
                        DateTime newDate = DateTime.Now;

                        // Difference in days, hours, and minutes.
                        TimeSpan ts = newDate - oldDate;

                        // Difference in days.
                        int diferenciaDias = ts.Days;
                        int diasExpira = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToExpire"]);
                        if (user.Activo == true)
                        { 
                            if (model.Usuario == model.Contrasena)
                            {                                
                                    return RedirectToAction("ResetPassword", "Account", new { code = model.Usuario });                                                            
                            }
                            else
                            {
                                if (diferenciaDias >= diasExpira)
                                {
                                    
                                    return RedirectToAction("ResetPassword", "Account", new { code = model.Usuario });
                                }
                                // Actualizar campo de ultimo acceso al sistema
                                user.UltimoIngreso = DateTime.Now;
                                sdb.Entry(user).State = EntityState.Modified;
                                sdb.SaveChanges();

                                return RedirectToAction("Index", "Home");
                            }

                        }
                        else
                        {
                            user = null;
                            ModelState.Remove("Password");
                            ModelState.AddModelError("", "¡El usuario no esta activo!");
                           
                            return View();
                        }
                    }
                }
            }
            ModelState.Remove("Password");
            ModelState.AddModelError("", "¡Usuario o contraseña invalidos!");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        private string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        private void SendEMail(string emailid, string subject, string body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(emailid);
            msg.From = new MailAddress("frogreportesdospinos@gmail.com");
            msg.Sender = new MailAddress("frogreportesdospinos@gmail.com");
          

            string strHostName = string.Empty;
            strHostName = Dns.GetHostName();

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = Convert.ToInt32("587");//587;//25;

            NetworkCredential credentials = new NetworkCredential("frogreportesdospinos@gmail.com", "FrOgDoSpInOs_123");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            client.Send(msg);

            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;
            //client.Host = "smtp.gmail.com";
            //client.Port = 587;


            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("frogreportesdospinos@gmail.com", "FrOgDoSpInOs_123");
            //client.UseDefaultCredentials = false;
            //client.Credentials = credentials;

            //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //msg.From = new MailAddress("frogreportesdospinos@gmail.com");
            //msg.To.Add(new MailAddress(emailid));

            //msg.Subject = subject;
            //msg.IsBodyHtml = true;
            //msg.Body = body;

            //client.Send(msg);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            
            using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
            {
                var user = sdb.SIBOACUsuarios.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                //var user = await UserManager.FindByNameAsync(model.Code);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    //return RedirectToAction("ResetPasswordConfirmation", "Account");
                    ModelState.AddModelError("", "¡El correo electronico que has indicado no pertenece a ninguna cuenta!");
                    return View();
                }else
                {
                    try
                    {
                        
                        string newpassword = GenerateRandomPassword(6);
                        
                        //get user emailid                        
                        var emailid = (from i in dbs.SIBOACUsuarios
                                       where i.Usuario == user.Usuario
                                       select i.Email).FirstOrDefault();
                        //send email
                        string subject = "Nueva Contraseña";
                        string body = "<b>Bienvenido(a): </b>"+user.Nombre+ "<br/><br/> Hemos recibido una petición de nueva contraseña del usuario: " + user.Usuario+ "<br/>Nueva contraseña: <br/><br/>" + newpassword; //edit it
                        try
                        {
                            SendEMail(emailid, subject, body);
                            TempData["Message"] = "Email enviado!";
                            ViewBag.Type = "success";
                            ViewBag.Message = "Email enviado!";

                            user.Contrasena = newpassword;
                            sdb.Entry(user).State = EntityState.Modified;
                            sdb.SaveChanges();

                            return RedirectToAction("ForgotPasswordConfirmation", "Account");
                        }
                        catch (Exception ex)
                        {
                            TempData["Message"] = "Ocurrió un problema mientras se enviaba el email." + ex.Message;
                            ViewBag.Type = "warning";
                            ViewBag.Message = "Ocurrió un problema mientras se enviaba el email" + "("+ex.Message+")";
                            return RedirectToAction("Login", "Account");                    
                        }                        
                        
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }                

                return View();
            }


        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword(string code)
        {
            return code == null ? View("Error") : View();
            //return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordAViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
            {
                var user = sdb.SIBOACUsuarios.Where(a => a.Usuario.Equals(model.Code)).FirstOrDefault();
                //var user = await UserManager.FindByNameAsync(model.Code);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    //return RedirectToAction("ResetPasswordConfirmation", "Account");
                    return View();
                }
                //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id.ToString());
                try
                {
                    //IdentityResult result = await this.UserManager.ResetPasswordAsync(user.Id.ToString(), model.Code, model.Password);
                    //  SIBOACUsuarios usuarioModificado = new SIBOACUsuarios();
                    if (model.OldPassword == user.Contrasena)
                    {
                        if (model.Code == model.NewPassword)
                        {
                            ModelState.AddModelError("", "¡La contraseña no puede ser igual al usuario!");
                            return View();
                        }
                        if (user.Contrasena == model.NewPassword)
                        {
                            ModelState.AddModelError("", "¡La contraseña no puede ser igual a la actual!");
                            return View();
                        }

                        user.Id = user.Id;
                        user.Usuario = user.Usuario;
                        user.Email = user.Email;
                        user.Contrasena = model.NewPassword;
                        user.Nombre = user.Nombre;
                        user.codigo = user.codigo;
                        user.FechaDeActualizacionClave = DateTime.Now;
                        user.UltimoIngreso = DateTime.Now;
                        user.Activo = user.Activo;

                        sdb.Entry(user).State = EntityState.Modified;
                        sdb.SaveChanges();

                        return RedirectToAction("ChangePasswordConfirmation", "Account");
                    }
                    else
                    {

                        ModelState.AddModelError("", "¡La contraseña actual no corresponde a la del usuario!");
                        return View();
                    }

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                return View();
            }
        }

        //
        // GET: /Account/ChangePasswordAConfirmation
        [AllowAnonymous]
        public ActionResult ChangePasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
            //return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (SIBOACSecurityEntities sdb = new SIBOACSecurityEntities())
            {
                var user = sdb.SIBOACUsuarios.Where(a => a.Usuario.Equals(model.Code)).FirstOrDefault();
                //var user = await UserManager.FindByNameAsync(model.Code);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    //return RedirectToAction("ResetPasswordConfirmation", "Account");
                    return View();
                }
                //string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id.ToString());
                try
                {
                    //IdentityResult result = await this.UserManager.ResetPasswordAsync(user.Id.ToString(), model.Code, model.Password);
                    //  SIBOACUsuarios usuarioModificado = new SIBOACUsuarios();
                    if (model.Password != user.Contrasena)
                    {
                        if(model.Code == model.Password)
                        {
                            ModelState.AddModelError("", "¡La contraseña no puede ser igual a la anterior o igual al usuario!");
                            return View();
                        }

                        user.Id = user.Id;
                        user.Usuario = user.Usuario;
                        user.Email = user.Email;
                        user.Contrasena = model.Password;
                        user.Nombre = user.Nombre;
                        user.codigo = user.codigo;
                        user.FechaDeActualizacionClave = DateTime.Now;
                        user.UltimoIngreso = DateTime.Now;
                        user.Activo = user.Activo;

                        sdb.Entry(user).State = EntityState.Modified;
                        sdb.SaveChanges();

                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    else
                    {                       

                        ModelState.AddModelError("", "¡La contraseña no puede ser igual a la anterior o igual al usuario!");
                        return View();
                    }
                    
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            return View();
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}