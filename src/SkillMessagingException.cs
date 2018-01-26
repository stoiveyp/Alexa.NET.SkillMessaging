using System;

namespace Alexa.NET.SkillMessaging
{
    public class SkillMessagingException : Exception
    {
        public SkillMessagingException(int statusCode):base("Error sending message. Received status code " + statusCode)
        {
            
        }

        public SkillMessagingException(string message) : base(message) { }
    }
}