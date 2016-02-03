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
    public class CampeonatoController : Controller
    {
        private Contexto db = new Contexto();

        public ActionResult Index()
        {
            return View(db.tbCampeonato.ToList().OrderBy(x => x.campeonatoNome));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCampeonato campeonato = db.tbCampeonato.Find(id);
            if (campeonato == null)
            {
                return HttpNotFound();
            }
            return View(campeonato);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirmed(string nome)
        {
            try
            {
                tbCampeonato campeonato = new tbCampeonato();
                campeonato.campeonatoNome = nome;
                db.tbCampeonato.Add(campeonato);
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
            tbCampeonato campeonato = db.tbCampeonato.Find(id);
            if (campeonato == null)
            {
                return HttpNotFound();
            }
            return View(campeonato);
        }

        public ActionResult EditConfirmed(int id, string nome)
        {
            try
            {
                tbCampeonato campeonato = db.tbCampeonato.Find(id);

                if (campeonato == null)
                {
                    throw new Exception(string.Format("|{0}|", "Campeonato não encontrado!"));
                }
                campeonato.campeonatoNome = nome;
                db.Entry(campeonato).State = EntityState.Modified;
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
            tbCampeonato campeonato = db.tbCampeonato.Find(id);
            db.tbCampeonato.Remove(campeonato);
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
