using System.Collections.Generic;
using TfsWebApi.Services.Entities;

namespace TfsWebApi.Gateways
{
    public interface ISprintGateway
    {
        IEnumerable<Story> GetStoriesFromSprint(string sprintId);
    }
}