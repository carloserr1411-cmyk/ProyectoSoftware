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
