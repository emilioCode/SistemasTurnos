//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemasTurnos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Turnos
    {
        public int id { get; set; }
        public int TipoServicio { get; set; }
        public Nullable<int> Turno { get; set; }
        public string Fecha { get; set; }
        public string Hora_inicio { get; set; }
        public string Hora_fin { get; set; }
        public Nullable<bool> Despachado { get; set; }
        public Nullable<bool> Habilitado { get; set; }
        public Nullable<bool> atendido { get; set; }
        public Nullable<int> usuario { get; set; }
    }
}
