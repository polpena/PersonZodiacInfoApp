using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PersonInfoApp.Models
{
    /// <summary>
    /// Represents a person with basic data and computed properties.
    /// </summary>
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }
        public bool IsAdult { get; private set; }
        public int Age { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        [JsonConstructor]
        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new InvalidEmailException();

            ComputeProperties();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, default) 
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new InvalidEmailException();
        }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, "", birthDate) 
        {
            ComputeProperties();
        }

        /// <summary>
        /// Computes all computed properties for the person.
        /// Must be called after creating the object.
        /// </summary>
        public void ComputeProperties()
        {
            if (BirthDate == default)
                return;

            if (BirthDate > DateTime.Today)
                throw new FutureBirthDateException();

            int age = DateTime.Today.Year - BirthDate.Year;
            if (DateTime.Today < BirthDate.AddYears(age))
                age--;

            if (age >= 135)
                throw new TooOldBirthDateException();

            Age = age;
            IsAdult = Age >= 18;
            SunSign = ZodiacCalculator.GetWesternZodiac(BirthDate);
            ChineseSign = ZodiacCalculator.GetChineseZodiac(BirthDate.Year);
            IsBirthday = (BirthDate.Day == DateTime.Today.Day && BirthDate.Month == DateTime.Today.Month);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
