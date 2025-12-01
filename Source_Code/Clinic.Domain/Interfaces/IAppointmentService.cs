using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinic.Domain.Entities;

namespace Clinic.Domain.Interfaces;

public interface IAppointmentService
{
    Task<List<Appointment>> GetByDateAsync(DateTime day); // view appointments by date
    Task<Appointment?> AddAsync(Appointment a);           // add new (no overlap)
    Task UpdateAsync(Appointment a);                      // update existing
    Task DeleteAsync(int id);                             // delete
}
