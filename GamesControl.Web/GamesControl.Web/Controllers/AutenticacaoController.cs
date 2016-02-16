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
using GamesControl.Web.Models;
using System.Web.Security;
using GamesControl.Web.Comum;

namespace GamesControl.Web.Controllers
{
    [Authorize]
    public class AutenticacaoController : Controller
    {
        private Contexto db = new Contexto();
        
        [AllowAnonymous]
        public ActionResult Login(string mensagem)
        {
            if (!string.IsNullOrWhiteSpace(mensagem))
            {
                ViewBag.Mensagem = mensagem;
            }
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsuarioLogadoViewModel model)
        {
            var usuarioAutenticado = Autenticacao.AutenticarUsuario(model.Email, model.Senha);

            if (string.IsNullOrWhiteSpace(usuarioAutenticado))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mensagem = usuarioAutenticado;
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ExpirarSessao()
        {
            Autenticacao.Deslogar();
            return RedirectToAction("Login", "Autenticacao", new { mensagem = "Sessão expirada! Logar novamente para continuar utilizando o sistema." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logoff()
        {
            Autenticacao.Deslogar();
            return RedirectToAction("Login", "Autenticacao");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}