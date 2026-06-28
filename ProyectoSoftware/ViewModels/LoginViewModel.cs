using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.DataAccess;
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
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                System.Windows.MessageBox.Show("Por favor, ingrese su correo y contraseña.",
                                               "Campos vacíos",
                                               System.Windows.MessageBoxButton.OK,
                                               System.Windows.MessageBoxImage.Warning);

                return;
            }

            try
            {
                // Usamos 'using' para abrir y cerrar la conexión automáticamente
                using (var context = new RevisionTecnicaContext())
                {
                    // 1. Buscamos al usuario en la base de datos real
                    Usuario? usuarioAutenticado = context.Usuarios
                        .FirstOrDefault(u => u.Email == Email && u.Password == Password);

                    // 2. Redirección dinámica según el rol
                    if (usuarioAutenticado != null)
                    {
                        if (usuarioAutenticado.Password != Password)
                        {
                            System.Windows.MessageBox.Show("Contraseña incorrecta.", "Error de Login", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            switch (usuarioAutenticado.Rol)
                            {
                                case "Gerencia":
                                    _navigationService.CurrentViewModel = new GerenciaViewModel();
                                    break;
                                case "Lider":
                                    _navigationService.CurrentViewModel = new LiderViewModel();
                                    break;
                                case "Ingeniero":
                                    _navigationService.CurrentViewModel = new IngenieroDesarrolladorViewModel();
                                    break;
                            }
                        }
                    }
                    else
                    {
                        // Opcional: Aquí podrías mostrar un mensaje de error en la UI 
                        // indicando que el correo o la contraseña son incorrectos.
                        System.Windows.MessageBox.Show("El correo ingresado no existe en el sistema.", "Error de Login", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Atrapar cualquier otro error de conexión para que el programa no se trabe
                System.Windows.MessageBox.Show($"Ocurrió un error al conectar: {ex.Message}");
            }
        }
    }
}
