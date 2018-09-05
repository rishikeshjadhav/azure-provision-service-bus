
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

        public static void CreateSubscription(string topicName)
        {
            string subscriptionName = string.Empty;
            try
            {
                Logger.LogMessage("Creating Service Bus Topic Subscription...");
                Logger.LogMessage("Please provide the name for Subscription: ");
                subscriptionName = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if Subscription with name {0} already exists under Topic with name {0}...", subscriptionName, topicName));
                if (!nameSpaceManager.SubscriptionExists(topicName, subscriptionName))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Subscription with name {0} under Topic {1} in service bus namespace", subscriptionName, topicName));
                    RuleDescription ruleDescription = null;
                    //if (null != department)
                    //{
                    //    ruleDescription = new RuleDescription()
                    //    {
                    //        Name = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_Rule", topicName, subscriptionName),
                    //        Filter = new SqlFilter(string.Format(CultureInfo.InvariantCulture, "Department = '{0}'", department))
                    //    };
                    //}

                    // Accept required details for creating topic
                    Logger.LogMessage("Please provide the default message time to live (in seconds): ");
                    string defaultMessageTime = Console.ReadLine();
                    Logger.LogMessage("Please provide the lock duration (in seconds with max of 300 seconds): ");
                    string lockDuration = Console.ReadLine();
                    Logger.LogMessage("Please provide max delivery count: ");
                    string maxDeliveryCount = Console.ReadLine();
                    Logger.LogMessage("Do you want to move expired messages to the dead-letter subqueue? (y/n): ");
                    string moveExpiredToDLQ = Console.ReadLine();
                    bool requireMoveExpiredToDLQ = false;
                    if (string.Equals(moveExpiredToDLQ, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        requireMoveExpiredToDLQ = true;
                    }
                    Logger.LogMessage("Do you want to move messages that cause filter evaluation exceptions  to the dead-letter subqueue? (y/n): ");
                    string enableDeadLetteringOnFilterEvaluationExceptions = Console.ReadLine();
                    bool requireEnableDeadLetteringOnFilterEvaluationExceptions = false;
                    if (string.Equals(enableDeadLetteringOnFilterEvaluationExceptions, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        requireEnableDeadLetteringOnFilterEvaluationExceptions = true;
                    }
                    Logger.LogMessage("Do you want to enable sessions? (y/n): ");
                    string requireSessions = Console.ReadLine();
                    bool enableSessions = false;
                    if (string.Equals(requireSessions, "y", StringComparison.OrdinalIgnoreCase))
                    {
                        enableSessions = true;
                    }

                    // Create subscription description with provided details
                    SubscriptionDescription subscriptionDescription = new SubscriptionDescription(topicName, subscriptionName);
                    subscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(defaultMessageTime, CultureInfo.InvariantCulture));
                    subscriptionDescription.LockDuration = new TimeSpan(0, 0, Convert.ToInt32(lockDuration, CultureInfo.InvariantCulture));
                    subscriptionDescription.MaxDeliveryCount = Convert.ToInt32(maxDeliveryCount, CultureInfo.InvariantCulture);
                    subscriptionDescription.EnableDeadLetteringOnMessageExpiration = requireMoveExpiredToDLQ;
                    subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = requireEnableDeadLetteringOnFilterEvaluationExceptions;
                    subscriptionDescription.RequiresSession = enableSessions;

                    // Create subscription
                    if (null == ruleDescription)
                    {
                        nameSpaceManager.CreateSubscription(subscriptionDescription);
                    }
                    else
                    {
                        nameSpaceManager.CreateSubscription(subscriptionDescription, ruleDescription);
                    }
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} created under Topic with name {1} in service bus namespace", subscriptionName, topicName));
                }
                else
                {
                    Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} already exists under Topic with name {1} under in service bus namespace", subscriptionName, topicName));
                }
            }
            catch
            {
                throw;
            }
        }

        public static string CreateTopic()
        {
            string topicName = string.Empty;
            try
            {
                Logger.LogMessage("Creating service bus topic...");
                Logger.LogMessage("Please provide the name for topic: ");
                topicName = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if topic with name {0} already exists in service bus namespace...", topicName));
                if (!nameSpaceManager.TopicExists(topicName))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Topic with name {0} in service bus namespace", topicName));

                    // Accept required details for creating topic
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

                    // Create topic description with provided details
                    TopicDescription topicDescription = new TopicDescription(topicName);
                    topicDescription.MaxSizeInMegabytes = Convert.ToInt32(maxTopicSize, CultureInfo.InvariantCulture);
                    topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(messageTimeToLive, CultureInfo.InvariantCulture));
                    topicDescription.RequiresDuplicateDetection = duplicateDetection;
                    if (topicDescription.RequiresDuplicateDetection)
                    {
                        topicDescription.DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 0, Convert.ToInt32(duplicateDetectionTimeWindowInSeconds, CultureInfo.InvariantCulture));
                    }
                    topicDescription.EnablePartitioning = enablePartitioning;

                    // Create topic
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating topic with name {0} in service bus namespace...", topicName));
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
            return topicName;
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
                            string topicName = CreateTopic();
                            Logger.LogMessage("Do you want to create subscriptions for this topic? (y/n):");
                            string requireTopicSubscriptions = Console.ReadLine();
                            if (string.Equals(requireTopicSubscriptions, "y", StringComparison.OrdinalIgnoreCase))
                            {
                                CreateSubscription(topicName);
                            }
                            break;
                        }
                    case 3:
                        {
                            Logger.LogMessage("Please provide the name for topic under which subscription is to be created: ");
                            string topicName = Console.ReadLine();
                            CreateSubscription(topicName);
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