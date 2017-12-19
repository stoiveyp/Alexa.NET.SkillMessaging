using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.SkillMessaging;
using Newtonsoft.Json;

namespace Alexa.NET
{
    public class AccessTokenClient
    {
        public const string ApiDomainBaseAddress = "https://api.amazon.com";
        public const string MessagingScope = "alexa:skill_messaging";
        public const string ClientCredentials = "client_credentials";

        public HttpClient Client { get; set; }
        private static readonly JsonSerializer Serializer = JsonSerializer.Create();

        public AccessTokenClient(string baseAddress):this(new HttpClient { BaseAddress = new Uri(baseAddress,UriKind.Absolute)})
        {

        }

        public AccessTokenClient(HttpClient client)
        {
            Client = client;
        }

        public async Task<AccessToken> Send(string clientId, string clientSecret)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string,string>
            {
                {"client_id",clientId},
                {"client_secret",clientSecret},
                {"grant_type",ClientCredentials},
                {"scope",MessagingScope}
            });

            var response = await Client.PostAsync("/auth/O2/token", content);
            using (var reader = new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync())))
            {
                return Serializer.Deserialize<AccessToken>(reader);
            }
        }
    }
}
