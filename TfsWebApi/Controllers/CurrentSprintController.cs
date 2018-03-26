using System.Web.Http;
using TfsWebApi.Services;
using TfsWebApi.Services.Entities;
using TfsWebApi.Gateways;

namespace TfsWebApi.Controllers
{
    public class CurrentSprintController : ApiController
    {
        private SprintService _sprintService = new SprintService(
            new TeamFoundationServerSprintGateway(
                "CoupApi",
                new BasicAuthentication { username = "", password = "" }
                )
            );

        [HttpGet]
        [Route("sprints/{sprint_id}/stories")]
        public IHttpActionResult GetStories(string sprint_id)
        {
            var stories = _sprintService.GetStories(sprint_id);

            return Ok(stories);
        }
    }

    
}
