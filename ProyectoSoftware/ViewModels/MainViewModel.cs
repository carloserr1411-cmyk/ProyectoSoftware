using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoSoftware.Services;

namespace ProyectoSoftware.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;

        public ObservableObject CurrentViewModel => (ObservableObject)_navigationService.CurrentViewModel;

        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            // Nos suscribimos a los cambios de navegación del servicio
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            // Esta línea sigue siendo necesaria y correcta. 
            // Le avisa al XAML que vuelva a leer la propiedad 'CurrentViewModel'
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}