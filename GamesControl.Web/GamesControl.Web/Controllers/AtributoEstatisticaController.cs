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
    public class AtributoEstatisticaController : Controller
    {
        private Contexto db = new Contexto();

        // GET: AtributoEstatistica
        public ActionResult Index()
        {
            return View(db.tbatributoestatistica.ToList());
        }

        // GET: AtributoEstatistica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbatributoestatistica tbatributoestatistica = db.tbatributoestatistica.Find(id);
            if (tbatributoestatistica == null)
            {
                return HttpNotFound();
            }
            return View(tbatributoestatistica);
        }

        // GET: AtributoEstatistica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AtributoEstatistica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "aributoEstatisticaId,aributoEstatisticaDescricao")] tbatributoestatistica tbatributoestatistica)
        {
            if (ModelState.IsValid)
            {
                db.tbatributoestatistica.Add(tbatributoestatistica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbatributoestatistica);
        }

        // GET: AtributoEstatistica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbatributoestatistica tbatributoestatistica = db.tbatributoestatistica.Find(id);
            if (tbatributoestatistica == null)
            {
                return HttpNotFound();
            }
            return View(tbatributoestatistica);
        }

        // POST: AtributoEstatistica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "aributoEstatisticaId,aributoEstatisticaDescricao")] tbatributoestatistica tbatributoestatistica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbatributoestatistica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbatributoestatistica);
        }

        // GET: AtributoEstatistica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbatributoestatistica tbatributoestatistica = db.tbatributoestatistica.Find(id);
            if (tbatributoestatistica == null)
            {
                return HttpNotFound();
            }
            return View(tbatributoestatistica);
        }

        // POST: AtributoEstatistica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbatributoestatistica tbatributoestatistica = db.tbatributoestatistica.Find(id);
            db.tbatributoestatistica.Remove(tbatributoestatistica);
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
