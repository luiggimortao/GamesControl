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
            return View(db.tbJogoStatus.ToList().OrderBy(x => x.jogoStatusId));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbJogoStatus tbJogoStatus = db.tbJogoStatus.Find(id);
            if (tbJogoStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbJogoStatus);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(int id, string descricao)
        {
            try
            {
                tbJogoStatus jogostatus = new tbJogoStatus();
                jogostatus.jogoStatusId = id;
                jogostatus.jogoStatusDescricao = descricao;
                db.tbJogoStatus.Add(jogostatus);
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
            tbJogoStatus tbJogoStatus = db.tbJogoStatus.Find(id);
            if (tbJogoStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbJogoStatus);
        }

        public ActionResult EditConfirmed(int id, string descricao)
        {
            try
            {
                tbJogoStatus tbJogoStatus = db.tbJogoStatus.Find(id);
                
                if (tbJogoStatus == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                tbJogoStatus.jogoStatusDescricao = descricao;
                db.Entry(tbJogoStatus).State = EntityState.Modified;
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
            tbJogoStatus tbJogoStatus = db.tbJogoStatus.Find(id);
            db.tbJogoStatus.Remove(tbJogoStatus);
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
