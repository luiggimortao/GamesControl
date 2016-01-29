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
    public class UsuarioStatusController : Controller
    {
        private Contexto db = new Contexto();

        // GET: UsuarioStatus
        public ActionResult Index()
        {
            return View(db.tbusuariostatus.ToList());
        }

        // GET: UsuarioStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            if (tbusuariostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbusuariostatus);
        }

        // GET: UsuarioStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usuarioStatusId,usuarioStatusDescricao")] tbusuariostatus tbusuariostatus)
        {
            if (ModelState.IsValid)
            {
                db.tbusuariostatus.Add(tbusuariostatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbusuariostatus);
        }

        // GET: UsuarioStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            if (tbusuariostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbusuariostatus);
        }

        // POST: UsuarioStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usuarioStatusId,usuarioStatusDescricao")] tbusuariostatus tbusuariostatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbusuariostatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbusuariostatus);
        }

        // GET: UsuarioStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            if (tbusuariostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbusuariostatus);
        }

        // POST: UsuarioStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            db.tbusuariostatus.Remove(tbusuariostatus);
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
