using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ParkingChecker.OutputApi.Helpers
{
    public class SecurityHelper
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetHash(object model, params PropertyInfo[] parametersThatNeedToBeIgnored)
        {
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
                {ContractResolver = new IgnorePropertiesResolver(parametersThatNeedToBeIgnored)});
            var pattern = new Regex(@"[:!@#$%^&*()}{|\:?><\[\]\\;/.,~]");
            var text = pattern.Replace(json, "");
            text = text.Replace(@"\", "");
            text = text.Replace('"'.ToString(), "");
            text = text.Replace(" ", "");
            text = text.ToLower();

            var hash = ComputeSha256Hash(text);
            return hash;
        }
    }
    
    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<PropertyInfo> ignoreProps;
        public IgnorePropertiesResolver(PropertyInfo[] propNamesToIgnore)
        {
           ignoreProps = new HashSet<PropertyInfo>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (ignoreProps.Any(x => x.Name == property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }
    }
}
