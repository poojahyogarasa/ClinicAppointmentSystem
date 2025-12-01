using System;
using System.Collections.Generic;
using System.Linq;
using Clinic.Domain.Entities;

namespace Clinic.App.Services
{
    public sealed class InMemoryPatientService : IPatientService
    {
        private readonly List<Patient> _items = new();

        public event EventHandler? DataChanged;

        public IEnumerable<Patient> GetAll()
            => _items.OrderBy(p => p.Id).ToList();

        public Patient? GetById(int id)
            => _items.FirstOrDefault(p => p.Id == id);

        public void Add(Patient p)
        {
            if (p == null) return;
            if (p.Id == 0)
            {
                p.Id = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;
            }
            _items.Add(p);
            OnDataChanged();
        }

        public void Update(Patient p)
        {
            if (p == null) return;
            var idx = _items.FindIndex(x => x.Id == p.Id);
            if (idx < 0) return;
            _items[idx] = p;
            OnDataChanged();
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item == null) return;
            _items.Remove(item);
            OnDataChanged();
        }

        public int Count() => _items.Count;

        private void OnDataChanged()
            => DataChanged?.Invoke(this, EventArgs.Empty);
    }
}
