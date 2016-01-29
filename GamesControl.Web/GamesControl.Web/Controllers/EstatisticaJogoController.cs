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
    public class EstatisticaJogoController : Controller
    {
        private Contexto db = new Contexto();

        // GET: EstatisticaJogo
        public ActionResult Index()
        {
            var tbestatisticajogo = db.tbestatisticajogo.Include(t => t.tbjogador).Include(t => t.tbjogo);
            return View(tbestatisticajogo.ToList());
        }

        // GET: EstatisticaJogo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbestatisticajogo tbestatisticajogo = db.tbestatisticajogo.Find(id);
            if (tbestatisticajogo == null)
            {
                return HttpNotFound();
            }
            return View(tbestatisticajogo);
        }

        // GET: EstatisticaJogo/Create
        public ActionResult Create()
        {
            ViewBag.jogadorId = new SelectList(db.tbjogador, "jogadorId", "jogadorNome");
            ViewBag.jogoId = new SelectList(db.tbjogo, "jogoId", "jogoId");
            return View();
        }

        // POST: EstatisticaJogo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "estatisticaJogoId,jogoId,jogadorId")] tbestatisticajogo tbestatisticajogo)
        {
            if (ModelState.IsValid)
            {
                db.tbestatisticajogo.Add(tbestatisticajogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.jogadorId = new SelectList(db.tbjogador, "jogadorId", "jogadorNome", tbestatisticajogo.jogadorId);
            ViewBag.jogoId = new SelectList(db.tbjogo, "jogoId", "jogoId", tbestatisticajogo.jogoId);
            return View(tbestatisticajogo);
        }

        // GET: EstatisticaJogo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbestatisticajogo tbestatisticajogo = db.tbestatisticajogo.Find(id);
            if (tbestatisticajogo == null)
            {
                return HttpNotFound();
            }
            ViewBag.jogadorId = new SelectList(db.tbjogador, "jogadorId", "jogadorNome", tbestatisticajogo.jogadorId);
            ViewBag.jogoId = new SelectList(db.tbjogo, "jogoId", "jogoId", tbestatisticajogo.jogoId);
            return View(tbestatisticajogo);
        }

        // POST: EstatisticaJogo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "estatisticaJogoId,jogoId,jogadorId")] tbestatisticajogo tbestatisticajogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbestatisticajogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.jogadorId = new SelectList(db.tbjogador, "jogadorId", "jogadorNome", tbestatisticajogo.jogadorId);
            ViewBag.jogoId = new SelectList(db.tbjogo, "jogoId", "jogoId", tbestatisticajogo.jogoId);
            return View(tbestatisticajogo);
        }

        // GET: EstatisticaJogo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbestatisticajogo tbestatisticajogo = db.tbestatisticajogo.Find(id);
            if (tbestatisticajogo == null)
            {
                return HttpNotFound();
            }
            return View(tbestatisticajogo);
        }

        // POST: EstatisticaJogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbestatisticajogo tbestatisticajogo = db.tbestatisticajogo.Find(id);
            db.tbestatisticajogo.Remove(tbestatisticajogo);
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
