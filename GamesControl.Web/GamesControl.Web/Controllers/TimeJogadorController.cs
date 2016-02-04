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

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbTime time = db.tbTime.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }

            var jogadoresSelecionados = time.tbJogador.Select(x => x.jogadorId).ToArray();
            ViewBag.Jogadores = new SelectList
            (
                db.tbJogador.Where(x => x.tbUsuario.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.OK && 
                                        !jogadoresSelecionados.Contains(x.jogadorId) &&
                                        x.jogadorAtivo == true).OrderBy(x => x.tbUsuario.usuarioNome),
                "jogadorId",
                "tbUsuario.usuarioNome"
            );

            return View(time);
        }

        public ActionResult Add(int idTime, string listaJogadores)
        {
            var time = db.tbTime.Find(idTime);
            if (time == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbJogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        time.tbJogador.Add(jogador);
                    }
                }
            }

            db.Entry(time).State = EntityState.Modified;
            db.SaveChanges();

            return PartialView();
        }

        public ActionResult Remove(int idTime, string listaJogadores)
        {
            if (idTime == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = db.tbTime.Find(idTime);
            if (time == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbJogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        time.tbJogador.Remove(jogador);
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
