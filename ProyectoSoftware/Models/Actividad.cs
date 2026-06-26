using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoSoftware.Models
{
    public class Actividad
    {
        public int IdActividad { get; set; }
        public required string IdProyecto { get; set; }
        public int IdIngenieroAsignado { get; set; }
        public required string NombreActividad { get; set; }
        public required string Estado { get; set; } // "Pendiente", "Terminada", "EnRevision", etc.
        public DateTime FechaRecepcion { get; set; }
        public DateTime? FechaCulminacion { get; set; } // Nullable porque no se culmina de inmediato

        // Propiedades de navegación
        public required virtual Proyecto Proyecto { get; set; }
        public required virtual Usuario IngenieroAsignado { get; set; }
        public virtual ICollection<Revision> Revisiones { get; set; }

        public Actividad()
        {
            Revisiones = new HashSet<Revision>();
        }
    }
}
