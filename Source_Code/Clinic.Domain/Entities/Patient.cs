// Clinic.Domain/Entities/Patient.cs
namespace Clinic.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }                     // <- use this as the primary key
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // OPTIONAL: if some old code still uses PatientId, safely map it to Id
        public int PatientId
        {
            get => Id;
            set => Id = value;
        }
    }
}
