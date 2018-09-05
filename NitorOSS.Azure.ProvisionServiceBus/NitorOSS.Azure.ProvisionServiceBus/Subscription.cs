
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NitorOSS.Azure.Logger;
    using System;
    using System.Globalization;

    public class Subscription
    {
        private static NamespaceManager nameSpaceManager;

        public string Name;

        public Subscription(string serviceBusConnectionString)
        {
            nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
        }

        public bool CheckIfExists(string topicName, string subscriptionName)
        {
            bool result = false;
            if (nameSpaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                result = true;
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} already exists under Topic with name {1} under in service bus namespace", subscriptionName, topicName));
            }
            return result;
        }

        private SubscriptionDescription GenerateSubscriptionDescription(string topicName, string subscriptionName)
        {
            // Accept required details for creating topic
            // Default message time to live
            Logger.LogMessage("Please provide the default message time to live (in seconds): ");
            string defaultMessageTime = Console.ReadLine();
            // Default lock duration
            Logger.LogMessage("Please provide the lock duration (in seconds with max of 300 seconds): ");
            string lockDuration = Console.ReadLine();
            // Maximum delivery count
            Logger.LogMessage("Please provide max delivery count: ");
            string maxDeliveryCount = Console.ReadLine();
            // Enable moving expired messages to dead-letter queue
            bool moveExpiredToDLQ = Helper.GetBooleanResponse("Do you want to move expired messages to the dead-letter subqueue? (y/n): ");
            // Enable dead lettering on filter evaluation exceptions
            bool enableDeadLetteringOnFilterEvaluationExceptions = Helper.GetBooleanResponse("Do you want to move messages that cause filter evaluation exceptions  to the dead-letter subqueue? (y/n): ");
            // Enable sessions
            bool enableSessions = Helper.GetBooleanResponse("Do you want to enable sessions? (y/n): ");
            // Create subscription description with provided details
            SubscriptionDescription subscriptionDescription = new SubscriptionDescription(topicName, subscriptionName);
            subscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(defaultMessageTime, CultureInfo.InvariantCulture));
            subscriptionDescription.LockDuration = new TimeSpan(0, 0, Convert.ToInt32(lockDuration, CultureInfo.InvariantCulture));
            subscriptionDescription.MaxDeliveryCount = Convert.ToInt32(maxDeliveryCount, CultureInfo.InvariantCulture);
            subscriptionDescription.EnableDeadLetteringOnMessageExpiration = moveExpiredToDLQ;
            subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = enableDeadLetteringOnFilterEvaluationExceptions;
            subscriptionDescription.RequiresSession = enableSessions;
            return subscriptionDescription;
        }

        public bool Create(string topicName)
        {
            bool result = true;
            try
            {
                Logger.LogMessage("Creating Service Bus Topic Subscription...");
                Logger.LogMessage("Please provide the name for Subscription: ");
                Name = Console.ReadLine();
                Logger.LogMessage(string.Format("Checking if Subscription with name {0} already exists under Topic with name {0}...", Name, topicName));
                if (!CheckIfExists(topicName, Name))
                {
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Subscription with name {0} under Topic {1} in service bus namespace", Name, topicName));
                    RuleDescription ruleDescription = null;
                    //if (null != department)
                    //{
                    //    ruleDescription = new RuleDescription()
                    //    {
                    //        Name = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_Rule", topicName, subscriptionName),
                    //        Filter = new SqlFilter(string.Format(CultureInfo.InvariantCulture, "Department = '{0}'", department))
                    //    };
                    //}

                    // Generate subscription description
                    SubscriptionDescription subscriptionDescription = GenerateSubscriptionDescription(topicName, Name);
                    // Create subscription
                    if (null == ruleDescription)
                    {
                        nameSpaceManager.CreateSubscription(subscriptionDescription);
                    }
                    else
                    {
                        nameSpaceManager.CreateSubscription(subscriptionDescription, ruleDescription);
                    }
                    Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} created under Topic with name {1} in service bus namespace", Name, topicName));
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
