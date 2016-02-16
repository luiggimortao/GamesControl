using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamesControl.Web;
using System.IO;
using GamesControl.Web.Models;
using GamesControl.Web.Comum;
using System.Globalization;

namespace GamesControl.Web.Controllers
{
    public class UsuarioController : Controller
    {
        #region - Variáveis -

        private Contexto db = new Contexto();

        #endregion

        #region - Actions -

        public ActionResult Index()
        {
            return View(db.tbUsuario.ToList().OrderBy(x => x.usuarioNome));
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUsuario usuario = db.tbUsuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            var jogador = usuario.tbJogador.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);
            var arbitro = usuario.tbArbitro.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);

            UsuarioViewModel usuarioVM = new UsuarioViewModel();
            usuarioVM.Usuario = usuario;
            usuarioVM.Jogador = jogador;
            usuarioVM.Arbitro = arbitro;
            return View(usuarioVM);
        }

        public ActionResult Create()
        {
            ViewBag.Status = new SelectList
            (
                db.tbUsuarioStatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
                "usuarioStatusId",
                "usuarioStatusDescricao"
            );

            ViewBag.Perfil = new SelectList
            (
                db.tbPerfil.ToList().OrderBy(p => p.perfilDescricao),
                "perfilId",
                "perfilDescricao"
            );

            ViewBag.Cidade = new SelectList
            (
                db.tbCidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome"
            );

            ViewBag.Incluido = "N";

            return View();
        }

        [HttpPost]
        public ActionResult Create(string usuarioNome, string usuarioEmail, string usuarioTelefone, int usuarioStatus, string[] usuarioPerfil, string jogadorDataNascimento, int? usuarioCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbUsuario usuario = new tbUsuario();
                usuario.usuarioEmail = usuarioEmail;
                usuario.usuarioNome = usuarioNome;
                usuario.usuarioSenha = System.Web.Security.Membership.GeneratePassword(6, 2);

                if (usuarioTelefone == "() -")
                {
                    usuarioTelefone = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(usuarioTelefone))
                {
                    usuario.usuarioTelefone = usuarioTelefone;
                }

                usuario.tbUsuarioStatus = db.tbUsuarioStatus.Find(usuarioStatus);

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

                db.tbUsuario.Add(usuario);
                db.SaveChanges();

                if (possuiJogador)
                {
                    this.AdicionarJogador(usuario, DateTime.ParseExact(jogadorDataNascimento, Constantes.DATA_PADRAO, CultureInfo.InvariantCulture), usuarioCidade.Value);
                }

                if (possuiArbitro)
                {
                    this.AdicionarArbitro(usuario);
                }

                if (fileUpload != null)
                {
                    this.ApagarArquivosUsuario(usuario.usuarioId);

                    var caminhoFoto = Path.Combine(Server.MapPath(Constantes.CAMINHO_FOTOS), usuario.usuarioId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto); // Save the file

                    usuario.usuarioFoto = string.Format("{0}{1}{2}", Constantes.CAMINHO_FOTOS, usuario.usuarioId, Path.GetExtension(fileUpload.FileName));
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.Status = new SelectList
                (
                    db.tbUsuarioStatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
                    "usuarioStatusId",
                    "usuarioStatusDescricao"
                );

                ViewBag.Perfil = new SelectList
                (
                    db.tbPerfil.ToList().OrderBy(p => p.perfilDescricao),
                    "perfilId",
                    "perfilDescricao"
                );

                ViewBag.Cidade = new SelectList
                (
                    db.tbCidade.ToList().OrderBy(p => p.cidadeNome),
                    "cidadeId",
                    "cidadeNome"
                );

                ViewBag.Incluido = "S";

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUsuario usuario = db.tbUsuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = new SelectList
            (
                db.tbUsuarioStatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
                "usuarioStatusId",
                "usuarioStatusDescricao",
                usuario.usuarioStatusId
            );

            ViewBag.Perfil = new SelectList
            (
                db.tbPerfil.ToList().OrderBy(p => p.perfilDescricao),
                "perfilId",
                "perfilDescricao",
                usuario.perfilId
            );

            var jogador = usuario.tbJogador.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);
            var arbitro = usuario.tbArbitro.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);

            ViewBag.Cidade = new SelectList
            (
                db.tbCidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome",
                jogador != null ? jogador.cidadeId.ToString() : string.Empty
            );

            ViewBag.Alterado = "N";

            UsuarioViewModel usuarioVM = new UsuarioViewModel();
            usuarioVM.Usuario = usuario;
            usuarioVM.Jogador = jogador;
            usuarioVM.Arbitro = arbitro;

            return View(usuarioVM);
        }

        [HttpPost]
        public ActionResult Edit(int usuarioId, string usuarioNome, string usuarioEmail, string usuarioTelefone, int usuarioStatus, string[] usuarioPerfil, string jogadorDataNascimento, int? usuarioCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbUsuario usuario = db.tbUsuario.Find(usuarioId);

                if (usuario == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                usuario.usuarioEmail = usuarioEmail;
                usuario.usuarioNome = usuarioNome;
                if (usuarioTelefone == "() -")
                {
                    usuarioTelefone = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(usuarioTelefone))
                {
                    usuario.usuarioTelefone = usuarioTelefone;
                }

                usuario.tbUsuarioStatus = db.tbUsuarioStatus.Find(usuarioStatus);

                this.ExcluirPerfis(usuarioId);
                this.ExcluirJogadores(usuarioId);
                this.ExcluirArbitros(usuarioId);
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

                if (fileUpload != null)
                {
                    this.ApagarArquivosUsuario(usuario.usuarioId);
                    var caminhoFoto = Path.Combine(Server.MapPath(Constantes.CAMINHO_FOTOS), usuario.usuarioId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto); // Save the file

                    usuario.usuarioFoto = string.Format("{0}{1}{2}", "~/Content/Fotos/", usuario.usuarioId, Path.GetExtension(fileUpload.FileName));
                }

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                tbJogador jogador = null;
                if (possuiJogador)
                {
                    jogador = this.AdicionarJogador(usuario, DateTime.ParseExact(jogadorDataNascimento, Constantes.DATA_PADRAO, CultureInfo.InvariantCulture), usuarioCidade.Value);
                }

                tbArbitro arbitro = null;
                if (possuiArbitro)
                {
                    arbitro = this.AdicionarArbitro(usuario);
                }

                ViewBag.Status = new SelectList
                (
                    db.tbUsuarioStatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
                    "usuarioStatusId",
                    "usuarioStatusDescricao",
                    usuario.usuarioStatusId
                );

                ViewBag.Perfil = new SelectList
                (
                    db.tbPerfil.ToList().OrderBy(p => p.perfilDescricao),
                    "perfilId",
                    "perfilDescricao",
                    usuario.perfilId
                );

                ViewBag.Cidade = new SelectList
                (
                    db.tbCidade.ToList().OrderBy(p => p.cidadeNome),
                    "cidadeId",
                    "cidadeNome",
                    jogador != null ? jogador.cidadeId.ToString() : string.Empty
                );

                ViewBag.Alterado = "S";

                UsuarioViewModel usuarioVM = new UsuarioViewModel();
                usuarioVM.Usuario = usuario;
                usuarioVM.Jogador = jogador;
                usuarioVM.Arbitro = arbitro;

                return View(usuarioVM);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tbUsuario usuario = db.tbUsuario.Find(id);
                tbUsuarioStatus status = db.tbUsuarioStatus.Find((int)Enuns.eStatusUsuario.Inativo);
                usuario.tbUsuarioStatus = status;

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        #endregion

        #region - Métodos -

        private tbJogador AdicionarJogador(tbUsuario usuario, DateTime dataNascimento, int idCidade)
        {
            tbJogador jogador = null;
            var jogadores = db.tbJogador.Where(x => x.tbUsuario.usuarioId == usuario.usuarioId);

            if (jogadores.Count() > 0)
            {
                jogador = jogadores.FirstOrDefault();
                jogador.jogadorDataNascimento = dataNascimento;
                jogador.cidadeId = idCidade;
                jogador.usuarioId = usuario.usuarioId;
                jogador.jogadorAtivo = true;

                db.Entry(jogador).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                jogador = new tbJogador();
                jogador.jogadorDataNascimento = dataNascimento;
                jogador.cidadeId = idCidade;
                jogador.usuarioId = usuario.usuarioId;
                jogador.jogadorAtivo = true;

                db.tbJogador.Add(jogador);
            }

            db.SaveChanges();

            return jogador;
        }

        private tbArbitro AdicionarArbitro(tbUsuario usuario)
        {
            tbArbitro arbitro = new tbArbitro();
            arbitro.usuarioId = usuario.usuarioId;

            db.tbArbitro.Add(arbitro);
            db.SaveChanges();

            return arbitro;
        }

        private void ExcluirPerfis(int idUsuario)
        {
            var usuario = db.tbUsuario.Find(idUsuario);
            if (usuario != null)
            {
                while (usuario.tbPerfil.Count > 0)
                {
                    usuario.tbPerfil.Remove(usuario.tbPerfil.FirstOrDefault());
                }
            }

            db.SaveChanges();
        }

        private void ExcluirJogadores(int idUsuario)
        {
            var jogadores = db.tbJogador.Where(x => x.usuarioId == idUsuario);

            if (jogadores.Count() > 0)
            {
                foreach (var jogador in jogadores)
                {
                    jogador.jogadorAtivo = false;
                    db.Entry(jogador).State = EntityState.Modified;
                }
            }

            db.SaveChanges();
        }

        private void ExcluirArbitros(int idUsuario)
        {
            var arbitros = db.tbArbitro.Where(x => x.usuarioId == idUsuario);

            if (arbitros.Count() > 0)
            {
                foreach (var arbitro in arbitros)
                {
                    db.tbArbitro.Remove(arbitro);
                }
            }

            db.SaveChanges();
        }

        private void ApagarArquivosUsuario(int idUsuario)
        {
            string pattern = string.Format("{0}.*", idUsuario);
            foreach (string file in Directory.GetFiles(Server.MapPath(Constantes.CAMINHO_FOTOS), pattern))
            {
                System.IO.File.Delete(file);
            }
        }

        #endregion

        #region - Destrutores -

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
