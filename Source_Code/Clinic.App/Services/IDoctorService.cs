using System;
using System.Collections.Generic;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public interface IDoctorService
    {
        event EventHandler? DataChanged;
        IEnumerable<Doctor> GetAll();
        Doctor? GetById(int id);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(int id);
        int Count();
    }
}