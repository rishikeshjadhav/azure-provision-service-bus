namespace NitorOSS.Azure.Logger
{
    using System;

    public static class Logger
    {
        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(string error)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Error Occurred: " + error);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }

        public static void LogException(Exception exception)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Exception Occurred: " + exception.Message);
            Console.WriteLine("Stack Trace: " + exception.StackTrace);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }
    }
}
