using System;
using System.Collections.Generic;
using System.Linq;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public sealed class InMemoryAppointmentService : IAppointmentService
    {
        private readonly List<Appointment> _items = new();
        public event EventHandler? DataChanged;

        private void Notify() => DataChanged?.Invoke(this, EventArgs.Empty);

        public IEnumerable<Appointment> GetAll() =>
            _items.OrderBy(a => a.Date.Date)
                  .ThenBy(a => a.Time)
                  .ToList();

        public Appointment? GetById(int id) => _items.FirstOrDefault(a => a.Id == id);

        public int Count() => _items.Count;

        public int CountOnDate(DateTime date)
        {
            var d = date.Date;
            return _items.Count(a => a.Date.Date == d);
        }

        public Appointment? NextUpcoming(DateTime fromDate)
        {
            var d0 = fromDate.Date;
            var t0 = fromDate.TimeOfDay;

            // strictly future relative to "fromDate":
            //  - any appointment on a later day
            //  - or same day but at or after the given time
            return _items
                .Where(a =>
                    a.Date.Date > d0 ||
                    (a.Date.Date == d0 && a.Time >= t0))
                .OrderBy(a => a.Date.Date)
                .ThenBy(a => a.Time)
                .FirstOrDefault();
        }


        public void Add(Appointment a)
        {
            if (a == null) return;
            if (a.Id == 0)
                a.Id = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;

            _items.Add(a);
            Notify();
        }

        public void Update(Appointment a)
        {
            var ex = GetById(a.Id);
            if (ex == null) return;

            ex.DoctorId = a.DoctorId;
            ex.PatientId = a.PatientId;
            ex.Date = a.Date;
            ex.Time = a.Time;

            Notify();
        }

        public void Delete(int id)
        {
            var ex = GetById(id);
            if (ex != null)
            {
                _items.Remove(ex);
                Notify();
            }
        }
    }
}
