namespace Ecommerce.Application.UnitTests.Helpers;

internal class SystemTimeUtc
{
    private static DateTime _dateTime;

    public static void Set(DateTime custom) => _dateTime = custom;

    public static void Reset() => _dateTime = DateTime.MinValue;

    public static DateTime Now
    {
        get
        {
            if (_dateTime != DateTime.MinValue)
            {
                return _dateTime;
            }

            return DateTime.UtcNow;
        }
    }
}
