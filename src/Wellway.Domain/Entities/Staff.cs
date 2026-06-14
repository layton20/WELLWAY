using Wellway.Domain.Common;
using Wellway.Domain.Enums;

namespace Wellway.Domain.Entities;

public sealed class Staff : BaseEntity
{
    public string UserId { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string JobTitle { get; private set; } = string.Empty;
    public StaffRole StaffRole { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    public string? PhoneExtension { get; private set; }
    public bool IsActive { get; private set; } = true;

    public string FullName => $"{FirstName} {LastName}";

    private readonly List<Appointment> _appointments = [];
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private Staff() { }

    public Staff(
        string userId,
        string firstName,
        string lastName,
        string jobTitle,
        StaffRole staffRole,
        Guid departmentId,
        string? phoneExtension)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        StaffRole = staffRole;
        DepartmentId = departmentId;
        PhoneExtension = phoneExtension;
    }

    public void UpdateDetails(string firstName, string lastName, string jobTitle, string? phoneExtension)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        PhoneExtension = phoneExtension;
        UpdateTimestamp();
    }

    public void UpdateRole(StaffRole role)
    {
        StaffRole = role;
        UpdateTimestamp();
    }

    public void UpdateDepartment(Guid departmentId)
    {
        DepartmentId = departmentId;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }
}
