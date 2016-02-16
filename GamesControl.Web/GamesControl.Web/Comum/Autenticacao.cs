using GamesControl.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GamesControl.Web.Comum
{
    public class Autenticacao
    {
        string _emailLogado = HttpContext.Current.User.Identity.Name;

        public static UsuarioLogadoViewModel GetUsuarioLogado()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
            {
                Deslogar();
                var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var loginUrl = helper.Action ("ExpirarSessao", "Autenticacao");
                HttpContext.Current.Response.Redirect(loginUrl, false);
                return null;
            }
            else
            {
                using (Contexto db = new Contexto())
                {
                    var usuario = db.tbUsuario.FirstOrDefault(x => x.usuarioEmail.Trim() == HttpContext.Current.User.Identity.Name);

                    if (usuario != null)
                    {
                        var usuarioLogado = new UsuarioLogadoViewModel();
                        usuarioLogado.Nome = usuario.usuarioNome;
                        usuario.usuarioEmail = usuario.usuarioEmail;

                        foreach (var perfil in usuario.tbPerfil)
                        {
                            usuarioLogado.ListaPerfis.Add(perfil.perfilId);
                        }

                        return usuarioLogado;
                    }
                }
            }

            return null;
        }

        public static bool IsUsuarioLogado()
        {
            return GetUsuarioLogado() != null;
        }

        public static string AutenticarUsuario(string email, string senha)
        {
            using (Contexto db = new Contexto())
            {
                var usuario = db.tbUsuario.FirstOrDefault(x => x.usuarioEmail.Trim() == email && 
                                                          x.usuarioSenha == senha);
                
                if (usuario != null)
                {
                    if (usuario.usuarioStatusId == (int)Enuns.eStatusUsuario.OK)
                    {
                        FormsAuthentication.SetAuthCookie(usuario.usuarioEmail, false);
                        return string.Empty;
                    }
                    else
                    {
                        return "Usuário não esta ativo no sistema! Entre em contato com o administrador.";
                    }                    
                }
                else
                {
                    return "Usuário ou senha inválido!";
                }
            }
        }

        public static void Deslogar()
        {
            FormsAuthentication.SignOut();
        }
    }
}