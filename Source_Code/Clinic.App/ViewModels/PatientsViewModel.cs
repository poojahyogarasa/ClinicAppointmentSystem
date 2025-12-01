using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Clinic.App.Infrastructure;
using Clinic.App.Services;
using Clinic.Domain.Entities;

namespace Clinic.App.ViewModels
{
    public sealed class PatientsViewModel : ViewModelBase
    {
        private readonly IPatientService _service;

        public PatientsViewModel(IPatientService service)
        {
            _service = service;

            AddCommand = new RelayCommand(_ => Add(), _ => CanAdd());
            UpdateCommand = new RelayCommand(_ => Update(), _ => Selected != null && CanAdd());

            _service.DataChanged += (_, __) => Load();
            Load();
        }

        private string? _name;
        public string? Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string? _phone;
        public string? Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }

        private string? _email;
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string? _address;
        public string? Address { get => _address; set { _address = value; OnPropertyChanged(); } }

        public ObservableCollection<Patient> Patients { get; } = new();

        private Patient? _selected;
        public Patient? Selected
        {
            get => _selected;
            set
            {
                _selected = value; OnPropertyChanged();
                if (value != null)
                {
                    Name = value.Name;
                    Phone = value.Phone;
                    Email = value.Email;
                    Address = value.Address;
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }

        private bool CanAdd() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Phone);

        private void Add()
        {
            _service.Add(new Patient
            {
                Name = Name?.Trim() ?? "",
                Phone = Phone?.Trim() ?? "",
                Email = Email?.Trim() ?? "",
                Address = Address?.Trim() ?? ""
            });
            Clear();
        }

        private void Update()
        {
            if (Selected == null) return;
            Selected.Name = Name?.Trim() ?? "";
            Selected.Phone = Phone?.Trim() ?? "";
            Selected.Email = Email?.Trim() ?? "";
            Selected.Address = Address?.Trim() ?? "";
            _service.Update(Selected);
            Clear();
        }

        private void Clear()
        {
            Name = Phone = Email = Address = string.Empty;
            Selected = null;
        }

        private void Load()
        {
            Patients.Clear();
            foreach (var p in _service.GetAll().ToList())
                Patients.Add(p);
            OnPropertyChanged(nameof(Patients));
        }
    }
}
