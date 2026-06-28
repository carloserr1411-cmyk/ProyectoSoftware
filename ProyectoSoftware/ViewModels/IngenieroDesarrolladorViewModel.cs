using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.DataAccess;
using ProyectoSoftware.Models;
using System;
using System.Collections.ObjectModel;

namespace ProyectoSoftware.ViewModels
{
    public partial class IngenieroDesarrolladorViewModel : ObservableObject
    {
        private readonly RevisionTecnicaContext _context;

        [ObservableProperty]
        private ObservableCollection<Actividad>? _misActividades;

        [ObservableProperty]
        private Actividad? _actividadSeleccionada;

        public IngenieroDesarrolladorViewModel()
        {
            _context = new RevisionTecnicaContext();
            CargarMisActividadesMock();
        }

        private void CargarMisActividadesMock()
        {
            // Simulamos las tareas asignadas específicamente a este ingeniero
            /*MisActividades = new ObservableCollection<Actividad>
            {
                new Actividad
                {
                    IdProyecto = "P01",
                    NombreActividad = "Modelo del Dominio",
                    Estado = "Pendiente",
                    FechaRecepcion = DateTime.Now.AddDays(-2)
                },
                new Actividad
                {
                    IdProyecto = "P01",
                    NombreActividad = "Interfaz de Login",
                    Estado = "En Corrección",
                    FechaRecepcion = DateTime.Now.AddDays(-1)
                }
            };*/
        }

        private void CargarMisActividadesReales()
        {
            // Simulamos que el ingeniero con ID 1 inició sesión (cambiarás esto luego con el login real)
            int idIngenieroLogueado = 1;

            var actividades = _context.Actividades
                .Include(a => a.Proyecto)
                .Where(a => a.IdIngenieroAsignado == idIngenieroLogueado &&
                           (a.Estado == "Pendiente" || a.Estado == "En Corrección"))
                .ToList();

            MisActividades = new ObservableCollection<Actividad>(actividades);
        }

        [RelayCommand]
        private void CulminarActividad()
        {
            if (ActividadSeleccionada != null &&
               (ActividadSeleccionada.Estado == "Pendiente" || ActividadSeleccionada.Estado == "En Corrección"))
            {
                using (var context = new RevisionTecnicaContext())
                {
                    // 1. Buscamos la actividad exacta en la base de datos
                    var actividadDb = context.Actividades
                        .FirstOrDefault(a => a.IdActividad == ActividadSeleccionada.IdActividad);

                    if (actividadDb != null)
                    {
                        // 2. Modificamos los valores
                        actividadDb.Estado = "Terminada";
                        actividadDb.FechaCulminacion = DateTime.Now;

                        // 3. Guardamos los cambios en SQL Server
                        context.SaveChanges();

                        // 4. Actualizamos la vista localmente
                        ActividadSeleccionada.Estado = "Terminada";
                        ActividadSeleccionada.FechaCulminacion = actividadDb.FechaCulminacion;

                        // Refrescamos el DataGrid
                        CargarMisActividadesReales();
                    }
                }
            }
        }
    }
}