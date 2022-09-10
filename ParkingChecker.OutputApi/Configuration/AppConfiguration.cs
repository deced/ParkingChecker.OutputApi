using System;

namespace ParkingChecker.OutputApi.Configuration
{
    public class AppConfiguration
    {
        public static string ApiAccessToken
        {
            get { return Environment.GetEnvironmentVariable("API_ACCESS_TOKEN") ?? "bxw470f2-679e-4b9b-ac91-f34ddfe23ca3"; }
        }
        public static string ApplicationDomain
        {
            get { return Environment.GetEnvironmentVariable("APPLICATION_DOMAIN") ?? "localhost"; }
        }
    }
}
