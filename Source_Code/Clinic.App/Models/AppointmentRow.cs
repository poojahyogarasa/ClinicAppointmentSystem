using System;

namespace Clinic.App.Models
{
    public class AppointmentRow
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Start { get; set; }
        public string Notes { get; set; } = "";
    }
}
