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
    public class AutorizacaoController : Controller
    {
        #region - Variáveis -

        private Contexto db = new Contexto();

        #endregion

        #region - Actions -

        public ActionResult ListarUsuariosPendentes(string mensagem)
        {
            if (!string.IsNullOrWhiteSpace(mensagem))
            {
                ViewBag.Mensagem = mensagem;
            }
            return View(db.tbUsuario.Where(x => x.tbUsuarioStatus.usuarioStatusId == (int)Enuns.eStatusUsuario.PendenteAprovacao).OrderByDescending(x => x.usuarioDataSolicitacao));
        }

        public ActionResult AutorizarNegarUsuario(int id, bool autorizar)
        {
            try
            {
                tbUsuario usuario = db.tbUsuario.Find(id);

                tbUsuarioStatus status = null;
                if (autorizar)
                {
                    status = db.tbUsuarioStatus.Find((int)Enuns.eStatusUsuario.OK);
                }
                else
                {
                    status = db.tbUsuarioStatus.Find((int)Enuns.eStatusUsuario.Negado);
                }

                usuario.tbUsuarioStatus = status;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("|{0}|", ex.Message));
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
