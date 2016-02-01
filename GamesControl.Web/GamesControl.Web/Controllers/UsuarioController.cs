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

namespace GamesControl.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private Contexto db = new Contexto();

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
            tbusuario tbusuario = db.tbusuario.Find(id);
            if (tbusuario == null)
            {
                return HttpNotFound();
            }
            return View(tbusuario);
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

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string usuarioNome, string usuarioEmail, string usuarioTelefone, int Status, string[] Perfil, HttpPostedFileBase fileUpload)
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

                usuario.tbusuariostatus = db.tbusuariostatus.Find(Status);

                foreach (string idPerfil in Perfil)
                {
                    var perfil = db.tbPerfil.Find(int.Parse(idPerfil));
                    if (perfil != null)
                    {
                        usuario.tbPerfil.Add(perfil);
                    }
                }

                db.tbusuario.Add(usuario);
                db.SaveChanges();

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

            ViewBag.Alterado = "N";

            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int usuarioId, string usuarioNome, string usuarioEmail, string usuarioTelefone, int Status, string[] Perfil, HttpPostedFileBase fileUpload)
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

                usuario.tbusuariostatus = db.tbusuariostatus.Find(Status);

                this.ExcluirPerfis(usuarioId);
                foreach (string idPerfil in Perfil)
                {
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

                ViewBag.Alterado = "S";

                return PartialView(usuario);
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

        private void ApagarArquivosUsuario(int idUsuario)
        {
            string pattern = string.Format("{0}.*", idUsuario);
            var matches = Directory.GetFiles(Server.MapPath("~/Content/Fotos"), pattern);
            foreach (string file in Directory.GetFiles(Server.MapPath("~/Content/Fotos")).Except(matches))
            {
                System.IO.File.Delete(file);
            }
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
