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
    public class JogoController : Controller
    {
        #region - Variáveis -

        private Contexto db = new Contexto();

        #endregion

        #region - Actions -

        public ActionResult Index()
        {
            return View(db.tbJogo.ToList().OrderByDescending(x => x.jogoData));
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
            ViewBag.Campeonato = new SelectList
            (
                db.tbCampeonato.OrderBy(u => u.campeonatoNome),
                "campeonatoId",
                "campeonatoNome"
            );

            ViewBag.Time = new SelectList
            (
                db.tbTime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome"
            );

            ViewBag.Status = new SelectList
            (
                db.tbJogoStatus.OrderBy(u => u.jogoStatusDescricao),
                "jogoStatusId",
                "jogoStatusDescricao"
            );

            return View();
        }

        public ActionResult CreateConfirmed(int campeonatoId, int timeCasaId, int timeVisitanteId, DateTime jogoData, int statusId, bool? copiarJogadorTimeCasa, bool? copiarJogadorTimeVisitante)
        {
            try
            {
                tbJogo jogo = new tbJogo();
                jogo.timeCasaId = timeCasaId;
                jogo.timeVisitanteId = timeVisitanteId;
                jogo.jogoData = jogoData;

                var status = db.tbJogoStatus.Find(statusId);
                jogo.tbJogoStatus = status;

                var campeonato = db.tbCampeonato.Find(campeonatoId);
                jogo.tbCampeonato = campeonato;

                db.tbJogo.Add(jogo);
                db.SaveChanges();

                if (copiarJogadorTimeCasa.HasValue && copiarJogadorTimeCasa.Value)
                {
                    this.AdicionarJogadoresTime(jogo.jogoId, jogo.timeCasaId);
                }

                if (copiarJogadorTimeVisitante.HasValue && copiarJogadorTimeVisitante.Value)
                {
                    this.AdicionarJogadoresTime(jogo.jogoId, jogo.timeVisitanteId);
                }

                ViewBag.Campeonato = new SelectList
                (
                    db.tbCampeonato.ToList().OrderBy(u => u.campeonatoNome),
                    "campeonatoId",
                    "campeonatoNome"
                );

                ViewBag.Time = new SelectList
                (
                    db.tbTime.ToList().OrderBy(u => u.timeNome),
                    "timeId",
                    "timeNome"
                );

                ViewBag.Status = new SelectList
                (
                    db.tbJogoStatus.ToList().OrderBy(u => u.jogoStatusDescricao),
                    "jogoStatusId",
                    "jogoStatusDescricao"
                );

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
            tbJogo jogo = db.tbJogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Campeonato = new SelectList
            (
                db.tbCampeonato.OrderBy(u => u.campeonatoNome),
                "campeonatoId",
                "campeonatoNome",
                jogo.campeonatoId
            );

            ViewBag.TimeCasa = new SelectList
            (
                db.tbTime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome",
                jogo.timeCasaId
            );

            ViewBag.TimeVisitante = new SelectList
            (
                db.tbTime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome",
                jogo.timeVisitanteId
            );

            ViewBag.Status = new SelectList
            (
                db.tbJogoStatus.OrderBy(u => u.jogoStatusDescricao),
                "jogoStatusId",
                "jogoStatusDescricao",
                jogo.tbJogoStatus.jogoStatusId
            );

            return View(jogo);
        }

        public ActionResult EditConfirmed(int jogoId, int campeonatoId, int timeCasaId, int timeVisitanteId, DateTime jogoData, int statusId, bool? copiarJogadorTimeCasa, bool? copiarJogadorTimeVisitante)
        {
            try
            {
                tbJogo jogo = db.tbJogo.Find(jogoId);

                if (jogo == null)
                {
                    throw new Exception(string.Format("|{0}|", "Jogo não encontrado!"));
                }

                jogo.timeCasaId = timeCasaId;
                jogo.timeVisitanteId = timeVisitanteId;
                jogo.jogoData = jogoData;

                var status = db.tbJogoStatus.Find(statusId);
                jogo.tbJogoStatus = status;

                var campeonato = db.tbCampeonato.Find(campeonatoId);
                jogo.tbCampeonato = campeonato;


                if (copiarJogadorTimeCasa.HasValue && copiarJogadorTimeCasa.Value)
                {
                    this.ExcluirJogadoresTime(jogo.jogoId, jogo.timeCasaId);
                    this.AdicionarJogadoresTime(jogo.jogoId, jogo.timeCasaId);
                }

                if (copiarJogadorTimeVisitante.HasValue && copiarJogadorTimeVisitante.Value)
                {
                    this.ExcluirJogadoresTime(jogo.jogoId, jogo.timeVisitanteId);
                    this.AdicionarJogadoresTime(jogo.jogoId, jogo.timeVisitanteId);
                }

                db.Entry(jogo).State = EntityState.Modified;
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
            tbJogo jogo = db.tbJogo.Find(id);
            db.tbJogo.Remove(jogo);
            db.SaveChanges();
            return PartialView();
        }

        #endregion

        #region - Métodos -

        private void ExcluirJogadoresTime(int idJogo, int idTme)
        {
            var jogo = db.tbJogo.Find(idJogo);
            if (jogo != null)
            {
                var jogadores = db.tbJogoJogadorTime.Where(x => x.tbJogo.jogoId == idJogo && x.tbTime.timeId == idTme);
                foreach (var jogador in jogadores)
                {
                    jogo.tbJogoJogadorTime.Remove(jogador);
                }

                db.SaveChanges();
            }
        }

        private void AdicionarJogadoresTime(int idJogo, int idTime)
        {
            var jogadoresTime = db.tbJogador.Where(j => j.tbTime.Any(t => t.timeId == idTime));
            jogadoresTime = jogadoresTime.Where(j => j.jogadorAtivo == true);

            if (jogadoresTime.Count() > 0)
            {
                var jogo = db.tbJogo.Find(idJogo);

                var time = db.tbTime.Find(idTime);

                foreach (var jogador in jogadoresTime)
                {
                    var jogoJogadorTime = new tbJogoJogadorTime();

                    jogoJogadorTime.tbJogo = jogo;
                    jogoJogadorTime.tbTime = time;
                    jogoJogadorTime.tbJogador = jogador;

                    db.tbJogoJogadorTime.Add(jogoJogadorTime);
                }
            }

            db.SaveChanges();
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
