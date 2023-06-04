using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public static class Messages
    {
        public static void PrintWelcomeMessage()
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("Welcome to the Crawler Page");
            Console.WriteLine("************************************************");
        }
        public static void PrintLoginMessage()
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("Website logged in");
           
        }
        public static void ScrapingQuestion()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("What products do you want to scrape?"); 
            Console.WriteLine("You can choose one option from three options");
            Console.WriteLine("1= All Products, 2= On Sale Products, 3= Regular Price Products");
            Console.WriteLine("------------------------------------------------");
            //public static readonly string ScrapeRequest = "How many products do you want to scrape";
            //public static readonly string ProductType = "What products do you want to scrape";


        }

        public static class Answers
        {
 
            public static readonly string Type = "What products do you want to scrape";

        }
        public static class Errors
        {
            public static readonly string InvalidInput = "ERROR:You entered an invalid option.";
          
        }

        
    }
}
