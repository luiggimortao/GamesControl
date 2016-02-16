using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesControl.Web.Models
{
    public class UsuarioLogadoViewModel
    {
        #region - Propriedades -

        public string Nome { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }

        private List<int> _listaPerfis;
        public List<int> ListaPerfis
        {
            get
            {
                if (_listaPerfis == null)
                {
                    _listaPerfis = new List<int>();
                }
                return _listaPerfis;
            }
            set
            {
                this._listaPerfis = value;
            }
        }
        #endregion
    }
}