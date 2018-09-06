
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using NitorOSS.Azure.Logger;
    using System;

    public class Start
    {
        static void Main(string[] args)
        {
            Logger.LogMessage("\nStarted execution of utility.");
            try
            {
                Logger.LogMessage("Please provide your service bus namespace url: ");
                string serviceBusConnectionString = Console.ReadLine();
                Logger.LogMessage("Provided service bus url :- " + serviceBusConnectionString + "\n\n");
                Logger.LogMessage("Please select the component to be created \n 1. Queue \n 2. Topic \n 3. Subscription");
                int selectedComponent = int.Parse(Console.ReadLine());
                switch (selectedComponent)
                {
                    case 1:
                        {
                            Queue queue = new Queue(serviceBusConnectionString);
                            queue.Create();
                            break;
                        }
                    case 2:
                        {
                            Topic topic = new Topic(serviceBusConnectionString);
                            if (topic.Create())
                            {
                                if (Helper.GetBooleanResponse("Do you want to create subscriptions for this topic? (y/n): "))
                                {
                                    Subscription subscription = new Subscription(serviceBusConnectionString);
                                    subscription.Create(topic.Name);
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            Topic topic = new Topic(serviceBusConnectionString);
                            Logger.LogMessage("Please provide the name for topic under which subscription is to be created: ");
                            topic.Name = Console.ReadLine();
                            if (!topic.CheckIfExists(topic.Name))
                            {
                                Subscription subscription = new Subscription(serviceBusConnectionString);
                                subscription.Create(topic.Name);
                            }
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
            Logger.LogMessage("\nCompleted execution of utility, press any key to exit.");
            Console.ReadKey();
        }
    }
}