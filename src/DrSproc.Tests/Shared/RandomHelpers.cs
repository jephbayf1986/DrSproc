using System;

namespace DrSproc.Tests.Shared
{
    internal static class RandomHelpers
    {
        public static int IntBetween(int a, int b)
        {
            var randGen = new Random();

            return randGen.Next(a, b);
        }

        public static DateTime DateInPast(int numDaysInPast)
        {
            var randGen = new Random();

            var daysInPast = randGen.Next(0, numDaysInPast);

            return DateTime.Now.AddDays((0 - daysInPast));
        }

        public static string RandomString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}