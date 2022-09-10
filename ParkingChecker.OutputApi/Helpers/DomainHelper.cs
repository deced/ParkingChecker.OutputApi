using System;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace ParkingChecker.OutputApi.Helpers
{
    public static class DomainHelper
    {
        public static bool ValidateDomain(string value)
        {
            var pingSender = new Ping();
            try
            {
                var reply = pingSender.Send(value);
                if(reply != null){
                    return true;
                }
                return false;
            }
            catch{
                return false;
            }
        }
        public static string GetDomain(string value)
        {
            if (!value.Contains("://")){
                value = "http://" + value;
            }
               
            try
            {
                var uri = new Uri(value);
                if (!uri.HostNameType.Equals(UriHostNameType.Dns) || uri.IsLoopback)
                    return string.Empty; // or throw an 
                value = uri.Host.ToString();
                value = value.Replace(" ","");
                value = value.Replace("/","");
                if(value.Contains("www.")){
                    value = value.Replace("www.","");
                }
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetDomainFromEmail(string value)
        {   
            try{
                var address = new MailAddress(value);
                string host = address.Host;
                return host;
            }
            catch{
                return string.Empty;
            }
            
        }
    }
}