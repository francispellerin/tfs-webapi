namespace TfsWebApi.Services.Entities
{
    public class BasicAuthentication : Authentication
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}