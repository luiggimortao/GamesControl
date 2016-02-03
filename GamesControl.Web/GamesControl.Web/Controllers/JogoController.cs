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
            return View(db.tbjogo.ToList().OrderByDescending(x => x.jogoData));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbusuariostatus tbusuariostatus = db.tbusuariostatus.Find(id);
            if (tbusuariostatus == null)
            {
                return HttpNotFound();
            }
            return View(tbusuariostatus);
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
                db.tbtime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome"
            );

            ViewBag.Status = new SelectList
            (
                db.tbjogostatus.OrderBy(u => u.jogoStatusDescricao),
                "jogoStatusId",
                "jogoStatusDescricao"
            );

            return View();
        }

        public ActionResult CreateConfirmed(int campeonatoId, int timeCasaId, int timeVisitanteId, DateTime jogoData, int statusId, bool? copiarJogadorTimeCasa, bool? copiarJogadorTimeVisitante)
        {
            try
            {
                tbjogo jogo = new tbjogo();
                jogo.timeCasaId = timeCasaId;
                jogo.timeVisitanteId = timeVisitanteId;
                jogo.jogoData = jogoData;

                var status = db.tbjogostatus.Find(statusId);
                jogo.tbjogostatus = status;

                var campeonato = db.tbCampeonato.Find(campeonatoId);
                jogo.tbCampeonato = campeonato;

                db.tbjogo.Add(jogo);
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
                    db.tbtime.ToList().OrderBy(u => u.timeNome),
                    "timeId",
                    "timeNome"
                );

                ViewBag.Status = new SelectList
                (
                    db.tbjogostatus.ToList().OrderBy(u => u.jogoStatusDescricao),
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
            tbjogo jogo = db.tbjogo.Find(id);
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
                db.tbtime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome",
                jogo.timeCasaId
            );

            ViewBag.TimeVisitante = new SelectList
            (
                db.tbtime.OrderBy(u => u.timeNome),
                "timeId",
                "timeNome",
                jogo.timeVisitanteId
            );

            ViewBag.Status = new SelectList
            (
                db.tbjogostatus.OrderBy(u => u.jogoStatusDescricao),
                "jogoStatusId",
                "jogoStatusDescricao",
                jogo.tbjogostatus.jogoStatusId
            );

            return View(jogo);
        }

        public ActionResult EditConfirmed(int jogoId, int campeonatoId, int timeCasaId, int timeVisitanteId, DateTime jogoData, int statusId, bool? copiarJogadorTimeCasa, bool? copiarJogadorTimeVisitante)
        {
            try
            {
                tbjogo jogo = db.tbjogo.Find(jogoId);

                if (jogo == null)
                {
                    throw new Exception(string.Format("|{0}|", "Jogo não encontrado!"));
                }

                jogo.timeCasaId = timeCasaId;
                jogo.timeVisitanteId = timeVisitanteId;
                jogo.jogoData = jogoData;

                var status = db.tbjogostatus.Find(statusId);
                jogo.tbjogostatus = status;

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
            tbjogo jogo = db.tbjogo.Find(id);
            db.tbjogo.Remove(jogo);
            db.SaveChanges();
            return PartialView();
        }
        
        #endregion

        #region - Métodos -

        private void ExcluirJogadoresTime(int idJogo, int idTme)
        {
            var jogo = db.tbjogo.Find(idJogo);
            if (jogo != null)
            {
                var jogadores = db.tbJogoJogadorTime.Where(x => x.tbjogo.jogoId == idJogo && x.tbtime.timeId == idTme);
                foreach (var jogador in jogadores)
                {
                    jogo.tbJogoJogadorTime.Remove(jogador);
                }

                db.SaveChanges();
            }
        }

        private void AdicionarJogadoresTime(int idJogo, int idTime)
        {
            var jogadoresTime = from jogador in db.tbjogador
                                where jogador.tbtime.Any(t => t.timeId == idTime)
                                select jogador;

            if (jogadoresTime.Count() > 0)
            {
                var jogo = db.tbjogo.Find(idJogo);

                var time = db.tbtime.Find(idTime);

                foreach (var jogador in jogadoresTime)
                {
                    var jogoJogadorTime = new tbJogoJogadorTime();

                    jogoJogadorTime.tbjogo = jogo;
                    jogoJogadorTime.tbtime = time;
                    jogoJogadorTime.tbjogador = jogador;

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
