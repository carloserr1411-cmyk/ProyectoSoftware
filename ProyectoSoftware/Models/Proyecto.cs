using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoSoftware.Models
{
    public class Proyecto
    {
        public required string IdProyecto { get; set; } // PK tipo string (Ej: "P01")
        public string? Descripcion { get; set; }
        public required string Estado { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Actividad> Actividades { get; set; }

        public Proyecto()
        {
            Actividades = new HashSet<Actividad>();
        }
    }
}
