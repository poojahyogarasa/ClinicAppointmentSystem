namespace Clinic.Domain.Entities
{
    public sealed class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        // WPF DatePicker binds to DateTime
        public DateTime Date { get; set; }

        // Store time-of-day separately
        public TimeSpan Time { get; set; }
    }
}
