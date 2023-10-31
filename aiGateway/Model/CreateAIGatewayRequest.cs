using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aiGateway.Model
{
    public record CreateAIGatewayRequest
    {
        public string model { get; set; }
        public Messages[] messages { get; set; }
        public float temperature { get; set; }

    }

    public record Messages
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public record CreateAIGatewayApiResult : CreateAIGatewayRequest
    {
        public string total_tokens { get; set; }
    }
}
