using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace PasswordGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            var generator = new PasswordPreference();
            generator.Greetings();
            generator.GeneratePasswordAnswer();
            generator.ShowGeneratedPassword();
            Console.ReadLine();

        }
    }
}