using System;
using Clinic.App.Infrastructure;
using Clinic.App.Services;

namespace Clinic.App.ViewModels
{
    public sealed class DashboardViewModel : ViewModelBase
    {
        private readonly IDoctorService _doc;
        private readonly IPatientService _pat;
        private readonly IAppointmentService _appt;

        public DashboardViewModel(IDoctorService d, IPatientService p, IAppointmentService a)
        {
            _doc = d; _pat = p; _appt = a;
            _doc.DataChanged += (_, __) => Refresh();
            _pat.DataChanged += (_, __) => Refresh();
            _appt.DataChanged += (_, __) => Refresh();
            Refresh();
        }

        private int _doctorCount, _patientCount, _todayCount;
        private string _next = "None";

        public int DoctorCount { get => _doctorCount; private set { _doctorCount = value; OnPropertyChanged(); } }
        public int PatientCount { get => _patientCount; private set { _patientCount = value; OnPropertyChanged(); } }
        public int TodayCount { get => _todayCount; private set { _todayCount = value; OnPropertyChanged(); } }
        public string NextAppointmentText { get => _next; private set { _next = value; OnPropertyChanged(); } }

        public void Refresh()
        {
            var today = DateTime.Today;
            DoctorCount = _doc.Count();
            PatientCount = _pat.Count();
            TodayCount = _appt.CountOnDate(today);

            // Use current date & time so "Next Appointment"
            // really means the next future slot.
            var now = DateTime.Now;
            var next = _appt.NextUpcoming(now);

            NextAppointmentText = next == null ? "None"
                : $"{next.Date:yyyy-MM-dd} {next.Time:hh\\:mm}";
        }

    }
}
