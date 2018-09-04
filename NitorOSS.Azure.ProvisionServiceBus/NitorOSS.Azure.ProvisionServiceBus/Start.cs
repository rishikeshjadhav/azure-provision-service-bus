﻿
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NitorOSS.Azure.Logger;
    using System;
    using System.Globalization;

    class Start
    {
        private static NamespaceManager nameSpaceManager;

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
            try
            {
                Logger.LogMessage("Creating service bus topic...");
                Logger.LogMessage("Please provide the name for topic: ");
                string topicName = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if topic with name {0} already exists in service bus namespace...", topicName));
                if (!nameSpaceManager.TopicExists(topicName))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Topic with name {0} in service bus namespace", topicName));

                    // Accept required values for creating topic
                    Logger.LogMessage("Please provide max topic size in MB (1024, 2048, 3072, 4096, 5120): ");
                    string maxTopicSize = Console.ReadLine();
                    Logger.LogMessage("Please provide message time to live (in seconds): ");
                    string messageTimeToLive = Console.ReadLine();
                    Logger.LogMessage("Do you want to enable duplicate detection? (y/n): ");
                    string enableDuplicateDetection = Console.ReadLine();
                    bool duplicateDetection = false;
                    string duplicateDetectionTimeWindowInSeconds = "30";
                    if (string.Equals(enableDuplicateDetection, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        duplicateDetection = true;
                        Logger.LogMessage("Please provide duplicate detection time (in seconds): ");
                        duplicateDetectionTimeWindowInSeconds = Console.ReadLine();
                    }
                    Logger.LogMessage("Do you want to enable partitioning? (y/n): ");
                    string enableTopicPartitioning = Console.ReadLine();
                    bool enablePartitioning = false;
                    if (string.Equals(enableTopicPartitioning, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        enablePartitioning = true;
                    }

                    // Create topic description with entered values
                    TopicDescription topicDescription = new TopicDescription(topicName);
                    topicDescription.MaxSizeInMegabytes = Convert.ToInt32(maxTopicSize, CultureInfo.InvariantCulture);
                    topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(messageTimeToLive, CultureInfo.InvariantCulture));
                    topicDescription.RequiresDuplicateDetection = Convert.ToBoolean(duplicateDetection, CultureInfo.InvariantCulture);
                    if (topicDescription.RequiresDuplicateDetection)
                    {
                        topicDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(duplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
                    }
                    topicDescription.EnablePartitioning = Convert.ToBoolean(enablePartitioning, CultureInfo.InvariantCulture);

                    // Create topic
                    nameSpaceManager.CreateTopic(topicDescription);

                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Topic with name {0} created in service bus namespace", topicName));
                }
                else
                {
                    Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nTopic with name {0} already exists in service bus namespace\n", topicName));
                }
            }
            catch
            {
                throw;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Logger.LogMessage("Please provide your service bus namespace url: ");
                string serviceBusConnectionString = Console.ReadLine();
                Logger.LogMessage("Provided service bus url :- " + serviceBusConnectionString + "\n\n");
                // Establish connection with service bus
                nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

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