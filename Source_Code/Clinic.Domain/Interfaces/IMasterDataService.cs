using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic.Domain.Entities;

namespace Clinic.Domain.Interfaces;

public interface IMasterDataService
{
    // Doctor management
    Task<List<Doctor>> GetDoctorsAsync();
    Task<Doctor?> AddDoctorAsync(Doctor d);
    Task UpdateDoctorAsync(Doctor d);
    Task DeleteDoctorAsync(int id);

    // Patient management
    Task<List<Patient>> GetPatientsAsync();
    Task<Patient?> AddPatientAsync(Patient p);
    Task UpdatePatientAsync(Patient p);
    Task DeletePatientAsync(int id);
}
