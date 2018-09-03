
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Start
    {
        public static void CreateQueue()
        {

            Console.WriteLine("\nCreating service bus queue......");
            Console.WriteLine("\nPlease provide the name for queue: ");
            string serviceBusQueueName = Console.ReadLine();

            Console.WriteLine("\n\nEnter max queue size:- \n Available Options \n 1. 1GB \n 2. 2GB \n 3. 3GB \n 4. 4GGB \n 5. 5GB ");
            int maxQueueSize = int.Parse(Console.ReadLine());

            Console.WriteLine("\n\nPlease select info for message time to live: ");
            Console.WriteLine("\nNumber of days: ");
            int msgTimeToLiveDays = int.Parse(Console.ReadLine());
            Console.WriteLine("\nNumber of hours: ");
            int msgTimeToLiveHours = int.Parse(Console.ReadLine());
            Console.WriteLine("\nNumber of minutes: ");
            int msgTimeToLiveMinutes = int.Parse(Console.ReadLine());
            Console.WriteLine("\nNumber of seconds: ");
            int msgTimeToLiveSeconds = int.Parse(Console.ReadLine());

        }

        public static void CreateTopic()
        {

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please provide your service bus namespace url: ");
            string servicBusConnectionString = Console.ReadLine();
            Console.WriteLine("Provided service bus url :- " + servicBusConnectionString + "\n\n");
            Console.WriteLine("Please select the component to be created \n 1. Queue \n 2. Topic");
            int selectedComponent = int.Parse(Console.ReadLine());

            switch (selectedComponent)
            {
                case 1:
                    {
                        CreateQueue();
                        break;
                    }
                case 2:
                    {
                        CreateTopic();
                        break;
                    }
            }
            Console.ReadKey();
            Console.WriteLine("");
        }
    }
}