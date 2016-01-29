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
    public class ArbitroController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Arbitro
        public ActionResult Index()
        {
            var tbarbitro = db.tbarbitro.Include(t => t.tbusuario);
            return View(tbarbitro.ToList());
        }

        // GET: Arbitro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbarbitro tbarbitro = db.tbarbitro.Find(id);
            if (tbarbitro == null)
            {
                return HttpNotFound();
            }
            return View(tbarbitro);
        }

        // GET: Arbitro/Create
        public ActionResult Create()
        {
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail");
            return View();
        }

        // POST: Arbitro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "arbitroId,arbitroNome,usuarioId")] tbarbitro tbarbitro)
        {
            if (ModelState.IsValid)
            {
                db.tbarbitro.Add(tbarbitro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbarbitro.usuarioId);
            return View(tbarbitro);
        }

        // GET: Arbitro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbarbitro tbarbitro = db.tbarbitro.Find(id);
            if (tbarbitro == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbarbitro.usuarioId);
            return View(tbarbitro);
        }

        // POST: Arbitro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "arbitroId,arbitroNome,usuarioId")] tbarbitro tbarbitro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbarbitro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usuarioId = new SelectList(db.tbusuario, "usuarioId", "usuarioEmail", tbarbitro.usuarioId);
            return View(tbarbitro);
        }

        // GET: Arbitro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbarbitro tbarbitro = db.tbarbitro.Find(id);
            if (tbarbitro == null)
            {
                return HttpNotFound();
            }
            return View(tbarbitro);
        }

        // POST: Arbitro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbarbitro tbarbitro = db.tbarbitro.Find(id);
            db.tbarbitro.Remove(tbarbitro);
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
