using System;
namespace webserver
{
    class Program
    {
        static void Main()
        {
            var server = new SimpleWebServer("http://localhost:60000/");
            server.Start();

            Console.WriteLine("Press Enter to stop the server");
            Console.ReadLine();

            server.Stop();
        }
    }
}
