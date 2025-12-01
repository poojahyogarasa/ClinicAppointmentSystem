using System;
using System.Collections.Generic;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public interface IAppointmentService
    {
        event EventHandler? DataChanged;

        IEnumerable<Appointment> GetAll();
        Appointment? GetById(int id);

        void Add(Appointment a);
        void Update(Appointment a);
        void Delete(int id);

        int Count();                                   // total
        int CountOnDate(DateTime date);                // use Date.Date
        Appointment? NextUpcoming(DateTime fromDate);  // >= fromDate (date & time)

    }
}
