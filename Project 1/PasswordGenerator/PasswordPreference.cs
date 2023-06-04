using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PasswordGenerator
{
    public class PasswordPreference
    {
        private static string UpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string LowerLetters = "abcdefghijklmnopqursuvwxyz";
        private static string Numbers = "123456789";
        private static string SpecialChars = @"!@£$%^&*()#€";
        private static string GeneratedPassword;
        public PasswordPreference()
        {

        }
        public void Greetings()
        {
            Console.WriteLine("*************************************************");
            Console.WriteLine("      Welcome to the Best Password Manager!      ");
            Console.WriteLine("*************************************************");
        }
        public void GeneratePasswordAnswer()
        {
            Console.WriteLine(Questions.IncludeNumbers);
            bool numbers = ReadAnswers(Console.ReadLine());

            Console.WriteLine(Questions.IncludeLowercaseCharacters);
            bool lowers = ReadAnswers(Console.ReadLine());

            Console.WriteLine(Questions.IncludeUppercaseCharacters);
            bool uppers = ReadAnswers(Console.ReadLine());

            Console.WriteLine(Questions.IncludeSpecialCharacters);
            bool specials = ReadAnswers(Console.ReadLine());

            Console.WriteLine(Questions.PasswordLength);
            int passwordLength = Convert.ToInt32(Console.ReadLine());


            GeneratedPassword = GeneratePassword(uppers, lowers, numbers, specials, passwordLength);

        }
        public bool ReadAnswers(string answer)

        {
            switch (answer)
            {
                case "Y":
                case "y":
                    return true;
                case "N":
                case "n":
                    return false;
                default:
                    Console.WriteLine(Errors.InvalidAnswer);
                    answer = Console.ReadLine();

                    return ReadAnswers(answer);
            }

        }


        public static string GeneratePassword(bool UpperLetter, bool LowerLetter, bool Number, bool Special, int PasswordLength)
        {
            Random randPasword = new Random();
            string passwordSet = string.Empty;
            char[] password = new char[PasswordLength];

            if (UpperLetter) passwordSet = passwordSet + UpperLetters;
            if (LowerLetter) passwordSet = passwordSet + UpperLetters.ToLower();
            if (Number) passwordSet = passwordSet + Numbers;
            if (Special) passwordSet = passwordSet + SpecialChars;

            for (int i = 0; i < PasswordLength; i++)
            {
                password[i] = passwordSet[randPasword.Next(passwordSet.Length - 1)];
            }
            return string.Join(null, password);
        }

        public void ShowGeneratedPassword()
        {

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("       Generated Password is here               ");
            Console.WriteLine(GeneratedPassword);
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------");
        }

    }
}
