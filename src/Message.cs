using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alexa.NET.SkillMessaging
{
    public class Message
    {
        [JsonProperty("data")]
        public IDictionary<string,string> Data { get; set; }

        [JsonProperty("expiresAfterSeconds")]
        public int ExpiresAfter { get; set; }

        public Message():this(new Dictionary<string, string>(), 0)
        {

        }

        public Message(IDictionary<string, string> data, int expiresAfter)
        {
            Data = data;
            ExpiresAfter = expiresAfter;
        }
    }
}
