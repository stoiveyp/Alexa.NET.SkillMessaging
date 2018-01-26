using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.SkillMessaging;
using Newtonsoft.Json.Linq;

namespace Alexa.NET
{
    public class SkillMessageClient
    {
        public const string NorthAmericaEndpoint = "https://api.amazonalexa.com/v1/skillmessages/users/";
        public const string EuropeEndpoint = "https://api.eu.amazonalexa.com/v1/skillmessages/users/";
        public const string AmazonRequestId = "X-Amzn-RequestID";
        public HttpClient Client { get; }

        public SkillMessageClient(string endpointUrl, string accessToken)
        {
            var client = new HttpClient { BaseAddress = new Uri(endpointUrl, UriKind.Absolute) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            Client = client;
        }

        public SkillMessageClient(HttpClient client)
        {
            Client = client;
        }

        public async Task<string> Send(Message message, string userId)
        {
            if (message.ExpiresAfter <= 0)
            {
                throw new SkillMessagingException("Message ExpiresAfter property must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new SkillMessagingException("User ID has not been set");
            }

            var response = await Client.PostAsync(userId, new StringContent(JObject.FromObject(message).ToString(), Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return response.Headers.GetValues(AmazonRequestId).FirstOrDefault();
            }
            else
            {
                throw new SkillMessagingException((int)response.StatusCode);
            }
        }
    }
}
