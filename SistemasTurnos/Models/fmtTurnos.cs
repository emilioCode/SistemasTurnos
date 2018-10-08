using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemasTurnos.Models
{
    public class fmtTurnos
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
        public int no { get; set; }
       
    }
}