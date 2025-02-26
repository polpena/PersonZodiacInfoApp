using System;

namespace PersonInfoApp.Models
{
    /// <summary>
    /// Exception thrown when the birth date is in the future.
    /// </summary>
    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException()
            : base("Date of birth cannot be in the future.") { }

        public FutureBirthDateException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Exception thrown when the birth date indicates an age that is too old.
    /// </summary>
    public class TooOldBirthDateException : Exception
    {
        public TooOldBirthDateException()
            : base("Date of birth is too old. We process only alive people :/") { }

        public TooOldBirthDateException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Exception thrown when the email address is invalid.
    /// </summary>
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("Wrong email address.") { }

        public InvalidEmailException(string message)
            : base(message) { }
    }
}
