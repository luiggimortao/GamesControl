using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GamesControl.Web;
using System.IO;
using GamesControl.Web.Models;
using GamesControl.Web.Comum;
using System.Globalization;

namespace GamesControl.Web.Controllers
{
    public class TimeJogadorController : Controller
    {
        #region - Variáveis -

        private Contexto db = new Contexto();

        #endregion

        #region - Actions -

        // GET: Usuario
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbtime time = db.tbtime.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }

            var jogadoresSelecionados = time.tbjogador.Select(x => x.jogadorId).ToArray();
            ViewBag.Jogadores = new SelectList
            (
                db.tbjogador.Where(x => x.tbusuario.tbusuariostatus.usuarioStatusId == (int)Enuns.eStatusUsuario.OK && !jogadoresSelecionados.Contains(x.jogadorId)).OrderBy(x => x.tbusuario.usuarioNome),
                "jogadorId",
                "tbusuario.usuarioNome"
            );

            return View(time);
        }

        // GET: Usuario/Add
        [HttpPost]
        public ActionResult Add(int idTime, string listaJogadores)
        {
            if (idTime == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = db.tbtime.Find(idTime);
            if (time == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbjogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        time.tbjogador.Add(jogador);
                    }
                }
            }

            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();

            return PartialView();
        }

        // GET: Usuario/Remove
        [HttpPost]
        public ActionResult Remove(int idTime, string listaJogadores)
        {
            if (idTime == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = db.tbtime.Find(idTime);
            if (time == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbjogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        time.tbjogador.Remove(jogador);
                    }
                }
            }

            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();

            return PartialView();
        }

        #endregion

        #region - Destrutores -

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
