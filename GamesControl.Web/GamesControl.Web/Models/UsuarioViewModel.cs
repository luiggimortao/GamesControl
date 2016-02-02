using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesControl.Web.Models
{
    public class UsuarioViewModel
    {
        #region - Propriedades -

        private tbusuario _usuario;
        public tbusuario Usuario
        {
            get
            {
                if (_usuario == null)
                {
                    _usuario = new tbusuario();
                }
                return _usuario;
            }
            set
            {
                this._usuario = value;
            }
        }

        private tbjogador _jogador;
        public tbjogador Jogador
        {
            get
            {
                if (_jogador == null)
                {
                    _jogador = new tbjogador();
                }
                return _jogador;
            }
            set
            {
                this._jogador = value;
            }
        }

        private tbarbitro _arbitro;
        public tbarbitro Arbitro
        {
            get
            {
                if (_arbitro == null)
                {
                    _arbitro = new tbarbitro();
                }
                return _arbitro;
            }
            set
            {
                this._arbitro = value;
            }
        }

        #endregion
    }
}