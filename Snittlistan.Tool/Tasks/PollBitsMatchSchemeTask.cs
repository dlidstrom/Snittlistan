namespace Snittlistan.Tool.Tasks
{
    using System;
    using System.Net.Http;
    using Queue;
    using Queue.Queries;

    public class PollBitsMatchSchemeTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            foreach (var uri in CommandLineTaskHelper.AllApiUrls())
            {
                var client = new HttpClient(new LoggingHandler(new HttpClientHandler()))
                {
                    Timeout = TimeSpan.FromSeconds(600)
                };
                var responseMessage = client.PostAsJsonAsync(uri, new GetTeamNamesQuery()).Result;
                responseMessage.EnsureSuccessStatusCode();
                var result = responseMessage.Content.ReadAsAsync<GetTeamNamesQuery.Result>().Result;
                foreach (var teamNameAndLevel in result.TeamNameAndLevels)
                {
                    // invoke the page automation script
                }

                // for all generated files, push to queue (two messages per team: match scheme and standings)
            }
        }

        public string HelpText => "Fetches the latest match scheme and standings for all teams from BITS.";
    }
}