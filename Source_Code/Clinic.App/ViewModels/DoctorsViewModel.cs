using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Clinic.App.Infrastructure;
using Clinic.App.Services;
using Clinic.Domain.Entities;

namespace Clinic.App.ViewModels
{
    public sealed class DoctorsViewModel : ViewModelBase
    {
        private readonly IDoctorService _service;

        public DoctorsViewModel(IDoctorService service)
        {
            _service = service;

            AddCommand = new RelayCommand(_ => Add(), _ => CanAdd());
            UpdateCommand = new RelayCommand(_ => Update(), _ => Selected != null && CanAdd());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => Selected != null);
            ClearCommand = new RelayCommand(_ => Clear());
            ReloadCommand = new RelayCommand(_ => Load());

            _service.DataChanged += (_, __) => Load();
            Load();
        }

        // bound to TextBoxes
        private string? _name;
        public string? Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string? _email;
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string? _phone;
        public string? Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }

        private string? _specialty;
        public string? Specialty { get => _specialty; set { _specialty = value; OnPropertyChanged(); } }

        // grid
        public ObservableCollection<Doctor> Doctors { get; } = new();

        private Doctor? _selected;
        public Doctor? Selected
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); }
        }

        // commands
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ReloadCommand { get; }

        // called by code-behind on DataGrid.SelectionChanged
        public void LoadFromSelection()
        {
            if (Selected == null) return;
            Name = Selected.Name;
            Email = Selected.Email;
            Phone = Selected.Phone;
            Specialty = Selected.Specialty;
        }

        private bool CanAdd() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Phone) &&
            !string.IsNullOrWhiteSpace(Specialty);

        private void Add()
        {
            _service.Add(new Doctor
            {
                Name = Name?.Trim() ?? "",
                Email = Email?.Trim() ?? "",
                Phone = Phone?.Trim() ?? "",
                Specialty = Specialty?.Trim() ?? ""
            });
            Clear();
        }

        private void Update()
        {
            if (Selected == null) return;
            Selected.Name = Name?.Trim() ?? "";
            Selected.Email = Email?.Trim() ?? "";
            Selected.Phone = Phone?.Trim() ?? "";
            Selected.Specialty = Specialty?.Trim() ?? "";
            _service.Update(Selected);
            Clear();
        }

        private void Delete()
        {
            if (Selected == null) return;
            _service.Delete(Selected.Id);
            Clear();
        }

        private void Clear()
        {
            Name = Email = Phone = Specialty = string.Empty;
            Selected = null;
        }

        private void Load()
        {
            Doctors.Clear();
            foreach (var d in _service.GetAll().ToList())
                Doctors.Add(d);

            // keep selection if possible
            if (Selected != null)
                Selected = Doctors.FirstOrDefault(x => x.Id == Selected.Id);
            OnPropertyChanged(nameof(Doctors));
        }
    }
}
