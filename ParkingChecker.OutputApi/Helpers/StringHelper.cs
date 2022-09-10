using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParkingChecker.OutputApi.Helpers
{
    public static class StringHelper
    {
        public static string GetHash(string value)
        {
            if(string.IsNullOrEmpty(value)){
                return string.Empty;
            }

            var prohibitedCharachers = new List<string>()
                {"#","_", ".", "/", @"\", "&", "!", "@","$", "%", "^", "*","(",")", "=", "+", "|", "?", ",", ";", ":", "[","]","{","}"};
           
            foreach(var item in prohibitedCharachers)
            {
                value = value.Replace(item, "");
            }

            var pattern = new Regex(@"[:!@#$%^&*()}{|\:?><\[\]\\;/.,~]");
            value = pattern.Replace(value, "");
            value = value.ToLower();
            value = value.Replace(" ","");
            value = value.Replace("-","");
            var hash = SecurityHelper.ComputeSha256Hash(value);
           
            return hash;
        }

       
    }
}