
namespace AzsunaBOT.Helpers
{
    public static class RemainingTimeCalculator
    {
        public static int CalculateRemainingMinTime(int minTimeInMinutes, int elapsedMilliseconds)
        {
            var minutes = TimeConverter.GetMinutesFromMilliseconds(elapsedMilliseconds);

            var remainingTime = minTimeInMinutes - minutes;

            return remainingTime;
        }

    }
}
