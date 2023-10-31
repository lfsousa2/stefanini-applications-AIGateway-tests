using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aiGateway.Model
{
    public class CoreExceptionDto
    {

        public CoreExceptionDto()
        {

        }

        public CoreExceptionDto(int status, string detail)
        {
            Errors?.Add(new Error(status, detail));
        }

        public int status { get; set; }
        public string detail { get; set; }

        public string? GetFirstErrorMessage()
        {
            return Errors?.FirstOrDefault()?.Detail;
        }

        public string GetErros()
        {
            if (Errors == null)
            {
                return $"{status} - {detail}";
            }
            var sb = new StringBuilder();
            foreach (var error in Errors)
            {
                sb.AppendLine($"{error.Status} - {error.Detail}");
            }
            return sb.ToString();
        }

        [JsonProperty("errors")]
        public ICollection<Error> Errors { get; set; } = new List<Error>();


        public class Error
        {
            [JsonProperty("status")]
            public int Status { get; set; }
            [JsonProperty("detail")]
            public string Detail { get; set; }

            public Error(int status, string detail)
            {
                Status = status;
                Detail = detail;
            }
        }

    }
}

