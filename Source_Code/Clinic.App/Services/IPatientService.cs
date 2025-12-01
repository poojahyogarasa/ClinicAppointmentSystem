using System;
using System.Collections.Generic;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public interface IPatientService
    {
        // Notify listeners (Dashboard, PatientsViewModel, etc.) whenever data changes
        event EventHandler? DataChanged;

        IEnumerable<Patient> GetAll();
        Patient? GetById(int id);

        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(int id);

        int Count();
    }
}
