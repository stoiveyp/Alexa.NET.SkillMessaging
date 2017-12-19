using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alexa.NET.SkillMessaging
{
    public class MessageReceivedRequest:Request.Type.Request
    {
        [JsonProperty("message")]
        public Dictionary<string,string> Message { get; set; }
    }
}
