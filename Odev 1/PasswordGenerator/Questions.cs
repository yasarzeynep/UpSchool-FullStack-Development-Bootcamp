using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public static class Questions
    {
        public static string IncludeNumbers => "Hello, Do you want to Include Numbers? (Y/N)";
        public static string IncludeLowercaseCharacters => "OK! How about lowercase characters? (Y/N)";
        public static string IncludeUppercaseCharacters => "Very nice! How about uppercase characters? (Y/N)";
        public static string IncludeSpecialCharacters => "All right! We are almost done. Would you also want to add special characters? (Y/N)";
        public static string PasswordLength => "Great! Lastly. How long do you want to keep your password length?";
    }
    // Error messsages added
    public static class Errors
    {
        public static string InvalidAnswer = "ERROR: Invalid Answer! You must say yes or no to the answers";

    }
}
