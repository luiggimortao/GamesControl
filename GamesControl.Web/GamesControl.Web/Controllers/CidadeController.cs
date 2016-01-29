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
    public class CidadeController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Cidade
        public ActionResult Index()
        {
            return View(db.tbcidade.ToList().OrderBy(x => x.cidadeNome));
        }

        // GET: Cidade/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbcidade tbcidade = db.tbcidade.Find(id);
            if (tbcidade == null)
            {
                return HttpNotFound();
            }
            return View(tbcidade);
        }

        // GET: Cidade/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cidade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string nome)
        {
            throw new Exception("|Zica do cabrito|");

            try
            {
                tbcidade cidade = new tbcidade();
                cidade.cidadeNome = nome;
                db.tbcidade.Add(cidade);
                db.SaveChanges();
                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        // GET: Cidade/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbcidade tbcidade = db.tbcidade.Find(id);
            if (tbcidade == null)
            {
                return HttpNotFound();
            }
            return View(tbcidade);
        }

        // POST: Cidade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int id, string nome)
        {
            try
            {
                tbcidade tbcidade = db.tbcidade.Find(id);
                
                if (tbcidade == null)
                {
                    throw new Exception(string.Format("|{0}|", "Cidade não encontrada!"));
                }
                tbcidade.cidadeNome = nome;
                db.Entry(tbcidade).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        // POST: Cidade/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbcidade tbcidade = db.tbcidade.Find(id);
            db.tbcidade.Remove(tbcidade);
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
