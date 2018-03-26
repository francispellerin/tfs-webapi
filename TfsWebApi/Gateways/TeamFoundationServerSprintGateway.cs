using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TfsWebApi.Services.Entities;
using TfsWebApi.Restsharp;

namespace TfsWebApi.Gateways
{
    public class TeamFoundationServerSprintGateway : ISprintGateway
    {
        private string _baseUrl;
        private Authentication _authentication;
        private string _teamProjectName;

        public TeamFoundationServerSprintGateway(string teamProjectName, Authentication authentication)
        {
            _baseUrl = "https://fpellerin.visualstudio.com";
            _authentication = authentication;
            _teamProjectName = teamProjectName;
        }

        public IEnumerable<Story> GetStoriesFromSprint(string sprintId)
        {
            var workItemsIds = GetWorkItemsIds(sprintId);

            return GetStories(workItemsIds);
        }

        private IEnumerable<long> GetWorkItemsIds(string sprintId)
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = _authentication.ToRestsharpAuthenticator();

            var request = new RestRequest($"DefaultCollection/{_teamProjectName}/_apis/wit/wiql?api-version=1.0");

            string body = string.Format(@"
{{
  ""query"": ""select[System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.IterationPath] from WorkItems where[System.TeamProject] = '{0}' and [System.IterationPath] Under '{0}\\{1}' order by [System.Id]""
}}
", _teamProjectName, sprintId);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Post<QueryResult>(request);

            return response.Data.workItems.ToList().Select(w => w.id);
        }

        private IEnumerable<Story> GetStories(IEnumerable<long> ids)
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = _authentication.ToRestsharpAuthenticator();

            var request = new RestRequest($"DefaultCollection/{_teamProjectName}/_apis/wit/WorkItems?ids={string.Join(",", ids)}");

            var response = client.Get<dynamic>(request);

            var stories = MapToStories(response.Data["value"]);

            return stories;
        }

        private IEnumerable<Story> MapToStories(dynamic content)
        {
            foreach (var item in content)
            {
                yield return new Story
                {
                    id = item["id"],
                    title = GetField(item, "System.Title"),
                    status = GetField(item, "System.State")
                };
            }
        }

        private string GetField(dynamic item, string fieldName)
        {
            var itemsDictionnary = ((Dictionary<string, object>)item);

            var fields = (Dictionary<string, object>)itemsDictionnary["fields"];

            return fields[fieldName].ToString();
        }
    }

    public class QueryResult
    {
        public List<WorkItem> workItems { get; set; }
    }

    public class WorkItem
    {
        public long id { get; set; }
    }
}