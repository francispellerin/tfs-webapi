using System.Collections.Generic;
using TfsWebApi.Gateways;
using TfsWebApi.Services.Entities;

namespace TfsWebApi.Services
{
    public class SprintService
    {
        private readonly ISprintGateway _sprintGateway;

        public SprintService(ISprintGateway sprintGateway)
        {
            _sprintGateway = sprintGateway;
        }
        public IEnumerable<Story> GetStories(string sprintId)
        {
            var stories = _sprintGateway.GetStoriesFromSprint(sprintId);

            return stories;
        }
    }
}