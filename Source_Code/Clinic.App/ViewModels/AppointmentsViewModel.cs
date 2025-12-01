using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Clinic.App.Infrastructure;
using Clinic.App.Services;
using Clinic.Domain.Entities;

namespace Clinic.App.ViewModels
{
    // Small DTO for the Upcoming grid (shows names instead of IDs)
    public sealed class AppointmentRow
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Doctor { get; set; } = "";
        public string Patient { get; set; } = "";
    }

    public sealed class AppointmentsViewModel : ViewModelBase
    {
        private readonly IAppointmentService _appt;
        private readonly IDoctorService _doc;
        private readonly IPatientService _pat;

        public AppointmentsViewModel(IAppointmentService a, IDoctorService d, IPatientService p)
        {
            _appt = a; _doc = d; _pat = p;

            _doc.DataChanged += (_, __) => LoadLookups();
            _pat.DataChanged += (_, __) => { LoadLookups(); LoadUpcoming(); };
            _appt.DataChanged += (_, __) => LoadUpcoming();

            NewCommand = new RelayCommand(_ => BeginNew());
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => Selected != null);

            Date = DateTime.Today;
            Time = TimeSpan.FromHours(8);
            IsEditing = true;

            BuildTimeSlots();
            LoadLookups();
            LoadUpcoming();
        }

        public bool IsEditing { get; set; }

        public ObservableCollection<Doctor> Doctors { get; } = new();
        public ObservableCollection<Patient> Patients { get; } = new();
        public ObservableCollection<TimeSpan> TimeSlots { get; } = new();

        // Raw selection (for Delete)
        public Appointment? Selected { get; set; }

        // Display rows for the right-hand grid
        public ObservableCollection<AppointmentRow> Upcoming { get; } = new();

        private Doctor? _selDoc; public Doctor? SelectedDoctor { get => _selDoc; set { _selDoc = value; OnPropertyChanged(); } }
        private Patient? _selPat; public Patient? SelectedPatient { get => _selPat; set { _selPat = value; OnPropertyChanged(); } }
        private DateTime _date; public DateTime Date { get => _date; set { _date = value; OnPropertyChanged(); } }
        private TimeSpan _time; public TimeSpan Time { get => _time; set { _time = value; OnPropertyChanged(); } }

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        private void BuildTimeSlots()
        {
            TimeSlots.Clear();
            for (var h = 8; h <= 17; h++)
            {
                TimeSlots.Add(new TimeSpan(h, 0, 0));
                TimeSlots.Add(new TimeSpan(h, 30, 0));
            }
        }

        private void LoadLookups()
        {
            var keepDoc = SelectedDoctor?.Id;
            var keepPat = SelectedPatient?.Id;

            Doctors.Clear(); foreach (var d in _doc.GetAll()) Doctors.Add(d);
            Patients.Clear(); foreach (var p in _pat.GetAll()) Patients.Add(p);

            SelectedDoctor = Doctors.FirstOrDefault(x => x.Id == keepDoc) ?? Doctors.FirstOrDefault();
            SelectedPatient = Patients.FirstOrDefault(x => x.Id == keepPat) ?? Patients.FirstOrDefault();
        }

        private void LoadUpcoming()
        {
            Upcoming.Clear();
            foreach (var a in _appt.GetAll().OrderBy(x => x.Date).ThenBy(x => x.Time))
            {
                var d = _doc.GetById(a.DoctorId);
                var p = _pat.GetById(a.PatientId);
                Upcoming.Add(new AppointmentRow
                {
                    Id = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    Doctor = d?.Name ?? $"#{a.DoctorId}",
                    Patient = p?.Name ?? $"#{a.PatientId}"
                });
            }
            OnPropertyChanged(nameof(Upcoming));
        }

        private void BeginNew()
        {
            IsEditing = true;
            Date = DateTime.Today;
            Time = TimeSpan.FromHours(8);
            if (SelectedDoctor == null) SelectedDoctor = Doctors.FirstOrDefault();
            if (SelectedPatient == null) SelectedPatient = Patients.FirstOrDefault();
            OnPropertyChanged(nameof(IsEditing));
        }

        private bool CanSave()
            => SelectedDoctor != null && SelectedPatient != null;

        private void Save()
        {
            if (SelectedDoctor == null || SelectedPatient == null) return;

            // === HARD CONFLICT CHECKS ===
            var sameSlot = _appt.GetAll().Any(a =>
                a.Date.Date == Date.Date &&
                a.Time == Time &&
                (a.DoctorId == SelectedDoctor.Id || a.PatientId == SelectedPatient.Id));

            if (sameSlot)
            {
                // MVVM-pure would expose a Message property; using MessageBox for brevity
                MessageBox.Show(
                    "This time slot is already taken by the selected doctor or patient.\n" +
                    "Please pick a different time.",
                    "Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _appt.Add(new Appointment
            {
                DoctorId = SelectedDoctor.Id,
                PatientId = SelectedPatient.Id,
                Date = Date.Date,
                Time = Time
            });

            LoadUpcoming();
        }

        private void Delete()
        {
            if (Selected == null) return;
            _appt.Delete(Selected.Id);
            LoadUpcoming();
        }
    }
}
