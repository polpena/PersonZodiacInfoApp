using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using PersonInfoApp.Models;

namespace PersonInfoApp.Services
{
    /// <summary>
    /// Provides methods to load and save user data.
    /// </summary>
    public static class UserDataService
    {
        private static readonly string FilePath = "users.json";

        /// <summary>
        /// Loads users from a JSON file. If the file does not exist, generates 50 sample users.
        /// </summary>
        public static async Task<ObservableCollection<Person>> LoadUsersAsync()
        {
            List<Person> users;

            if (!File.Exists(FilePath))
            {
                users = GenerateSampleUsers();
                SaveUsers(users);
            }
            else
            {
                string json = await File.ReadAllTextAsync(FilePath);
                users = JsonSerializer.Deserialize<List<Person>>(json);
            }

            foreach (var user in users)
            {
                user.ComputeProperties();
            }

            return new ObservableCollection<Person>(users);
        }

        /// <summary>
        /// Generates 50 sample users with random data.
        /// </summary>
        private static List<Person> GenerateSampleUsers()
        {
            var users = new List<Person>();
            var rand = new Random();

            string[] firstNames = new[]
            {
                "John", "Alice", "Michael", "Sarah", "Robert", "Emily", "David", "Jessica", "Ivan",
                "Taras", "Oleh", "Andriy", "Anna", "Mark", "Laura", "Peter", "Victoria", "Emma",
                "Liam", "Sophia", "James", "Mia", "Daniel", "Chloe", "Andrew", "Isabella", "Victor",
                "Maria", "Alex", "Nathan", "Brian", "Lily", "Dmitry", "Kate", "Samuel"
            };

            string[] lastNames = new[]
            {
                "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller", "Davis", "Garcia",
                "Rodriguez", "Wilson", "Kovalenko", "Petruk", "Shevchenko", "Bondarenko", "Melnyk",
                "Khomenko", "Holub", "Shevchuk", "Petrov", "Koval", "Kalashnyk", "Melnychuk",
                "Martin", "Wright", "King", "Scott", "Green", "Young", "Hall", "Turner",
                "Polishchuk", "Petrenko", "Novak", "Mitchell", "Rericha"
            };

            string[] emailProviders = new[]
            { "example.com", "domain.net", "gmail.com", "outlook.com", "yahoo.com", "ukr.net", "mydomain.com" };

            for (int i = 1; i <= 50; i++)
            {
                string firstName = firstNames[rand.Next(firstNames.Length)];
                string lastName = lastNames[rand.Next(lastNames.Length)];
                int randomNumber = rand.Next(100, 1000);
                string provider = emailProviders[rand.Next(emailProviders.Length)];
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}{randomNumber}@{provider}";

                int year = rand.Next(1940, 2012);
                int month = rand.Next(1, 13);
                int day = rand.Next(1, 29); // use 1 to 28 to ensure validity
                DateTime birthDate = new DateTime(year, month, day);

                try
                {
                    var person = new Person(firstName, lastName, email, birthDate);
                    users.Add(person);
                }
                catch (Exception)
                {
                    // Skip invalid entries
                    continue;
                }
            }

            return users;
        }

        /// <summary>
        /// Saves the given list of users to a JSON file
        /// </summary>
        public static void SaveUsers(List<Person> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
