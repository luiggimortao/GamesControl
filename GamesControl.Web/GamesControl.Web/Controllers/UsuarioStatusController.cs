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
            return View(db.tbusuariostatus.ToList().OrderBy(x => x.usuarioStatusId));
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
        public ActionResult Create(int id, string descricao)
        {
            try
            {
                tbusuariostatus usuariostatus = new tbusuariostatus();
                usuariostatus.usuarioStatusId = id;
                usuariostatus.usuarioStatusDescricao = descricao;
                db.tbusuariostatus.Add(usuariostatus);
                db.SaveChanges();
                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
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
        public ActionResult Edit(int id, string descricao)
        {
            try
            {
                tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
                
                if (tbusuariostatus == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                tbusuariostatus.usuarioStatusDescricao = descricao;
                db.Entry(tbusuariostatus).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        // POST: UsuarioStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            db.tbusuariostatus.Remove(tbusuariostatus);
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
