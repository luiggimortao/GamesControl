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
    
    public partial class tbarbitro
    {
        public tbarbitro()
        {
            this.tbjogo = new HashSet<tbjogo>();
        }
    
        public int arbitroId { get; set; }
        public string arbitroNome { get; set; }
        public Nullable<int> usuarioId { get; set; }
    
        public virtual tbusuario tbusuario { get; set; }
        public virtual ICollection<tbjogo> tbjogo { get; set; }
    }
}
