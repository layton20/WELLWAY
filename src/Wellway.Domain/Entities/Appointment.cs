using Wellway.Domain.Common;
using Wellway.Domain.Enums;

namespace Wellway.Domain.Entities;

public sealed class Appointment : AuditableEntity
{
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public Guid StaffId { get; private set; }
    public Staff Staff { get; private set; } = null!;
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    public AppointmentType AppointmentType { get; private set; }
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;
    public DateTimeOffset ScheduledAt { get; private set; }
    public int DurationInMinutes { get; private set; }
    public string? Room { get; private set; }
    public string ReasonForVisit { get; private set; } = string.Empty;
    public string? CancellationReason { get; private set; }
    public string? Notes { get; private set; }

    private Appointment() { }

    public Appointment(
        string createdBy,
        Guid patientId,
        Guid staffId,
        Guid departmentId,
        AppointmentType appointmentType,
        DateTimeOffset scheduledAt,
        int durationInMinutes,
        string reasonForVisit,
        string? room = null)
        : base(createdBy)
    {
        PatientId = patientId;
        StaffId = staffId;
        DepartmentId = departmentId;
        AppointmentType = appointmentType;
        Status = AppointmentStatus.Scheduled;
        ScheduledAt = scheduledAt;
        DurationInMinutes = durationInMinutes;
        ReasonForVisit = reasonForVisit;
        Room = room;
    }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Scheduled)
            throw new InvalidOperationException(
                $"Cannot confirm an appointment with status {Status}.");

        Status = AppointmentStatus.Confirmed;
        UpdateTimestamp();
    }

    public void Cancel(string reason)
    {
        Status = AppointmentStatus.Cancelled;
        CancellationReason = reason;
        UpdateTimestamp();
    }

    public void Complete()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new InvalidOperationException(
                $"Cannot complete an appointment with status {Status}.");

        Status = AppointmentStatus.Completed;
        UpdateTimestamp();
    }

    public void MarkAsNoShow()
    {
        Status = AppointmentStatus.NoShow;
        UpdateTimestamp();
    }

    public void Reschedule(DateTimeOffset newScheduledAt)
    {
        if (Status is AppointmentStatus.Cancelled or AppointmentStatus.Completed)
            throw new InvalidOperationException(
                $"Cannot reschedule an appointment with status {Status}.");

        ScheduledAt = newScheduledAt;
        Status = AppointmentStatus.Scheduled;
        UpdateTimestamp();
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        UpdateTimestamp();
    }
}
