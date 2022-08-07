using BLL;
using SKFPortal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SKFPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            vmUser vm = new vmUser();
            vm.username = "juan.adm24@gmail.com";
            vm.password = "SKF123";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(vmUser vm)
        {
            SKFBLL skfBLL = new SKFBLL();
            try
            {
                var usuarioSKF = skfBLL.Login(vm.password, vm.username);
                if (usuarioSKF.Email != null)
                {
                    if (!string.IsNullOrEmpty(usuarioSKF.Personal_id))
                    {
                        FormsAuthentication.SetAuthCookie(usuarioSKF.Personal_id, false);
                        Session["UsuarioSKF"] = usuarioSKF;
                        Session["NombreUsuario"] = string.Format("{0}, {1} {2}", usuarioSKF.Nombres, usuarioSKF.Apellido_Paterno, usuarioSKF.Apellido_Materno);
                        return RedirectToAction("Dashboard", "Home");

                    }
                    else
                    {
                        TempData["Message"] = "Usuario no encontrado / Credenciales incorrectas";
                        return View(vm);
                    }
                }
                else
                {
                    TempData["Message"] = "Usuario no encontrado";
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            


        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
    }
}
