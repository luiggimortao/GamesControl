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
    public class TimeController : Controller
    {
        #region - Variáveis -

        private Contexto db = new Contexto();

        #endregion

        #region - Actions -

        public ActionResult Index()
        {
            return View(db.tbtime.ToList().OrderBy(x => x.timeNome));
        }

        public ActionResult Detail(int? id)
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

            return View(time);
        }

        public ActionResult Create()
        {
            ViewBag.Cidade = new SelectList
            (
                db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome"
            );

            ViewBag.Incluido = "N";

            return View();
        }

        [HttpPost]
        public ActionResult Create(string timeNome, int timeCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbtime time = new tbtime();
                time.timeNome = timeNome;
                time.tbcidade = db.tbcidade.Find(timeCidade);

                db.tbtime.Add(time);
                db.SaveChanges();

                if (fileUpload != null)
                {
                    this.ApagarArquivosTime(time.timeId);

                    var caminhoFoto = Path.Combine(Server.MapPath("~/Content/TimeLogo"), time.timeId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto);

                    time.timeCaminhoLogo = string.Format("{0}{1}{2}", "~/Content/TimeLogo/", time.timeId, Path.GetExtension(fileUpload.FileName));
                    db.Entry(time).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.Cidade = new SelectList
                (
                    db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                    "cidadeId",
                    "cidadeNome"
                );

                ViewBag.Incluido = "S";

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
            tbtime time = db.tbtime.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cidade = new SelectList
            (
                db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                "cidadeId",
                "cidadeNome",
                time.tbcidade != null ? time.tbcidade.cidadeId.ToString() : string.Empty
            );

            ViewBag.Alterado = "N";

            return View(time);
        }

        [HttpPost]
        public ActionResult Edit(int timeId, string timeNome, int timeCidade, HttpPostedFileBase fileUpload)
        {
            try
            {
                tbtime time = db.tbtime.Find(timeId);

                if (time == null)
                {
                    throw new Exception(string.Format("|{0}|", "Status não encontrado!"));
                }

                time.timeNome = timeNome;
                time.tbcidade = db.tbcidade.Find(timeCidade);

                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();

                if (fileUpload != null)
                {
                    this.ApagarArquivosTime(time.timeId);

                    var caminhoFoto = Path.Combine(Server.MapPath(Constantes.CAMINHO_LOGOS), time.timeId + Path.GetExtension(fileUpload.FileName));
                    fileUpload.SaveAs(caminhoFoto);

                    time.timeCaminhoLogo = string.Format("{0}{1}{2}", Constantes.CAMINHO_LOGOS, time.timeId, Path.GetExtension(fileUpload.FileName));
                    db.Entry(time).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.Cidade = new SelectList
                (
                    db.tbcidade.ToList().OrderBy(p => p.cidadeNome),
                    "cidadeId",
                    "cidadeNome",
                    time.tbcidade != null ? time.tbcidade.cidadeId.ToString() : string.Empty
                );

                ViewBag.Alterado = "S";
                return View(time);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tbtime time = db.tbtime.Find(id);
                this.ExcluirJogadores(id);
                db.tbtime.Remove(time);
                db.SaveChanges();

                this.ApagarArquivosTime(id);

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
            }
        }

        #endregion

        #region - Métodos -

        private tbjogador AdicionarJogador(tbusuario usuario, DateTime dataNascimento, int idCidade)
        {
            tbjogador jogador = new tbjogador();
            jogador.jogadorDataNascimento = dataNascimento;
            jogador.cidadeId = idCidade;
            jogador.usuarioId = usuario.usuarioId;

            db.tbjogador.Add(jogador);
            db.SaveChanges();

            return jogador;
        }

        private void ExcluirJogadores(int idTime)
        {
            var time = db.tbtime.Find(idTime);
            if (time != null)
            {
                while (time.tbjogador.Count > 0)
                {
                    time.tbjogador.Remove(time.tbjogador.FirstOrDefault());
                }
            }

            db.SaveChanges();
        }

        private void ApagarArquivosTime(int idTime)
        {
            string pattern = string.Format("{0}.*", idTime);
            foreach (string file in Directory.GetFiles(Server.MapPath(Constantes.CAMINHO_LOGOS), pattern))
            {
                System.IO.File.Delete(file);
            }
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
