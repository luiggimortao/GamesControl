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
    
    public partial class tbtime
    {
        public tbtime()
        {
            this.tbjogo = new HashSet<tbjogo>();
            this.tbjogo1 = new HashSet<tbjogo>();
            this.tbjogador = new HashSet<tbjogador>();
        }
    
        public int timeId { get; set; }
        public string timeNome { get; set; }
        public string timeCaminhoLogo { get; set; }
        public int cidadeId { get; set; }
    
        public virtual ICollection<tbjogo> tbjogo { get; set; }
        public virtual ICollection<tbjogo> tbjogo1 { get; set; }
        public virtual ICollection<tbjogador> tbjogador { get; set; }
        public virtual tbcidade tbcidade { get; set; }
    }
}
