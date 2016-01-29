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
    public class JogoStatusController : Controller
    {
        private Contexto db = new Contexto();

        // GET: JogoStatus
        public ActionResult Index()
        {
            return View(db.tbjogostatus.ToList());
        }

        // GET: JogoStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            if (tbjogostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbjogostatus);
        }

        // GET: JogoStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JogoStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jogoStatusId,jogoStatusDescricao")] tbjogostatus tbjogostatus)
        {
            if (ModelState.IsValid)
            {
                db.tbjogostatus.Add(tbjogostatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbjogostatus);
        }

        // GET: JogoStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            if (tbjogostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbjogostatus);
        }

        // POST: JogoStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jogoStatusId,jogoStatusDescricao")] tbjogostatus tbjogostatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbjogostatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbjogostatus);
        }

        // GET: JogoStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            if (tbjogostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbjogostatus);
        }

        // POST: JogoStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            db.tbjogostatus.Remove(tbjogostatus);
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
