using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Remoting.Channels;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace myFirstForm
{
    internal class KeyMessagerClient
    {
        //Constant interval definitions
        public const int twoHoursInterval = 7200;
        public const int sixHoursInterval = 21600;
        //Constant channel numbers that has 6 hour interval
        public readonly List<int> sixHourChannels = new List<int>(){ 3, 4, 5, 11, 83, 84, 85, 86, 87, 88, 93 };
        //Base variables which will be retrieved from the constructor.
        public List<string> fileContents = new List<string>();
        public List<int> channelNums = new List<int>();
        public string authorizationCode;

        //Final variables which will be returned from functions
        public List<HttpContent> generatedTwoHourContents = new List<HttpContent>();
        public List<HttpContent> generatedSixHourContents = new List<HttpContent>();
        public List<string> generatedTwoHourChannelLinks = new List<string>();
        public List<string> generatedSixHourChannelLinks = new List<string>();
        public List<int> timeIntervals = new List<int>();

        //Task counts will be used to keep track of how many messages sent total.
        public static int twoHourTaskCounts = 0; //0 by default.
        public static int sixHourTaskCounts = 0; //0 by default.

        //Let's define the constructor and assign each parameter to base properties we defined above.
        public KeyMessagerClient(List<string> fileContents, List<int> channelNums, List<int> timeIntervals, string authorizationCode)
        {
            this.fileContents = fileContents;
            this.channelNums = channelNums;
            this.timeIntervals = timeIntervals;
            this.authorizationCode = authorizationCode;
        }
        //Function that will initiate the main process.
        public async Task InitiateProcess()
        {
            var httpClient = new HttpClient(); //Create an instance of HttpClient.
            try
            {
                httpClient.DefaultRequestHeaders.Add("authorization", authorizationCode); //Authorize the request.
            }
            catch (Exception e)
            {
                MessageBox.Show($"Authorization code couldn't be processed. Check if your code is in correct format and try again! Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            httpClient.BaseAddress = new Uri("https://discord.com/api/v9/channels/"); //Define the base URI.

            //Let's iterate through each fileContent and channelNum and generate HttpContents with channel links.
            for (int i = 0, j = 0; i < fileContents.Count && j < channelNums.Count; i++, j++)
            {
                var buffer = new
                {
                    content = fileContents[i]
                };
                var json = JsonConvert.SerializeObject(buffer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string channel = FindChannelLinkByChannelNum(channelNums[j]);
                if (sixHourChannels.Contains(channelNums[j]))
                {
                    generatedSixHourContents.Add(content);
                    generatedSixHourChannelLinks.Add(channel);
                }
                generatedTwoHourContents.Add(content); //Define a buffer with each content, serialize it as JSON, create a HttpContent out of it and store in generatedContents list.
                generatedTwoHourChannelLinks.Add(channel); //Take the returned discord url from FindChannelLinkByChannelNum() function and store it in generatedChannelLinks list.

                json.Remove(0, json.Length);
                channel.Remove(0, channel.Length);
            }
            //Let's send messages to all channels one time
            for (int z = 0; z < generatedTwoHourContents.Count; z++)
            {
                try
                {
                    await sendMessage(httpClient, generatedTwoHourChannelLinks, generatedTwoHourContents, z);
                }
                catch(Exception e)
                {
                    MessageBox.Show($"I couldn't send your message to the {channelNums[z]}th channel! Are you sure you aren't banned or you don't have cooldown? Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            for(int f = 0; f < generatedSixHourContents.Count; f++)
            {
                try
                {
                    await sendMessage(httpClient, generatedSixHourChannelLinks, generatedSixHourContents, f);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"I couldn't send your message to the {channelNums[f]}th channel! Are you sure you aren't banned or you don't have cooldown? Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Put the rest in an infinite loop
            //First, let's split different time intervals in different lists.
            //This logic is not being used in the current version of software. You can quote this foreach loop with List initializations. No functionality will be affected.
            List<int> twoHourIntervals = new List<int>();
            List<int> sixHourIntervals = new List<int>();
            foreach(int interval in timeIntervals)
            {
                if(interval == twoHoursInterval)
                {
                    twoHourIntervals.Add(interval);
                }
                else if(interval == sixHoursInterval)
                {
                    sixHourIntervals.Add(interval);
                }
                else
                {
                    MessageBox.Show($"The interval {interval} you entered is invalid. Try to do better math, man. Remember, 1 hour = 3600 secs : ]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //throw an exception if invalid time interval found.
                }
            }

            //And off we go!
            while (true)
            {
                await sendMessageEveryTwoHours(httpClient, generatedTwoHourChannelLinks, generatedTwoHourContents);
                await sendMessageEverySixHours(httpClient, generatedSixHourChannelLinks, generatedSixHourContents);
            }
        }
        private static async Task sendMessage(HttpClient httpClient, List<string> channelLink, List<HttpContent> content, int msgOrder)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.PostAsync($"{httpClient.BaseAddress}{channelLink[msgOrder]}", content[msgOrder]);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                if (response != null)
                {
                    MessageBox.Show($"There was something wrong with HTTP request. Server returned error code: {response.StatusCode}. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"There was something wrong with HTTP request. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private static async Task sendMessageEveryTwoHours(HttpClient httpClient,List<string> twoHourChannelLinks,List<HttpContent> twoHourContents)
        {
            await Task.Delay(twoHoursInterval * 1000);
            HttpResponseMessage response = null;
            try
            {
                for(int msgOrder = 0; msgOrder < twoHourContents.Count; msgOrder++)
                {
                    response = await httpClient.PostAsync($"{httpClient.BaseAddress}{twoHourChannelLinks[msgOrder]}", twoHourContents[msgOrder]);
                    response.EnsureSuccessStatusCode();
                }
                twoHourTaskCounts++;
            }
            catch (Exception e)
            {
                if (response != null)
                {
                    MessageBox.Show($"There was something wrong with HTTP request. Server returned error code: {response.StatusCode}. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"There was something wrong with HTTP request. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private static async Task sendMessageEverySixHours(HttpClient httpClient, List<string> sixHourChannelLinks, List<HttpContent> sixHourContents)
        {
            await Task.Delay(sixHoursInterval * 1000);
            HttpResponseMessage response = null;
            try
            {
                for (int msgOrder = 0; msgOrder < sixHourContents.Count; msgOrder++)
                {
                    response = await httpClient.PostAsync($"{httpClient.BaseAddress}{sixHourChannelLinks[msgOrder]}", sixHourContents[msgOrder]);
                    response.EnsureSuccessStatusCode();
                }
                sixHourTaskCounts++;
            }
            catch (Exception e)
            {
                if (response != null)
                {
                    MessageBox.Show($"There was something wrong with HTTP request. Server returned error code: {response.StatusCode}. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"There was something wrong with HTTP request. This usually happens when you have cooldown on the specific channel you are trying to send message to. Wait your cooldown to finish if that's the case and try again. Error message: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private static string FindChannelLinkByChannelNum(int channelNum)
        {
            try
            {
                string json = System.IO.File.ReadAllText("links.json");
                JObject jsonObject = JObject.Parse(json);
                string link = (string)jsonObject[channelNum.ToString()];
                return link;
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was an error in reading 'links.json' file. Make sure you have 'links.json' file in the same directory as this file and try again! Error message: {e.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
