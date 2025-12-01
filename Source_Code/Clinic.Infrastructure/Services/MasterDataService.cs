using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain.Entities;
using Clinic.Domain.Interfaces;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services;

public class MasterDataService : IMasterDataService
{
    private readonly AppDbContext _db;
    public MasterDataService(AppDbContext db) => _db = db;

    // Doctors
    public Task<List<Doctor>> GetDoctorsAsync() =>
        _db.Doctors.Where(d => d.IsActive).OrderBy(d => d.Name).ToListAsync();

    public async Task<Doctor?> AddDoctorAsync(Doctor d)
    {
        _db.Doctors.Add(d);
        await _db.SaveChangesAsync();
        return d;
    }

    public async Task UpdateDoctorAsync(Doctor d)
    {
        _db.Doctors.Update(d);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        var d = await _db.Doctors.FindAsync(id);
        if (d is not null) { d.IsActive = false; await _db.SaveChangesAsync(); }
    }

    // Patients
    public Task<List<Patient>> GetPatientsAsync() =>
        _db.Patients.Where(p => p.IsActive).OrderBy(p => p.FullName).ToListAsync();

    public async Task<Patient?> AddPatientAsync(Patient p)
    {
        _db.Patients.Add(p);
        await _db.SaveChangesAsync();
        return p;
    }

    public async Task UpdatePatientAsync(Patient p)
    {
        _db.Patients.Update(p);
        await _db.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(int id)
    {
        var p = await _db.Patients.FindAsync(id);
        if (p is not null) { p.IsActive = false; await _db.SaveChangesAsync(); }
    }
}
