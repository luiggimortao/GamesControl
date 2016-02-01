using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamesControl.Web;

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
        public ActionResult Create(string email, string nome, string telefone, int status, int perfil)
        {
            try
            {
                tbusuario usuario = new tbusuario();
                usuario.usuarioEmail = email;
                usuario.usuarioNome = nome;
                usuario.usuarioSenha = "123";

                if (telefone == "() -")
                {
                    telefone = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(telefone))
                {
                    usuario.usuarioTelefone = telefone;
                }

                usuario.tbusuariostatus = db.tbusuariostatus.Find(status);
                usuario.tbPerfil = db.tbPerfil.Find(perfil);

                db.tbusuario.Add(usuario);
                db.SaveChanges();

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
            tbusuario tbusuario = db.tbusuario.Find(id);
            if (tbusuario == null)
            {
                return HttpNotFound();
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

            return View(tbusuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int id, string email, string nome, string senha, string telefone, int status, int perfil)
        {
            try
            {
                tbusuario usuario = db.tbusuario.Find(id);

                if (usuario == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                usuario.usuarioEmail = email;
                usuario.usuarioNome = nome;
                usuario.usuarioSenha = senha;
                if (!string.IsNullOrWhiteSpace(telefone))
                {
                    usuario.usuarioTelefone = telefone;
                }
                usuario.tbusuariostatus.usuarioStatusId = status;
                usuario.tbPerfil.perfilId = perfil;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

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

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbusuario tbusuario = db.tbusuario.Find(id);
            db.tbusuario.Remove(tbusuario);
            db.SaveChanges();
            return PartialView();
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
