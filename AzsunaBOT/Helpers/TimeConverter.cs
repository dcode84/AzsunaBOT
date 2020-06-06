
namespace AzsunaBOT.Helpers
{
    public static class TimeConverter
    {
        public static int GetSecondsFromMilliseconds(int milliSeconds)
        {
            var seconds = milliSeconds / 1000;

            return seconds;
        }

        public static int GetMinutesFromSeconds(int seconds)
        {
            var minutes = seconds / 60;

            return minutes;
        }

        public static int GetMinutesFromMilliseconds(int milliseconds)
        {
            var seconds = GetSecondsFromMilliseconds(milliseconds);

            var minutes = GetMinutesFromSeconds(seconds);

            return minutes;
        }

    }
}
