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
            return View(db.tbUsuarioStatus.ToList().OrderBy(x => x.usuarioStatusId));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUsuarioStatus tbUsuarioStatus = db.tbUsuarioStatus.Find(id);
            if (tbUsuarioStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbUsuarioStatus);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(int id, string descricao)
        {
            try
            {
                tbUsuarioStatus usuariostatus = new tbUsuarioStatus();
                usuariostatus.usuarioStatusId = id;
                usuariostatus.usuarioStatusDescricao = descricao;
                db.tbUsuarioStatus.Add(usuariostatus);
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
            tbUsuarioStatus tbUsuarioStatus = db.tbUsuarioStatus.Find(id);
            if (tbUsuarioStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbUsuarioStatus);
        }

        public ActionResult EditConfirmed(int id, string descricao)
        {
            try
            {
                tbUsuarioStatus tbUsuarioStatus = db.tbUsuarioStatus.Find(id);
                
                if (tbUsuarioStatus == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }
                tbUsuarioStatus.usuarioStatusDescricao = descricao;
                db.Entry(tbUsuarioStatus).State = EntityState.Modified;
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
            tbUsuarioStatus tbUsuarioStatus = db.tbUsuarioStatus.Find(id);
            db.tbUsuarioStatus.Remove(tbUsuarioStatus);
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
