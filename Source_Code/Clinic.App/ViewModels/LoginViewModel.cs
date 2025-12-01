using System;                           // <-- add this line
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Clinic.App.Infrastructure;
using Clinic.App.Services;
using Clinic.App.Views;
using Microsoft.Extensions.DependencyInjection;


namespace Clinic.App.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _sp;
        private readonly IAuthService _auth;

        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel(IServiceProvider sp)
        {
            _sp = sp;
            _auth = _sp.GetRequiredService<IAuthService>();
            LoginCommand = new RelayCommand(_ => DoLogin());
        }

        private void DoLogin()
        {
            if (_auth.Login(Username, Password))
            {
                var main = new MainWindow
                {
                    DataContext = _sp.GetRequiredService<MainViewModel>()
                };

                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = main;
                main.Show();
            }
            else
            {
                MessageBox.Show("Invalid credentials (try admin / 1234).",
                                "Login",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
