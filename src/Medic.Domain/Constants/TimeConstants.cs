namespace Medic.Domain.Constants;

public static class TimeConstants
{
    public static int UTC = 5;

    public static DateTime GetNow()
        => DateTime.UtcNow.AddHours(UTC);
}