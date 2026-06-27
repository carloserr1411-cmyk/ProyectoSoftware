using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.Models;
using System.Collections.ObjectModel;
using System;

namespace ProyectoSoftware.ViewModels
{
    public partial class IngenieroViewModel : ObservableObject
    {
        // Tabla principal
        [ObservableProperty]
        private ObservableCollection<Actividad> _actividadesAsignadas;

        [ObservableProperty]
        private Actividad _actividadSeleccionada;

        // Formulario flotante de Revisión
        [ObservableProperty]
        private bool _isDialogOpen;

        [ObservableProperty]
        private string _observaciones;

        public IngenieroViewModel()
        {
            CargarActividadesMock();
        }

        private void CargarActividadesMock()
        {
            // Simulamos las tareas que el Líder envió a este ingeniero específico
            /*ActividadesAsignadas = new ObservableCollection<Actividad>
            {
                new Actividad { IdActividad = 1, IdProyecto = "P01", NombreActividad = "Modelo del Dominio", Estado = "En Revisión", IngenieroAsignado = new Usuario { Nombre = "Pablo Daza" } },
                new Actividad { IdActividad = 3, IdProyecto = "P02", NombreActividad = "Diseño de Base de Datos", Estado = "En Revisión", IngenieroAsignado = new Usuario { Nombre = "Luis Torres" } }
            };*/
        }

        [RelayCommand]
        private void AbrirDialogoRevision()
        {
            if (ActividadSeleccionada != null)
            {
                // Limpiamos observaciones previas y abrimos el diálogo
                Observaciones = string.Empty;
                IsDialogOpen = true;
            }
        }

        [RelayCommand]
        private void CerrarDialogo()
        {
            IsDialogOpen = false;
        }

        // Usamos un solo comando que recibe el veredicto como parámetro desde el XAML
        [RelayCommand]
        private void EmitirVeredicto(string veredicto)
        {
            if (string.IsNullOrWhiteSpace(Observaciones) && veredicto == "Corrección")
            {
                // Regla de negocio: Si rechaza, debe justificar obligatoriamente
                System.Diagnostics.Debug.WriteLine("Error: Debe escribir observaciones si solicita una corrección.");
                return;
            }

            // Aquí guardarías la entidad "Revision" en Entity Framework
            System.Diagnostics.Debug.WriteLine($"Veredicto: {veredicto} | Observaciones: {Observaciones}");

            // Simulamos que la tarea desaparece de su bandeja de pendientes
            ActividadesAsignadas.Remove(ActividadSeleccionada);

            IsDialogOpen = false;
        }
    }
}