using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Request.Type;

namespace Alexa.NET.SkillMessaging
{
    public class MessageReceivedRequestTypeConverter:IRequestTypeConverter
    {
        public bool CanConvert(string requestType)
        {
            return requestType == "Messaging.MessageReceived";
        }

        public Request.Type.Request Convert(string requestType)
        {
            return new MessageReceivedRequest();
        }
    }
}
