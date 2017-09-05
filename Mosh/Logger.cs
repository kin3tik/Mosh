using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosh
{
    public class Logger
    {

        public static void Error(string message)
        {
            Error(null, message);
        }

        public static void Error(Exception e, string message = null)
        {
            Console.WriteLine("****************************");
            Console.WriteLine($"ERROR: {message}");
            if (e != null)
                Console.WriteLine($"{e.Message}");
            Console.WriteLine("****************************");
        }

    }
}
