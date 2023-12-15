namespace PostMortemTests.Services;

public class ClockService : IClockService
{
    public DateTime CalculateReceiveAt(string address, DateTime placedAt) => DateTime.Now.AddDays(2);
    public bool IsPlacedAtValid(DateTime placedAt, out DateTime? dateTimeNow)
    {
        if (placedAt > DateTime.Now)
        {
            dateTimeNow = DateTime.Now;
            return false;
        }
        else
        {
            dateTimeNow = null;
            return true;
        }
    }
}

public interface IClockService
{
    bool IsPlacedAtValid(DateTime placedAt, out DateTime? dateTimeNow);
    DateTime CalculateReceiveAt(string address, DateTime placedAt);
}
