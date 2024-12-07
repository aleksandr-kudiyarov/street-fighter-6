namespace Kudiyarov.StreetFighter6.Common.Entities;

public readonly record struct Percentage(double Value)
{
    public static implicit operator double(Percentage value)
    {
        return value.Value;
    }

    public static implicit operator Percentage(double value)
    {
        return new Percentage(value);
    }
}