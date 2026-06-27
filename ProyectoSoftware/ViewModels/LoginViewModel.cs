using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.Models;
using ProyectoSoftware.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProyectoSoftware.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;

        // Propiedades enlazadas a los TextBox del XAML (Email y Password)
        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _password;

        public LoginViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void Login()
        {
            // Simulamos el usuario
            Usuario usuarioAutenticado = new Usuario { 
                Rol = "Gerente", 
                Nombre = "Carlos", 
                Email = "carloserr1411@hotmail.com", 
                Password = "1234" 
            };

            if (usuarioAutenticado != null)
            {
                switch (usuarioAutenticado.Rol)
                {
                    case "Gerente":
                        _navigationService.CurrentViewModel = new GerenciaViewModel();
                        break;
                    case "Lider":
                        _navigationService.CurrentViewModel = new LiderViewModel();
                        break;
                    case "Ingeniero":
                        _navigationService.CurrentViewModel = new IngenieroRevisorViewModel();
                        break;
                }
            }
        }
    }
}
