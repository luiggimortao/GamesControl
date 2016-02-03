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

        public ActionResult Index()
        {
            return View(db.tbjogostatus.ToList().OrderBy(x => x.jogoStatusId));
        }

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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(int id, string descricao)
        {
            try
            {
                tbjogostatus jogostatus = new tbjogostatus();
                jogostatus.jogoStatusId = id;
                jogostatus.jogoStatusDescricao = descricao;
                db.tbjogostatus.Add(jogostatus);
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
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            if (tbjogostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbjogostatus);
        }

        public ActionResult EditConfirmed(int id, string descricao)
        {
            try
            {
                tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
                
                if (tbjogostatus == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                tbjogostatus.jogoStatusDescricao = descricao;
                db.Entry(tbjogostatus).State = EntityState.Modified;
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
            tbjogostatus tbjogostatus = db.tbjogostatus.Find(id);
            db.tbjogostatus.Remove(tbjogostatus);
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
