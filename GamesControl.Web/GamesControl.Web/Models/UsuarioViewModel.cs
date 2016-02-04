using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesControl.Web.Models
{
    public class UsuarioViewModel
    {
        #region - Propriedades -

        private tbUsuario _usuario;
        public tbUsuario Usuario
        {
            get
            {
                if (_usuario == null)
                {
                    _usuario = new tbUsuario();
                }
                return _usuario;
            }
            set
            {
                this._usuario = value;
            }
        }

        private tbJogador _jogador;
        public tbJogador Jogador
        {
            get
            {
                if (_jogador == null)
                {
                    _jogador = new tbJogador();
                }
                return _jogador;
            }
            set
            {
                this._jogador = value;
            }
        }

        private tbArbitro _arbitro;
        public tbArbitro Arbitro
        {
            get
            {
                if (_arbitro == null)
                {
                    _arbitro = new tbArbitro();
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