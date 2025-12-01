using System;
using System.Collections.Generic;
using System.Linq;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public sealed class InMemoryDoctorService : IDoctorService
    {
        private readonly List<Doctor> _items = new();
        public event EventHandler? DataChanged;

        private void Notify() => DataChanged?.Invoke(this, EventArgs.Empty);

        public IEnumerable<Doctor> GetAll() => _items.OrderBy(d => d.Name).ToList();
        public Doctor? GetById(int id) => _items.FirstOrDefault(d => d.Id == id);
        public int Count() => _items.Count;

        public void Add(Doctor doctor)
        {
            if (doctor.Id == 0)
                doctor.Id = _items.Count == 0 ? 1 : _items.Max(d => d.Id) + 1;
            _items.Add(doctor);
            Notify();
        }

        public void Update(Doctor doctor)
        {
            var ex = GetById(doctor.Id);
            if (ex == null) return;
            ex.Name = doctor.Name; ex.Email = doctor.Email; ex.Phone = doctor.Phone; ex.Specialty = doctor.Specialty;
            Notify();
        }

        public void Delete(int id)
        {
            var ex = GetById(id);
            if (ex != null) { _items.Remove(ex); Notify(); }
        }
    }
}
