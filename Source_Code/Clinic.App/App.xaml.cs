using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Clinic.App.Services;
using Clinic.App.ViewModels;
using Clinic.App.Views;

namespace Clinic.App
{
    public partial class App : Application
    {
        public ServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var sc = new ServiceCollection();

            // Core services
            sc.AddSingleton<IDoctorService, InMemoryDoctorService>();
            sc.AddSingleton<IPatientService, InMemoryPatientService>();
            sc.AddSingleton<IAppointmentService, InMemoryAppointmentService>();
            sc.AddSingleton<IAuthService, AuthService>();

            // ViewModels
            sc.AddSingleton<MainViewModel>();                 // keep app state
            sc.AddTransient<DoctorsViewModel>();
            sc.AddTransient<PatientsViewModel>();
            sc.AddTransient<AppointmentsViewModel>();
            sc.AddTransient<DashboardViewModel>();
            sc.AddTransient<LoginViewModel>(sp => new LoginViewModel(sp));

            Services = sc.BuildServiceProvider();

            // Open login ONCE (no StartupUri anywhere)
            var login = new LoginWindow
            {
                DataContext = Services.GetRequiredService<LoginViewModel>()
            };
            Current.MainWindow = login;
            login.Show();
        }
    }
}
