
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventService.SharedLibrary.Model.ResponseModel
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class Result<T> : ProblemDetails
    {

        public Result()
        {
            Status = StatusCodes.Status200OK;
        }
        
        [JsonProperty("response")]
        public T Response { get; set; }

        [JsonProperty("errorFlag")]
        public bool ErrorFlag { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
