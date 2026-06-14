using Wellway.Domain.Common;

namespace Wellway.Domain.Entities;

public sealed class Department : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? PhoneExtension { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<Staff> _staffMembers = [];
    private readonly List<Appointment> _appointments = [];

    public IReadOnlyCollection<Staff> StaffMembers => _staffMembers.AsReadOnly();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private Department() { }

    public Department(string name, string? description, string? phoneExtension)
    {
        Name = name;
        Description = description;
        PhoneExtension = phoneExtension;
    }

    public void UpdateDetails(string name, string? description, string? phoneExtension)
    {
        Name = name;
        Description = description;
        PhoneExtension = phoneExtension;
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
