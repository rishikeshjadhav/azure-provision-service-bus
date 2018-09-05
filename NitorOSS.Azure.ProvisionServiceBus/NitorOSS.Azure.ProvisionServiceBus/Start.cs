
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NitorOSS.Azure.Logger;
    using System;
    using System.Globalization;

    public class Start
    {
        private static NamespaceManager nameSpaceManager;

        public static void CreateQueue()
        {
            try
            {
                //accept required values for creating queue
                Logger.LogMessage("\nCreating service bus queue...");
                Logger.LogMessage("\nPlease provide the name for queue: ");
                string queueName = Console.ReadLine();

                Logger.LogMessage(string.Format("\nChecking if queue with name {0} already exists in service bus namespace...", queueName));
                if (!nameSpaceManager.QueueExists(queueName))
                {
                    Logger.LogMessage("\nPlease provide max queue size in MB (1024, 2048, 3072, 4096, 5120): ");
                    string maxQueueSize = Console.ReadLine();
                    Logger.LogMessage("\nPlease provide message time to live (in seconds): ");
                    string messageTimeToLive = Console.ReadLine();

                    Logger.LogMessage("\nPlease provide lock duration time (in seconds): ");
                    string lockDuration = Console.ReadLine();

                    Logger.LogMessage("\nDo you want to enable duplicate detection? (y/n): ");
                    string enableDuplicateDetection = Console.ReadLine();
                    bool duplicateDetection = false;
                    string duplicateDetectionTimeWindowInSeconds = "30";
                    if (string.Equals(enableDuplicateDetection, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        duplicateDetection = true;
                        Logger.LogMessage("\nPlease provide duplicate detection time (in seconds): ");
                        duplicateDetectionTimeWindowInSeconds = Console.ReadLine();
                    }

                    Logger.LogMessage("\nDo you want to enable dead lettering on message expiration? (y/n)");
                    string enableDeadLettering = Console.ReadLine();
                    bool deadLettering = false;
                    if (string.Equals(enableDeadLettering, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        deadLettering = true;
                    }

                    Logger.LogMessage("\nDo you want to enable sessions? (y/n)");
                    string enableSession = Console.ReadLine();
                    bool session = false;
                    if (string.Equals(enableSession, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        session = true;
                    }

                    Logger.LogMessage("\nDo you want to enable partitioning? (y/n): ");
                    string enableQueuePartitioning = Console.ReadLine();
                    bool enablePartitioning = false;
                    if (string.Equals(enableQueuePartitioning, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        enablePartitioning = true;
                    }

                    // Create queue description with entered values                   
                    QueueDescription queueDescription = new QueueDescription(queueName);
                    queueDescription.MaxSizeInMegabytes = Convert.ToInt32(maxQueueSize, CultureInfo.InvariantCulture);
                    queueDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(messageTimeToLive, CultureInfo.InvariantCulture));
                    queueDescription.RequiresDuplicateDetection = duplicateDetection;
                    if (queueDescription.RequiresDuplicateDetection)
                    {
                        queueDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(duplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
                    }
                    queueDescription.EnableDeadLetteringOnMessageExpiration = deadLettering;
                    queueDescription.RequiresSession = session;
                    queueDescription.EnablePartitioning = enablePartitioning;

                    // Create topic
                    nameSpaceManager.CreateQueue(queueDescription);
                }
                else
                {
                    Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nQueue with name {0} already exists in service bus namespace\n", queueName));
                }
            }
            catch
            {
                throw;
            }
        }

        static void Main(string[] args)
        {
            Logger.LogMessage("\n Started execution of utility.");
            try
            {
                Logger.LogMessage("Please provide your service bus namespace url: ");
                string serviceBusConnectionString = Console.ReadLine();
                Logger.LogMessage("Provided service bus url :- " + serviceBusConnectionString + "\n\n");
                // Establish connection with service bus
                nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

                Logger.LogMessage("Please select the component to be created \n 1. Queue \n 2. Topic \n 3. Subscription");
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
            Logger.LogMessage("\n Completed execution of utility, press any key to exit.");
            Console.ReadKey();
        }
    }
}