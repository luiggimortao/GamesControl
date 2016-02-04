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
    public class EstatisticaJogoJogadorTimeController : Controller
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

            ViewBag.JogadoresTimeCasa = new SelectList
            (
                jogo.tbJogoJogadorTime.Where(x => x.tbTime.timeId == jogo.tbTime.timeId).OrderBy(x => x.tbJogador.tbUsuario.usuarioNome),
                "tbJogador.jogadorId",
                "tbJogador.tbUsuario.usuarioNome"
            );

            ViewBag.JogadoresTimeVisitante = new SelectList
            (
                jogo.tbJogoJogadorTime.Where(x => x.tbTime.timeId == jogo.tbTime1.timeId).OrderBy(x => x.tbJogador.tbUsuario.usuarioNome),
                "tbJogador.jogadorId",
                "tbJogador.tbUsuario.usuarioNome"
            );

            ViewBag.Estatisticas = new SelectList
            (
                db.tbAtributoEstatistica.OrderBy(x => x.atributoEstatisticaDescricao),
                "atributoEstatisticaId",
                "atributoEstatisticaDescricao"
            );

            return View(jogo);
        }

        public ActionResult Add(int idJogo, int idTime, int idJogador, int idAtributoEstatistica, int quantidade)
        {
            var estatisticaExistente = db.tbEstatisticaJogoJogadorTime.FirstOrDefault(x => x.jogoId == idJogo &&
                                                                                      x.timeId == idTime &&
                                                                                      x.jogadorId == idJogador &&
                                                                                      x.atributoEstatisticaId == idAtributoEstatistica);
            if (estatisticaExistente != null)
            {
                estatisticaExistente.estatisticaJogoQuantidade = quantidade;
                db.Entry(estatisticaExistente).State = EntityState.Modified;
            }
            else
            {
                var jogoJogadorTime = db.tbJogoJogadorTime.FirstOrDefault(x => x.tbJogo.jogoId == idJogo &&
                                                                              x.tbTime.timeId == idTime &&
                                                                              x.tbJogador.jogadorId == idJogador);
                if (jogoJogadorTime == null)
                {
                    return HttpNotFound();
                }

                var atributoEstatistica = db.tbAtributoEstatistica.Find(idAtributoEstatistica);
                if (atributoEstatistica == null)
                {
                    return HttpNotFound();
                }

                var estatisticaJogoJogadorTime = new tbEstatisticaJogoJogadorTime();
                estatisticaJogoJogadorTime.tbJogoJogadorTime = jogoJogadorTime;
                estatisticaJogoJogadorTime.tbAtributoEstatistica = atributoEstatistica;
                estatisticaJogoJogadorTime.estatisticaJogoQuantidade = quantidade;

                db.tbEstatisticaJogoJogadorTime.Add(estatisticaJogoJogadorTime);
            }

            db.SaveChanges();

            return PartialView();
        }

        public ActionResult Remove(int idJogo, int idTime, int idJogador, int idAtributoEstatistica)
        {
            var tbEstatisticaJogoJogadorTime = db.tbEstatisticaJogoJogadorTime.FirstOrDefault(x => x.tbJogoJogadorTime.tbJogo.jogoId == idJogo &&
                                                                                                   x.tbJogoJogadorTime.tbTime.timeId == idTime &&
                                                                                                   x.tbJogoJogadorTime.tbJogador.jogadorId == idJogador &&
                                                                                                   x.tbAtributoEstatistica.atributoEstatisticaId == idAtributoEstatistica);
            if (tbEstatisticaJogoJogadorTime != null)
            {
                db.tbEstatisticaJogoJogadorTime.Remove(tbEstatisticaJogoJogadorTime);
                db.SaveChanges();
            }


            return PartialView();
        }

        public ActionResult ListarEstatisticasTimeCasa(int idJogo, int idTime, int idJogador)
        {
            var lista = db.tbEstatisticaJogoJogadorTime.Where(x => x.jogoId == idJogo &&
                                                                x.timeId == idTime &&
                                                                x.jogadorId == idJogador);

            return PartialView(lista);
        }

        public ActionResult ListarEstatisticasTimeVisitante(int idJogo, int idTime, int idJogador)
        {
            var lista = db.tbEstatisticaJogoJogadorTime.Where(x => x.jogoId == idJogo &&
                                                                   x.timeId == idTime &&
                                                                   x.jogadorId == idJogador);

            return PartialView(lista);
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
