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
    public class JogoController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Jogo
        public ActionResult Index()
        {
            var tbjogo = db.tbjogo.Include(t => t.tbjogostatus).Include(t => t.tbtime).Include(t => t.tbtime1);
            return View(tbjogo.ToList());
        }

        // GET: Jogo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogo tbjogo = db.tbjogo.Find(id);
            if (tbjogo == null)
            {
                return HttpNotFound();
            }
            return View(tbjogo);
        }

        // GET: Jogo/Create
        public ActionResult Create()
        {
            ViewBag.jogoStatusId = new SelectList(db.tbjogostatus, "jogoStatusId", "jogoStatusDescricao");
            ViewBag.timeCasaId = new SelectList(db.tbtime, "timeId", "timeNome");
            ViewBag.timeVisitanteId = new SelectList(db.tbtime, "timeId", "timeNome");
            return View();
        }

        // POST: Jogo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jogoId,timeCasaId,timeVisitanteId,jogoData,jogoStatusId")] tbjogo tbjogo)
        {
            if (ModelState.IsValid)
            {
                db.tbjogo.Add(tbjogo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.jogoStatusId = new SelectList(db.tbjogostatus, "jogoStatusId", "jogoStatusDescricao", tbjogo.jogoStatusId);
            ViewBag.timeCasaId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeCasaId);
            ViewBag.timeVisitanteId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeVisitanteId);
            return View(tbjogo);
        }

        // GET: Jogo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogo tbjogo = db.tbjogo.Find(id);
            if (tbjogo == null)
            {
                return HttpNotFound();
            }
            ViewBag.jogoStatusId = new SelectList(db.tbjogostatus, "jogoStatusId", "jogoStatusDescricao", tbjogo.jogoStatusId);
            ViewBag.timeCasaId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeCasaId);
            ViewBag.timeVisitanteId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeVisitanteId);
            return View(tbjogo);
        }

        // POST: Jogo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jogoId,timeCasaId,timeVisitanteId,jogoData,jogoStatusId")] tbjogo tbjogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbjogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.jogoStatusId = new SelectList(db.tbjogostatus, "jogoStatusId", "jogoStatusDescricao", tbjogo.jogoStatusId);
            ViewBag.timeCasaId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeCasaId);
            ViewBag.timeVisitanteId = new SelectList(db.tbtime, "timeId", "timeNome", tbjogo.timeVisitanteId);
            return View(tbjogo);
        }

        // GET: Jogo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogo tbjogo = db.tbjogo.Find(id);
            if (tbjogo == null)
            {
                return HttpNotFound();
            }
            return View(tbjogo);
        }

        // POST: Jogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbjogo tbjogo = db.tbjogo.Find(id);
            db.tbjogo.Remove(tbjogo);
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
