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

        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.tbusuario.ToList().OrderBy(x => x.usuarioNome));
        }

        // GET: Usuario/Details/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuario usuario = db.tbusuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            var jogador = usuario.tbjogador.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);
            var arbitro = usuario.tbarbitro.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);

            UsuarioViewModel usuarioVM = new UsuarioViewModel();
            usuarioVM.Usuario = usuario;
            usuarioVM.Jogador = jogador;
            usuarioVM.Arbitro = arbitro;
            return View(usuarioVM);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList
            (
                db.tbusuariostatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
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
                db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome"
            );

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string usuarioNome, string usuarioEmail, string usuarioTelefone, int usuarioStatus, string[] usuarioPerfil, string jogadorDataNascimento, int? usuarioCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbusuario usuario = new tbusuario();
                usuario.usuarioEmail = usuarioEmail;
                usuario.usuarioNome = usuarioNome;
                usuario.usuarioSenha = "123";

                if (usuarioTelefone == "() -")
                {
                    usuarioTelefone = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(usuarioTelefone))
                {
                    usuario.usuarioTelefone = usuarioTelefone;
                }

                usuario.tbusuariostatus = db.tbusuariostatus.Find(usuarioStatus);

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

                db.tbusuario.Add(usuario);
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

                    var caminhoFoto = Path.Combine(Server.MapPath("~/Content/Fotos"), usuario.usuarioId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto); // Save the file

                    usuario.usuarioFoto = string.Format("{0}{1}{2}", "~/Content/Fotos/", usuario.usuarioId, Path.GetExtension(fileUpload.FileName));
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.Status = new SelectList
                (
                    db.tbusuariostatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
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
                    db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                    "cidadeId",
                    "cidadeNome"
                );

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuario usuario = db.tbusuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = new SelectList
            (
                db.tbusuariostatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
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

            var jogador = usuario.tbjogador.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);
            var arbitro = usuario.tbarbitro.FirstOrDefault(x => x.usuarioId == usuario.usuarioId);

            ViewBag.Cidade = new SelectList
            (
                db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
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

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int usuarioId, string usuarioNome, string usuarioEmail, string usuarioTelefone, int usuarioStatus, string[] usuarioPerfil, string jogadorDataNascimento, int? usuarioCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbusuario usuario = db.tbusuario.Find(usuarioId);

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

                usuario.tbusuariostatus = db.tbusuariostatus.Find(usuarioStatus);

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
                    var caminhoFoto = Path.Combine(Server.MapPath("~/Content/Fotos"), usuario.usuarioId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto); // Save the file

                    usuario.usuarioFoto = string.Format("{0}{1}{2}", "~/Content/Fotos/", usuario.usuarioId, Path.GetExtension(fileUpload.FileName));
                }

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                tbjogador jogador = null;
                if (possuiJogador)
                {
                    jogador = this.AdicionarJogador(usuario, DateTime.ParseExact(jogadorDataNascimento, Constantes.DATA_PADRAO, CultureInfo.InvariantCulture), usuarioCidade.Value);
                }

                tbarbitro arbitro = null;
                if (possuiArbitro)
                {
                    arbitro = this.AdicionarArbitro(usuario);
                }

                ViewBag.Status = new SelectList
                (
                    db.tbusuariostatus.ToList().OrderBy(u => u.usuarioStatusDescricao),
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
                    db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
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

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tbusuario tbusuario = db.tbusuario.Find(id);
                this.ExcluirPerfis(id);
                this.ExcluirJogadores(id);
                this.ExcluirArbitros(id);
                db.tbusuario.Remove(tbusuario);
                db.SaveChanges();

                this.ApagarArquivosUsuario(id);

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        #endregion

        #region - Métodos -

        private tbjogador AdicionarJogador(tbusuario usuario, DateTime dataNascimento, int idCidade)
        {
            tbjogador jogador = new tbjogador();
            jogador.jogadorDataNascimento = dataNascimento;
            jogador.cidadeId = idCidade;
            jogador.usuarioId = usuario.usuarioId;

            db.tbjogador.Add(jogador);
            db.SaveChanges();

            return jogador;
        }

        private tbarbitro AdicionarArbitro(tbusuario usuario)
        {
            tbarbitro arbitro = new tbarbitro();
            arbitro.usuarioId = usuario.usuarioId;

            db.tbarbitro.Add(arbitro);
            db.SaveChanges();

            return arbitro;
        }

        private void ExcluirPerfis(int idUsuario)
        {
            var usuario = db.tbusuario.Find(idUsuario);
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
            var jogadores = db.tbjogador.Where(x => x.usuarioId == idUsuario);

            if (jogadores.Count() > 0)
            {
                foreach (var jogador in jogadores)
                {
                    db.tbjogador.Remove(jogador);
                }
            }

            db.SaveChanges();
        }

        private void ExcluirArbitros(int idUsuario)
        {
            var arbitros = db.tbarbitro.Where(x => x.usuarioId == idUsuario);

            if (arbitros.Count() > 0)
            {
                foreach (var arbitro in arbitros)
                {
                    db.tbarbitro.Remove(arbitro);
                }
            }

            db.SaveChanges();
        }

        private void ApagarArquivosUsuario(int idUsuario)
        {
            string pattern = string.Format("{0}.*", idUsuario);
            foreach (string file in Directory.GetFiles(Server.MapPath("~/Content/Fotos"), pattern))
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
