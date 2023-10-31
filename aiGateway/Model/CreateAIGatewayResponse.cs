using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aiGateway.Model
{
    public class CreateAIGatewayResponse
    {

        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public Usage usage { get; set; }
        public AIGatewayDto[] data { get; set; }

    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class AIGatewayDto
    {
        [JsonProperty("model")]
        public string model { get; set; }
        [JsonProperty("prompt_tokens")]
        public int prompt_tokens { get; set; }
        [JsonProperty("completion_tokens")]
        public int completion_tokens { get; set; }
        [JsonProperty("total_tokens")]
        public int total_tokens { get; set; }
        [JsonProperty("servicesId")]
        public string servicesId { get; set; }
        [JsonProperty("apiKey")]
        public string apiKey { get; set; }
    }

    public class GetListError
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public string instance { get; set; }
    }
}
