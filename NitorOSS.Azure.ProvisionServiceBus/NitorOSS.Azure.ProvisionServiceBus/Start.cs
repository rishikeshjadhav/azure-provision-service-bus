
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using NitorOSS.Azure.Logger;
    using System;

    class Start
    {
        public static void CreateQueue()
        {
            Logger.LogMessage("\nCreating service bus queue......");
            Logger.LogMessage("\nPlease provide the name for queue: ");
            string serviceBusQueueName = Console.ReadLine();

            Logger.LogMessage("\n\nEnter max queue size:- \n Available Options \n 1. 1GB \n 2. 2GB \n 3. 3GB \n 4. 4GGB \n 5. 5GB ");
            int maxQueueSize = int.Parse(Console.ReadLine());

            Logger.LogMessage("\n\nPlease select info for message time to live: ");
            Logger.LogMessage("\nNumber of days: ");
            int msgTimeToLiveDays = int.Parse(Console.ReadLine());
            Logger.LogMessage("\nNumber of hours: ");
            int msgTimeToLiveHours = int.Parse(Console.ReadLine());
            Logger.LogMessage("\nNumber of minutes: ");
            int msgTimeToLiveMinutes = int.Parse(Console.ReadLine());
            Logger.LogMessage("\nNumber of seconds: ");
            int msgTimeToLiveSeconds = int.Parse(Console.ReadLine());

        }

        public static void CreateTopic()
        {

        }

        static void Main(string[] args)
        {
            try
            {
                Logger.LogMessage("Please provide your service bus namespace url: ");
                string servicBusConnectionString = Console.ReadLine();
                Logger.LogMessage("Provided service bus url :- " + servicBusConnectionString + "\n\n");
                Logger.LogMessage("Please select the component to be created \n 1. Queue \n 2. Topic");
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
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }

            Console.ReadKey();
            Logger.LogMessage(string.Empty);
        }
    }
}