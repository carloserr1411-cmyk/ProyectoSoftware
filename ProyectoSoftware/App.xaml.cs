using ProyectoSoftware.DataAccess;
using ProyectoSoftware.Services;
using ProyectoSoftware.ViewModels;
using ProyectoSoftware.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ProyectoSoftware
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new RevisionTecnicaContext())
            {
                // 1. Borra cualquier rastro de archivos corruptos o vacíos de 0 bytes
                context.Database.EnsureDeleted();

                // 2. Lee tus DbSet y tu OnModelCreating, y genera físicamente 
                // todas las tablas (Usuarios, Proyectos, etc.) con sus datos semilla
                context.Database.EnsureCreated();
            }

            // 1. Instanciamos el servicio
            NavigationService navigationService = new NavigationService();

            // 2. Definimos la vista inicial (Login)
            navigationService.CurrentViewModel = new LoginViewModel(navigationService);

            // 3. Inicializamos el MainViewModel y la ventana
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationService)
            };

            MainWindow.Show();
        }
    }

}
