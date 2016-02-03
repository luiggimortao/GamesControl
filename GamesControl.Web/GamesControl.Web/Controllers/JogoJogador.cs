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
    public class JogoJogadorController : Controller
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
            tbjogo jogo = db.tbjogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }

            var jogadoresSelecionadosCasa = jogo.tbJogoJogadorTime.Where(x => x.tbjogo.jogoId == id && x.tbtime.timeId == jogo.timeCasaId).Select(x => x.jogadorId).ToArray();
            var jogadoresSelecionadosVisitante = jogo.tbJogoJogadorTime.Where(x => x.tbjogo.jogoId == id && x.tbtime.timeId == jogo.timeVisitanteId).Select(x => x.jogadorId).ToArray();
            ViewBag.JogadoresTimeCasa = new SelectList
            (
                db.tbjogador.Where(x => !jogadoresSelecionadosCasa.Contains(x.jogadorId) && !jogadoresSelecionadosVisitante.Contains(x.jogadorId)).OrderBy(x => x.tbusuario.usuarioNome),
                "jogadorId",
                "tbusuario.usuarioNome"
            );

            ViewBag.JogadoresTimeVisitante = new SelectList
            (
                db.tbjogador.Where(x => !jogadoresSelecionadosCasa.Contains(x.jogadorId) && !jogadoresSelecionadosVisitante.Contains(x.jogadorId)).OrderBy(x => x.tbusuario.usuarioNome),
                "jogadorId",
                "tbusuario.usuarioNome"
            );

            return View(jogo);
        }

        public ActionResult Add(int idJogo, int idTime, string listaJogadores)
        {
            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var jogo = db.tbjogo.Find(idJogo);
                if (jogo == null)
                {
                    return HttpNotFound();
                }

                var time = db.tbtime.Find(idTime);
                if (time == null)
                {
                    return HttpNotFound();
                }

                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbjogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        var jogoJogadorTime = new tbJogoJogadorTime();
                        jogoJogadorTime.tbjogo = jogo;
                        jogoJogadorTime.tbtime = time;
                        jogoJogadorTime.tbjogador = jogador;

                        jogo.tbJogoJogadorTime.Add(jogoJogadorTime);
                    }
                }

                db.Entry(jogo).State = EntityState.Modified;
                db.SaveChanges();
            }

            return PartialView();
        }

        public ActionResult Remove(int idJogo, int idTime, string listaJogadores)
        {
            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var jogo = db.tbjogo.Find(idJogo);
                if (jogo == null)
                {
                    return HttpNotFound();
                }

                var time = db.tbtime.Find(idTime);
                if (time == null)
                {
                    return HttpNotFound();
                }

                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbjogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        var jogoJogadorTime = new tbJogoJogadorTime();
                        jogoJogadorTime.tbjogo = jogo;
                        jogoJogadorTime.tbtime = time;
                        jogoJogadorTime.tbjogador = jogador;

                        jogo.tbJogoJogadorTime.Remove(jogoJogadorTime);
                    }
                }

                db.Entry(jogo).State = EntityState.Modified;
                db.SaveChanges();
            }

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
