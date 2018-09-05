
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NitorOSS.Azure.Logger;
    using System;
    using System.Globalization;

    public class Topic
    {
        private static NamespaceManager nameSpaceManager;

        public string Name;

        public Topic(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string topicName)
        {
            bool result = false;
            if (nameSpaceManager.TopicExists(topicName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "\nTopic with name {0} already exists in service bus namespace\n", topicName));
            }
            return result;
        }

        private TopicDescription GenerateTopicDescription(string topicName)
        {
            // Accept required details for creating topic
            // Maximum Topic size
            Logger.LogMessage("Please provide max topic size in MB (1024, 2048, 3072, 4096, 5120): ");
            string maxTopicSize = Console.ReadLine();
            // Message time to live
            Logger.LogMessage("Please provide message time to live (in seconds): ");
            string messageTimeToLive = Console.ReadLine();
            // Enable duplicate detection
            bool duplicateDetection = Helper.GetBooleanResponse("Do you want to enable duplicate detection? (y/n): ");
            string duplicateDetectionTimeWindowInSeconds = "30";
            if (duplicateDetection)
            {
                Logger.LogMessage("Please provide duplicate detection time (in seconds): ");
                duplicateDetectionTimeWindowInSeconds = Console.ReadLine();
            }
            // Enable partitioning
            bool enablePartitioning = Helper.GetBooleanResponse("Do you want to enable partitioning? (y/n): ");
            // Create topic description with provided details
            TopicDescription topicDescription = new TopicDescription(Name);
            topicDescription.MaxSizeInMegabytes = Convert.ToInt32(maxTopicSize, CultureInfo.InvariantCulture);
            topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(messageTimeToLive, CultureInfo.InvariantCulture));
            topicDescription.RequiresDuplicateDetection = duplicateDetection;
            if (topicDescription.RequiresDuplicateDetection)
            {
                topicDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(duplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
            }
            topicDescription.EnablePartitioning = enablePartitioning;
            return topicDescription;
        }

        public bool Create()
        {
            bool result = true;
            try
            {
                Logger.LogMessage("Creating service bus topic...");
                Logger.LogMessage("Please provide the name for topic: ");
                Name = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if topic with name {0} already exists in service bus namespace...", Name));
                if (!CheckIfExists(Name))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Topic with name {0} in service bus namespace", Name));
                    // Generate Topic description
                    TopicDescription topicDescription = GenerateTopicDescription(Name);
                    // Create topic
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating topic with name {0} in service bus namespace...", Name));
                    nameSpaceManager.CreateTopic(topicDescription);
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Topic with name {0} created in service bus namespace", Name));
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
