namespace TfsWebApi.Services.Entities
{
    public class Story
    {
        public long id { get; internal set; }
        public dynamic title { get; internal set; }
        public dynamic status { get; internal set; }
    }
}