using System;
using System.IO;
using System.Net;
using System.Text;


namespace SetiINTERPOL
{
    class Program
    {
        public delegate void Message(string someString);
        
        static void Main(string[] args)
        {
            Message message = Text;

            message("-- WELCOME TO THE HACKER SIMULATOR --\n" +
                "-- PLEASE ENTER NAME & SURNAME OF THE WANTED PERSON --");
            message("NAME:");
            string name = Console.ReadLine();
            message("SURNAME: ");
            string surname = Console.ReadLine();

            MyJsonInteraction interaction = new MyJsonInteraction(name, surname);
            interaction.ShowWantedPersons();

            Console.ReadKey();
        }

        private static void Text(string text)
        {
            Console.WriteLine(text);
        }
    }
}
