//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GamesControl.Web
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbPerfil
    {
        public tbPerfil()
        {
            this.tbUsuario = new HashSet<tbUsuario>();
        }
    
        public int perfilId { get; set; }
        public string perfilDescricao { get; set; }
    
        public virtual ICollection<tbUsuario> tbUsuario { get; set; }
    }
}
