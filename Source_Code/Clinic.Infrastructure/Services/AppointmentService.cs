using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain.Entities;
using Clinic.Domain.Interfaces;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;
    public AppointmentService(AppDbContext db) => _db = db;

    public Task<List<Appointment>> GetByDateAsync(DateTime day)
    {
        var start = day.Date;
        var end = start.AddDays(1);
        return _db.Appointments
            .Where(a => a.Start >= start && a.Start < end)
            .OrderBy(a => a.Start)
            .ToListAsync();
    }

    public async Task<Appointment?> AddAsync(Appointment a)
    {
        if (a.End <= a.Start) throw new InvalidOperationException("End time must be after Start time.");

        bool overlaps = await _db.Appointments.AnyAsync(x =>
            x.DoctorId == a.DoctorId &&
            x.Status != "Cancelled" &&
            a.Start < x.End && x.Start < a.End); // interval overlap rule

        if (overlaps) throw new InvalidOperationException("This doctor already has an overlapping appointment.");

        _db.Appointments.Add(a);
        await _db.SaveChangesAsync();
        return a;
    }

    public async Task UpdateAsync(Appointment a)
    {
        if (a.End <= a.Start) throw new InvalidOperationException("End time must be after Start time.");

        bool overlaps = await _db.Appointments.AnyAsync(x =>
            x.Id != a.Id &&
            x.DoctorId == a.DoctorId &&
            x.Status != "Cancelled" &&
            a.Start < x.End && x.Start < a.End);

        if (overlaps) throw new InvalidOperationException("Overlapping appointment exists.");

        _db.Appointments.Update(a);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var appt = await _db.Appointments.FindAsync(id);
        if (appt is not null)
        {
            _db.Appointments.Remove(appt);
            await _db.SaveChangesAsync();
        }
    }
}
