using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace ProyectoSoftware.Services
{
    public class NavigationService
    {
        public event Action? CurrentViewModelChanged;

        private ObservableObject? _currentViewModel;

        public ObservableObject? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}