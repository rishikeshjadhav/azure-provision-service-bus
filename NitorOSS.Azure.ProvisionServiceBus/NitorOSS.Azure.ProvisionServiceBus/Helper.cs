
namespace NitorOSS.Azure.ProvisionServiceBus
{
    using NitorOSS.Azure.Logger;
    using System;

    public static class Helper
    {
        public static bool GetBooleanResponse(string message)
        {
            bool result = false;
            Logger.LogMessage(message);
            string userResponse = Console.ReadLine();
            if (string.Equals(userResponse, "y", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            return result;
        }
    }
}
