using FortniteBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace FortniteBot.Dialogs
{
    public enum Platform
    {
        pc,
        xbl,
        psn,
    }

    [Serializable]
    public class ForniteData
    {
        [Prompt("What Username would you like to search?")]
        public string UserName;
        [Prompt("What platform does this user play on? {||}")]
        public Platform? Platform;

        public static IForm<ForniteData> BuildForm()
        {
            OnCompletionAsyncDelegate<ForniteData> DisplayData = async (context, state) =>
            {
                //method to display data back from api
                var replyMessage = context.MakeMessage();

                var data = GetDataAsync(state.UserName, state.Platform.ToString()).Result;
                int noOfKills = 0;
                for (int i = 0; i < data.recentMatches.Count; i++)
                {
                    noOfKills += data.recentMatches[i].kills;
                }

                replyMessage.Text = $"Username: {data.epicUserHandle}, Platform: {data.platformNameLong}, # of kills in last {data.recentMatches.Count} matches: {noOfKills}";

                await context.PostAsync(replyMessage);
            };

            return new FormBuilder<ForniteData>()
                .Message("I can help with finding data from Fortnite.")
                .OnCompletion(DisplayData)
                .Build();
        }

        static HttpClient client = new HttpClient();

        static async Task<RootObject> GetDataAsync(string userName, string platform)
        {
            string stringUri = "https://api.fortnitetracker.com/v1/profile/" + platform + "/" + userName + "/";
            //string escapedUri = Uri.EscapeDataString(uri);
            Uri builtUri = new Uri(stringUri);
            string absoluteUri = builtUri.AbsoluteUri;

            RootObject json = null;
            var requestData = new HttpRequestMessage(HttpMethod.Get, absoluteUri);
            //var requestData = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.fortnitetracker.com/v1/profile/{0}/{1}", platform, userName));
            requestData.Headers.Add("TRN-Api-Key", "ea772b88-a8be-408c-bb13-4402eb9048f1");

            //HttpResponseMessage response = await client.SendAsync(requestData);
            HttpResponseMessage response = client.SendAsync(requestData).Result;

            if (response.IsSuccessStatusCode)
            {
                var serializer = new JavaScriptSerializer();
                json = serializer.Deserialize<RootObject>(await response.Content.ReadAsStringAsync());
            }
            return json;
        }

    }
}