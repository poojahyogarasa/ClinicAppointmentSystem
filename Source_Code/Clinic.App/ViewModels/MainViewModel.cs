using System.Windows.Input;
using Clinic.App.Infrastructure;
using Clinic.App.Services;
using Clinic.Domain.Entities;

namespace Clinic.App.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            IDoctorService doc = new InMemoryDoctorService();
            IPatientService pat = new InMemoryPatientService();
            IAppointmentService appt = new InMemoryAppointmentService();

            // seed so you immediately see data + dropdowns
            doc.Add(new Doctor { Name = "Dr. Siva", Email = "siva@clinic.com", Phone = "0771234567", Specialty = "Cardiology" });
            pat.Add(new Patient { Name = "Anu", Phone = "0778888888", Email = "anu@mail.com", Address = "Jaffna" });

            Dashboard = new DashboardViewModel(doc, pat, appt);
            Doctors = new DoctorsViewModel(doc);
            Patients = new PatientsViewModel(pat);
            Appointments = new AppointmentsViewModel(appt, doc, pat) { IsEditing = true }; // enable pickers

            Current = Dashboard;

            ShowDashboardCommand = new RelayCommand(_ => Current = Dashboard);
            ShowDoctorsCommand = new RelayCommand(_ => Current = Doctors);
            ShowPatientsCommand = new RelayCommand(_ => Current = Patients);
            ShowAppointmentsCommand = new RelayCommand(_ => Current = Appointments);
        }

        private object? _current;
        public object? Current { get => _current; set { _current = value; OnPropertyChanged(); } }

        public DashboardViewModel Dashboard { get; }
        public DoctorsViewModel Doctors { get; }
        public PatientsViewModel Patients { get; }
        public AppointmentsViewModel Appointments { get; }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowDoctorsCommand { get; }
        public ICommand ShowPatientsCommand { get; }
        public ICommand ShowAppointmentsCommand { get; }
    }
}
