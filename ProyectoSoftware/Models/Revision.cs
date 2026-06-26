using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoSoftware.Models
{
    public class Revision
    {
        public int IdRevision { get; set; }
        public int IdActividad { get; set; }
        public int IdIngenieroRevisor { get; set; }
        public string? Observaciones { get; set; }
        public required string Veredicto { get; set; } // "Aprobado", "Correccion"
        public DateTime? FechaRevision { get; set; }

        // Propiedades de navegación
        public required virtual Actividad Actividad { get; set; }
        public required virtual Usuario IngenieroRevisor { get; set; }
    }
}
