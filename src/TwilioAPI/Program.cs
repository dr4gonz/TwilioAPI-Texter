using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwilioAPI
{
        public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            //1
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //2
            var request = new RestRequest("Accounts/ACb4144136d7e68bf288ff485807b583b1/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("ACb4144136d7e68bf288ff485807b583b1", "d3ccfd4371c4d3d7ef5b7b4986c87291");
            var response = new RestResponse();
            //3
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;

            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (var message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();

            //4
            Console.WriteLine(jsonResponse["messages"]);
           
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
