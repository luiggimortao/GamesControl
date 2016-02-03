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

        public ActionResult Index()
        {
            return View(db.tbusuariostatus.ToList().OrderBy(x => x.usuarioStatusId));
        }

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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(int id, string descricao)
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

        public ActionResult EditConfirmed(int id, string descricao)
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
