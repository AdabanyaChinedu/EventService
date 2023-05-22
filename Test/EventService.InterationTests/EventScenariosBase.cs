
namespace EventService.IntegrationTests
{
    public class EventScenariosBase
    {
        private const string ApiUrlBase = "api/v1/events";

        public static class Get
        {
            public static string GetEventByIdUrl(Guid id)
            {
                return $"{ApiUrlBase}/{id}";
            }

            public static string GetPagedEventsUrl(int pageIndex, int pageSize)
            {
               return $"{ApiUrlBase}?pageIndex={pageIndex}&pageSize={pageSize}";
            } 
            
        }

        public static class Post
        {
            public static string CreateEventUrl = $"{ApiUrlBase}/";
            
        }

        public static class Put
        {
            public static string UpdateEventUrl(Guid id)
            {
                return $"{ApiUrlBase}/{id}";
            }
        }

        public static class Delete
        {
            public static string RemoveEventUrl(Guid id)
            {
                return $"{ApiUrlBase}/{id}";
            }
        }
    }
}
