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
            var tbusuario = db.tbusuario.Include(t => t.tbusuariostatus);
            return View(tbusuario.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
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
            ViewBag.usuarioStatusId = new SelectList(db.tbusuariostatus, "usuarioStatusId", "usuarioStatusDescricao");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usuarioId,usuarioEmail,usuarioNome,usuarioSenha,usuarioStatusId")] tbusuario tbusuario)
        {
            if (ModelState.IsValid)
            {
                db.tbusuario.Add(tbusuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.usuarioStatusId = new SelectList(db.tbusuariostatus, "usuarioStatusId", "usuarioStatusDescricao", tbusuario.usuarioStatusId);
            return View(tbusuario);
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
            ViewBag.usuarioStatusId = new SelectList(db.tbusuariostatus, "usuarioStatusId", "usuarioStatusDescricao", tbusuario.usuarioStatusId);
            return View(tbusuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usuarioId,usuarioEmail,usuarioNome,usuarioSenha,usuarioStatusId")] tbusuario tbusuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbusuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usuarioStatusId = new SelectList(db.tbusuariostatus, "usuarioStatusId", "usuarioStatusDescricao", tbusuario.usuarioStatusId);
            return View(tbusuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbusuario tbusuario = db.tbusuario.Find(id);
            db.tbusuario.Remove(tbusuario);
            db.SaveChanges();
            return RedirectToAction("Index");
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
