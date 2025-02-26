namespace PersonInfoApp.Models
{
    /// <summary>
    /// Provides methods for calculating Western and Chinese zodiac signs.
    /// </summary>
    public static class ZodiacCalculator
    {
        /// <summary>
        /// Calculates the Western zodiac sign based on the given birth date.
        /// Uses typical zodiac boundaries:
        /// - Capricorn: December 22 - January 19
        /// - Aquarius: January 20 - February 18
        /// - Pisces: February 19 - March 20
        /// - Aries: March 21 - April 19
        /// - Taurus: April 20 - May 20
        /// - Gemini: May 21 - June 20
        /// - Cancer: June 21 - July 22
        /// - Leo: July 23 - August 22
        /// - Virgo: August 23 - September 22
        /// - Libra: September 23 - October 22
        /// - Scorpio: October 23 - November 21
        /// - Sagittarius: November 22 - December 21
        /// </summary>
        public static string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day;
            int month = birthDate.Month;

            if ((month == 1 && day <= 19) || (month == 12 && day >= 22))
                return "Capricorn";
            if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
                return "Aquarius";
            if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
                return "Pisces";
            if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
                return "Aries";
            if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
                return "Taurus";
            if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
                return "Gemini";
            if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
                return "Cancer";
            if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
                return "Leo";
            if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
                return "Virgo";
            if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
                return "Libra";
            if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
                return "Scorpio";
            if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
                return "Sagittarius";

            return "Unknown";
        }

        /// <summary>
        /// Calculates the Chinese zodiac sign based on the given year.
        /// </summary>
        public static string GetChineseZodiac(int year)
        {
            // The array order corresponds to the cycle starting with Monkey
            string[] animals = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            return animals[year % 12];
        }
    }
}
