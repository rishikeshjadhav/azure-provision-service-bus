
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NitorOSS.Azure.Logger;
    using System;
    using System.Globalization;

    public class Queue
    {
        private static NamespaceManager nameSpaceManager;

        public string Name;

        public Queue(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string queueName)
        {
            bool result = false;
            if (nameSpaceManager.QueueExists(queueName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nQueue with name {0} already exists in service bus namespace\n", queueName));
            }
            return result;
        }

        private QueueDescription GenerateQueueDescription(string queueName)
        {
            // Accept required details for creating queue
            // Maximum Queue size
            Logger.LogMessage("Please provide max queue size in MB (1024, 2048, 3072, 4096, 5120): ");
            string maxQueueSize = Console.ReadLine();
            // Message time to live
            Logger.LogMessage("Please provide message time to live (in seconds): ");
            string messageTimeToLive = Console.ReadLine();
            //Lock duration time
            Logger.LogMessage("Please provide lock duration time (in seconds): ");
            string lockDurationTime = Console.ReadLine();
            // Enable duplicate detection
            bool duplicateDetection = Helper.GetBooleanResponse("Do you want to enable duplicate detection? (y/n): ");
            string duplicateDetectionTimeWindowInSeconds = "30";
            if (duplicateDetection)
            {
                Logger.LogMessage("Please provide duplicate detection time (in seconds): ");
                duplicateDetectionTimeWindowInSeconds = Console.ReadLine();
            }
            //Enable dead lettering on message expiration
            bool deadLettering = Helper.GetBooleanResponse("\nDo you want to enable dead lettering on message expiration? (y/n)");
            bool session = Helper.GetBooleanResponse("\nDo you want to enable sessions? (y/n): ");
            // Enable partitioning
            bool enablePartitioning = Helper.GetBooleanResponse("Do you want to enable partitioning? (y/n): ");
            // Create queue description with provided details
            QueueDescription queueDescription = new QueueDescription(Name);
            queueDescription.MaxSizeInMegabytes = Convert.ToInt32(maxQueueSize, CultureInfo.InvariantCulture);
            queueDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(messageTimeToLive, CultureInfo.InvariantCulture));
            queueDescription.LockDuration = new TimeSpan(0, 0, 0, Convert.ToInt32(lockDurationTime, CultureInfo.InvariantCulture));
            queueDescription.RequiresDuplicateDetection = duplicateDetection;
            if (queueDescription.RequiresDuplicateDetection)
            {
                queueDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(duplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
            }
            queueDescription.EnableDeadLetteringOnMessageExpiration = deadLettering;
            queueDescription.RequiresSession = session;
            queueDescription.EnablePartitioning = enablePartitioning;
            return queueDescription;
        }

        public bool Create()
        {
            bool result = true;
            try
            {
                Logger.LogMessage("Creating service bus queue...");
                Logger.LogMessage("Please provide the name for queue: ");
                Name = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if queue with name {0} already exists in service bus namespace...", Name));
                if (!CheckIfExists(Name))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating queue with name {0} in service bus namespace", Name));
                    // Generate queue description
                    QueueDescription queueDescription = GenerateQueueDescription(Name);
                    // Create queue
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating queue with name {0} in service bus namespace...", Name));
                    nameSpaceManager.CreateQueue(queueDescription);
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Queue with name {0} created in service bus namespace", Name));
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
                throw;
            }
            return result;
        }
    }
}
