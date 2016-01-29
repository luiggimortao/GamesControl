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
    public class JogadorController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Jogador
        public ActionResult Index()
        {
            var tbjogador = db.tbjogador.Include(t => t.tbcidade).Include(t => t.tbusuario);
            return View(tbjogador.ToList());
        }

        // GET: Jogador/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogador tbjogador = db.tbjogador.Find(id);
            if (tbjogador == null)
            {
                return HttpNotFound();
            }
            return View(tbjogador);
        }

        // GET: Jogador/Create
        public ActionResult Create()
        {
            ViewBag.cidadeId = new SelectList(db.tbcidade, "cidadeId", "cidadeNome");
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail");
            return View();
        }

        // POST: Jogador/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jogadorId,jogadorNome,jogadorDataNascimento,jogadorEndereco,jogadorCaminhoFoto,jogadorTelefone,cidadeId,usuarioId")] tbjogador tbjogador)
        {
            if (ModelState.IsValid)
            {
                db.tbjogador.Add(tbjogador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cidadeId = new SelectList(db.tbcidade, "cidadeId", "cidadeNome", tbjogador.cidadeId);
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbjogador.usuarioId);
            return View(tbjogador);
        }

        // GET: Jogador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogador tbjogador = db.tbjogador.Find(id);
            if (tbjogador == null)
            {
                return HttpNotFound();
            }
            ViewBag.cidadeId = new SelectList(db.tbcidade, "cidadeId", "cidadeNome", tbjogador.cidadeId);
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbjogador.usuarioId);
            return View(tbjogador);
        }

        // POST: Jogador/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jogadorId,jogadorNome,jogadorDataNascimento,jogadorEndereco,jogadorCaminhoFoto,jogadorTelefone,cidadeId,usuarioId")] tbjogador tbjogador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbjogador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cidadeId = new SelectList(db.tbcidade, "cidadeId", "cidadeNome", tbjogador.cidadeId);
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbjogador.usuarioId);
            return View(tbjogador);
        }

        // GET: Jogador/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogador tbjogador = db.tbjogador.Find(id);
            if (tbjogador == null)
            {
                return HttpNotFound();
            }
            return View(tbjogador);
        }

        // POST: Jogador/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbjogador tbjogador = db.tbjogador.Find(id);
            db.tbjogador.Remove(tbjogador);
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
