# Alexa.NET.SkillMessaging
A simple .NET Core library for handling skill mesaging to Alexa skills
&nbsp;
## Gain access to Skill Messaging OAuth Token
```csharp
var client = new AccessTokenClient(AccessTokenClient.ApiDomainBaseAddress);
var accessToken = await client.Send(clientId,clientSecret);
var oauthToken = accessToken.Token;
```
&nbsp;
## Send Message to Skill
```csharp
var payload = new Dictionary<string, string>{ {"testKey", "testValue"} };

var messages = new Alexa.NET.SkillMessageClient(Alexa.NET.SkillMessageClient.EuropeEndpoint, oauthToken);
var messageToSend = new Alexa.NET.SkillMessaging.Message(payload,300);

var messageId = await messages.Send(messageToSend, userId);
```
&nbsp;
## Add support for Skill Messaging requests
```csharp
 RequestConverter.RequestConverters.Add(new MessageReceivedRequestTypeConverter());
```