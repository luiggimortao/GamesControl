using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamesControl.Web;
using System.Threading;

namespace GamesControl.Web.Controllers
{
    public class CidadeController : Controller
    {
        private Contexto db = new Contexto();

        public ActionResult Index()
        {
            return View(db.tbCidade.ToList().OrderBy(x => x.cidadeNome));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCidade tbCidade = db.tbCidade.Find(id);
            if (tbCidade == null)
            {
                return HttpNotFound();
            }
            return View(tbCidade);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(string nome)
        {
            try
            {
                tbCidade cidade = new tbCidade();
                cidade.cidadeNome = nome;
                db.tbCidade.Add(cidade);
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
            tbCidade tbCidade = db.tbCidade.Find(id);
            if (tbCidade == null)
            {
                return HttpNotFound();
            }
            return View(tbCidade);
        }

        public ActionResult EditConfirmed(int id, string nome)
        {
            try
            {
                tbCidade tbCidade = db.tbCidade.Find(id);
                
                if (tbCidade == null)
                {
                    throw new Exception(string.Format("|{0}|", "Cidade não encontrada!"));
                }
                tbCidade.cidadeNome = nome;
                db.Entry(tbCidade).State = EntityState.Modified;
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
            tbCidade tbCidade = db.tbCidade.Find(id);
            db.tbCidade.Remove(tbCidade);
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
