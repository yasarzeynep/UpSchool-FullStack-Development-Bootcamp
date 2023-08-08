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
        public static void FirstMessage()
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("Website logged in");

        }
        public static void PrintLoginMessage()
        {
            Console.WriteLine("************************************************");
            Console.WriteLine("Website logged in");

        }
        public static void ScrapingQuestion()
        {

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("What type of products do you want to crawler?");
            Console.WriteLine("You can choose one option from three options");
            Console.WriteLine("Please enter: A,B or C options.");
            Console.WriteLine("A= All Products, B= On Sale Products, C= Regular Price Products")
                //public static readonly string ScrapeRequest = "How many products do you want to scrape";
                //public static readonly string ProductType = "What products do you want to scrape";

                }


        public static void EmailMessage()
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Do you want to receive the crawled products by email?");
            Console.WriteLine("Please enter 'Y' for Yes or 'N' for No:");

        }
        public static void EmailAdressMessage()
        {
            Console.WriteLine("Please enter your email address to receive the results:");
        }

        public static void ScrapingContinue()
        {

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("Do you want to continue product crawler? ");
            Console.WriteLine("Please enter 'Y' for Yes or 'N' for No:");
        }


    }
    public static class Answers
    {

        public static readonly string Type = "";

    }
    public static class Errors
    {
        public static readonly string InvalidInput = "ERROR:You entered an invalid option.";

    }


}






