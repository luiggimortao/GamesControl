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
            tbJogo jogo = db.tbJogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }

            var jogadoresSelecionadosCasa = jogo.tbJogoJogadorTime.Where(x => x.tbJogo.jogoId == id && x.tbTime.timeId == jogo.timeCasaId).Select(x => x.jogadorId).ToArray();
            var jogadoresSelecionadosVisitante = jogo.tbJogoJogadorTime.Where(x => x.tbJogo.jogoId == id && x.tbTime.timeId == jogo.timeVisitanteId).Select(x => x.jogadorId).ToArray();
            ViewBag.JogadoresTimeCasa = new SelectList
            (
                db.tbJogador.Where(x => !jogadoresSelecionadosCasa.Contains(x.jogadorId) &&
                                        !jogadoresSelecionadosVisitante.Contains(x.jogadorId) &&
                                        x.jogadorAtivo == true).OrderBy(x => x.tbUsuario.usuarioNome),
                "jogadorId",
                "tbUsuario.usuarioNome"
            );

            ViewBag.JogadoresTimeVisitante = new SelectList
            (
                db.tbJogador.Where(x => !jogadoresSelecionadosCasa.Contains(x.jogadorId) &&
                                        !jogadoresSelecionadosVisitante.Contains(x.jogadorId) &&
                                        x.jogadorAtivo == true).OrderBy(x => x.tbUsuario.usuarioNome),
                "jogadorId",
                "tbUsuario.usuarioNome"
            );

            return View(jogo);
        }

        public ActionResult Add(int idJogo, int idTime, string listaJogadores)
        {
            if (!string.IsNullOrWhiteSpace(listaJogadores))
            {
                var jogo = db.tbJogo.Find(idJogo);
                if (jogo == null)
                {
                    return HttpNotFound();
                }

                var time = db.tbTime.Find(idTime);
                if (time == null)
                {
                    return HttpNotFound();
                }

                var splitJogadores = listaJogadores.Split('|');
                foreach (var idJogador in splitJogadores)
                {
                    var jogador = db.tbJogador.Find(int.Parse(idJogador));
                    if (jogador != null)
                    {
                        var jogoJogadorTime = new tbJogoJogadorTime();
                        jogoJogadorTime.tbJogo = jogo;
                        jogoJogadorTime.tbTime = time;
                        jogoJogadorTime.tbJogador = jogador;

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
                var splitJogadores = listaJogadores.Split('|');
                foreach (var textoIdJogador in splitJogadores)
                {
                    int idJogador = int.Parse(textoIdJogador);
                    var jogoJogadorTime = db.tbJogoJogadorTime.FirstOrDefault(x => x.tbJogo.jogoId == idJogo &&
                                                                                    x.tbTime.timeId == idTime &&
                                                                                    x.tbJogador.jogadorId == idJogador);
                    if (jogoJogadorTime != null)
                    {
                        db.tbJogoJogadorTime.Remove(jogoJogadorTime);
                    }
                }

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
