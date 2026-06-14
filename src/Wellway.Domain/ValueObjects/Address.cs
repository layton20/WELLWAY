namespace Wellway.Domain.ValueObjects;

public sealed class Address
{
    public string Line1 { get; private set; } = string.Empty;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string? County { get; private set; }
    public string PostCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    private Address() { }

    public Address(
        string line1,
        string? line2,
        string city,
        string? county,
        string postCode,
        string? country = null)
    {
        Line1 = line1;
        Line2 = line2;
        City = city;
        County = county;
        PostCode = postCode.Trim().ToUpperInvariant();
        Country = country ?? "United Kingdom";
    }

    public override bool Equals(object? obj) =>
        obj is Address other &&
        Line1 == other.Line1 &&
        Line2 == other.Line2 &&
        City == other.City &&
        County == other.County &&
        PostCode == other.PostCode &&
        Country == other.Country;

    public override int GetHashCode() =>
        HashCode.Combine(Line1, Line2, City, County, PostCode, Country);

    public static bool operator ==(Address? left, Address? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Address? left, Address? right) =>
        !(left == right);
}
