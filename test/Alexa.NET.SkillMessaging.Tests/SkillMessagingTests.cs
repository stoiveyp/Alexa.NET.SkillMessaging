using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.SkillMessaging;
using Xunit;

namespace Alexa.NET.SkillManagement.Tests
{
    public class SkillMessagingTests
    {
        private readonly IDictionary<string, string> TestMessageData = new Dictionary<string, string>
        {
            {"notificationTitle", "New Message from Backend"},
            {"spokenOutput", "Hi, This is the message sent from Backend."}
        };

        private const string AuthToken = "authToken";
        private const string UserId = "fakeUserId";

        public SkillMessagingTests()
        {
            RequestConverter.RequestConverters.Add(new MessageReceivedRequestTypeConverter());
        }

        [Fact]
        public void MessageGeneratesCorrectJson()
        {
            var message = new Message(TestMessageData,60);
            Assert.True(Utility.CompareJson(message, "Message.json"));
        }

        [Fact]
        public async Task SendThrowsExceptionIfExpiryIsZero()
        {
            var message = new Message(TestMessageData, 0);
            var messageRequest = new SkillMessageClient(new HttpClient());
            await Assert.ThrowsAsync<SkillMessagingException>(() => messageRequest.Send(message,UserId));
        }

        [Fact]
        public async Task SendThrowsExceptionIfUserIdIsEmpty()
        {
            var message = new Message(TestMessageData, 30);
            var messageRequest = new SkillMessageClient(new HttpClient());
            await Assert.ThrowsAsync<SkillMessagingException>(() => messageRequest.Send(message, string.Empty));
        }

        [Fact]
        public void GeneratesCorrectClientWithConstructorData()
        {
            var messageRequest = new SkillMessageClient(SkillMessageClient.EuropeEndpoint,AuthToken);
            Assert.Equal(messageRequest.Client.DefaultRequestHeaders.Authorization.Parameter,AuthToken);
            Assert.Equal(messageRequest.Client.BaseAddress.Host,new Uri(SkillMessageClient.EuropeEndpoint).Host);
        }

        [Fact]
        public async Task SendsCorrectRequest()
        {
            var handler = new ActionMessageHandler(req =>
            {
                Assert.Equal(HttpMethod.Post,req.Method);
                Assert.Equal(new Uri(new Uri(SkillMessageClient.EuropeEndpoint), UserId).ToString(),req.RequestUri.ToString());
                var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                response.Headers.Add("X-Amzn-RequestID","xxx");
                return response;
            });

            var client = new HttpClient(handler) {BaseAddress = new Uri(SkillMessageClient.EuropeEndpoint, UriKind.Absolute)};
            var messageRequest = new SkillMessageClient(client);

            var message = new Message(TestMessageData, 60);
            await messageRequest.Send(message, UserId);
        }

        [Fact]
        public async Task HandlesResponseHeaderCorrectly()
        {
            var handler = new ActionMessageHandler(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                response.Headers.Add("X-Amzn-RequestID", "xxx");
                return response;
            });

            var client = new HttpClient(handler) { BaseAddress = new Uri(SkillMessageClient.EuropeEndpoint, UriKind.Absolute) };
            var messageRequest = new SkillMessageClient(client);

            var message = new Message(TestMessageData, 60);
            Assert.Equal("xxx", await messageRequest.Send(message, UserId));
        }

        [Fact]
        public void MessageReceivedRequestDeserializesProperly()
        {
            string noticeValue = "<< text of message >>";
            var request = Utility.ExampleFileContent<SkillRequest>("SampleMessage.json");
            Assert.IsType<MessageReceivedRequest>(request.Request);

            var messageReceived = (MessageReceivedRequest)request.Request;
            Assert.Equal(noticeValue,messageReceived.Message["notice"]);
        }
    }
}
