using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.DataAccess;
using ProyectoSoftware.Models;
using System.Collections.ObjectModel;

namespace ProyectoSoftware.ViewModels
{
    public partial class LiderViewModel : ObservableObject
    {
        // Lista observable para que el DataGrid se actualice automáticamente
        [ObservableProperty]
        private ObservableCollection<Actividad>? _actividadesTerminadas;

        [ObservableProperty]
        private Actividad? _actividadSeleccionada;

        [ObservableProperty]
        private ObservableCollection<Usuario>? _ingenierosDisponibles;

        // Los dos ingenieros que el líder seleccionará en el ComboBox
        [ObservableProperty]
        private Usuario? _revisor1Seleccionado;

        [ObservableProperty]
        private Usuario? _revisor2Seleccionado;

        // Controla si el cuadro de diálogo está abierto o cerrado
        [ObservableProperty]
        private bool _isDialogOpen;

        public LiderViewModel()
        {
            // Simulamos datos de la base de datos por ahora
            CargarActividadesReales();
            CargarIngenierosMock();
        }

        private void CargarActividadesReales()
        {
            using (var context = new RevisionTecnicaContext())
            {
                // Traemos solo las actividades que están "Terminadas"
                // y cargamos la información del Ingeniero asociado.
                var actividades = context.Actividades
                    .Include(a => a.IngenieroAsignado)
                    .Where(a => a.Estado == "Terminada")
                    .ToList();

                // Actualizamos nuestra propiedad observable
                ActividadesTerminadas = new ObservableCollection<Actividad>(actividades);
            }
        }

        private void CargarIngenierosMock()
        {
            // Simulamos a los ingenieros de la base de datos
            /*IngenierosDisponibles = new ObservableCollection<Usuario>
            {
                new Usuario { IdUsuario = 2, Nombre = "Pedro Pereza", Rol = "Ingeniero" },
                new Usuario { IdUsuario = 3, Nombre = "Ana Campos", Rol = "Ingeniero" },
                new Usuario { IdUsuario = 4, Nombre = "Luis Torres", Rol = "Ingeniero" }
            };*/
        }

        [RelayCommand]
        private void AbrirDialogoAsignacion()
        {
            if (ActividadSeleccionada != null)
            {
                // Limpiamos selecciones anteriores
                Revisor1Seleccionado = null;
                Revisor2Seleccionado = null;

                // Abrimos el diálogo cambiando el flag booleano
                IsDialogOpen = true;
            }
        }

        [RelayCommand]
        private void CerrarDialogo()
        {
            IsDialogOpen = false;
        }

        [RelayCommand]
        private void ConfirmarAsignacion()
        {
            // Validaciones de negocio antes de asignar
            if (Revisor1Seleccionado == null || Revisor2Seleccionado == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: Debes seleccionar a 2 revisores.");
                return;
            }

            if (Revisor1Seleccionado.IdUsuario == Revisor2Seleccionado.IdUsuario)
            {
                System.Diagnostics.Debug.WriteLine("Error: Los revisores deben ser personas distintas.");
                return;
            }

            // Aquí iría tu lógica de Entity Framework para guardar la revisión
            System.Diagnostics.Debug.WriteLine($"Revisión asignada a {Revisor1Seleccionado.Nombre} y {Revisor2Seleccionado.Nombre} para la actividad {ActividadSeleccionada.NombreActividad}");

            // Cambiamos el estado de la actividad (Mock)
            ActividadSeleccionada.Estado = "En Revisión";

            // Refrescamos la UI y cerramos el diálogo
            OnPropertyChanged(nameof(ActividadesTerminadas));
            IsDialogOpen = false;
        }
    }
}