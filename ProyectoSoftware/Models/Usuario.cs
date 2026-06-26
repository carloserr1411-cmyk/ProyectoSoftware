using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoSoftware.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; } // "Gerente", "Lider", "Ingeniero"

        // Propiedades de navegación
        public virtual ICollection<Actividad> ActividadesAsignadas { get; set; }
        public virtual ICollection<Revision> RevisionesAsignadas { get; set; }

        public Usuario()
        {
            ActividadesAsignadas = new HashSet<Actividad>();
            RevisionesAsignadas = new HashSet<Revision>();
        }
    }
}
