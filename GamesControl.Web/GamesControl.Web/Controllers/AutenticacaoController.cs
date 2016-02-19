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
using System.Data.Entity;
using System.IO;

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

        [AllowAnonymous]
        public ActionResult SolicitarAcesso()
        {
            ViewBag.Perfil = new SelectList
            (
                db.tbPerfil.Where(x => x.perfilId != (int)Enuns.ePerfilUsuario.Administrador).OrderBy(p => p.perfilDescricao),
                "perfilId",
                "perfilDescricao"
            );

            ViewBag.Cidade = new SelectList
            (
                db.tbCidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome"
            );

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SolicitarAcesso(string usuarioNome, string usuarioEmail, string usuarioSenha, string usuarioTelefone, string[] usuarioPerfil, string jogadorDataNascimento, int? usuarioCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                var usuarioExistente = db.tbUsuario.FirstOrDefault(x => x.usuarioEmail.Trim() == usuarioEmail);
                if (usuarioExistente != null)
                {
                    if (usuarioExistente.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.OK)
                    {
                        return RedirectToAction("Login", "Autenticacao", new { mensagem = "Você já possui um usuário com acesso ao sistema! Informe login e senha para acessá-lo." });
                    }

                    if (usuarioExistente.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.Negado)
                    {
                        return RedirectToAction("Login", "Autenticacao", new { mensagem = "Você já possui uma solicitação de acesso que foi negada! Entre em contato com os administradores do sistema para verificar o ocorrido." });
                    }

                    if (usuarioExistente.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.PendenteAprovacao)
                    {
                        return RedirectToAction("Login", "Autenticacao", new { mensagem = "Sua solicitação de acesso ainda está pendente de autorização." });
                    }

                    if (usuarioExistente.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.Inativo || usuarioExistente.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.Banido)
                    {
                        return RedirectToAction("Login", "Autenticacao", new { mensagem = "Você já possui um usuário inativo no sistema. Entre em contato com os administradores." });
                    }                    
                }

                tbUsuario usuario = new tbUsuario();
                usuario.usuarioEmail = usuarioEmail;
                usuario.usuarioNome = usuarioNome;
                usuario.usuarioSenha = usuarioSenha;

                if (usuarioTelefone == "() -")
                {
                    usuarioTelefone = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(usuarioTelefone))
                {
                    usuario.usuarioTelefone = usuarioTelefone;
                }

                usuario.tbUsuarioStatus = db.tbUsuarioStatus.Find((int)Enuns.eStatusUsuario.PendenteAprovacao);

                bool possuiJogador = false;
                bool possuiArbitro = false;
                foreach (string idPerfil in usuarioPerfil)
                {
                    if (int.Parse(idPerfil) == (int)Enuns.ePerfilUsuario.Jogador)
                    {
                        possuiJogador = true;
                    }

                    if (int.Parse(idPerfil) == (int)Enuns.ePerfilUsuario.Arbitro)
                    {
                        possuiArbitro = true;
                    }

                    var perfil = db.tbPerfil.Find(int.Parse(idPerfil));
                    if (perfil != null)
                    {
                        usuario.tbPerfil.Add(perfil);
                    }
                }

                usuario.usuarioDataSolicitacao = DateTime.Now;

                db.tbUsuario.Add(usuario);
                db.SaveChanges();

                UsuarioController usuarioController = new UsuarioController();

                if (possuiJogador)
                {
                    usuarioController.AdicionarJogador(usuario, DateTime.ParseExact(jogadorDataNascimento, Constantes.DATA_PADRAO, CultureInfo.InvariantCulture), usuarioCidade.Value);
                }

                if (possuiArbitro)
                {
                    usuarioController.AdicionarArbitro(usuario);
                }

                if (fileUpload != null)
                {
                    usuarioController.ApagarArquivosUsuario(usuario.usuarioId);

                    var caminhoFoto = Path.Combine(Server.MapPath(Constantes.CAMINHO_FOTOS), usuario.usuarioId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto); // Save the file

                    usuario.usuarioFoto = string.Format("{0}{1}{2}", Constantes.CAMINHO_FOTOS, usuario.usuarioId, Path.GetExtension(fileUpload.FileName));
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Login", "Autenticacao", new { mensagem = "Sua solicitação de acesso foi enviada! Aguarde até que nossos administradores lhe concedam permissão." });
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
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