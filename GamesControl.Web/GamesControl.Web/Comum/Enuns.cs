using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamesControl.Web.Comum
{
    public class Enuns
    {
        public enum ePerfilUsuario
        {
            Administrador = 1,
            Jogador = 2,
            Arbitro = 3,
            Visitante = 4
        }

        public enum eStatusUsuario
        {
            OK = 1,
            PendenteAprovacao = 2,
            Inativo = 3,
            Banido = 4
        }
    }
}