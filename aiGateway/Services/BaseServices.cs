using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cms.Shared.Services
{
    public class BaseServices
    {
        public RestClient client;
        public RestRequest endpoint;
        public RestResponse resp;


        public string urlBase = "https://ai-gateway.qa.saiapplications.com/services";

        public void Client(string url)
        {
            var options = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            client = new RestClient(options);
        }

        public RestRequest Endpoint(string rota)
        {
            endpoint = new RestRequest(rota);
            return endpoint;
        }

        public RestResponse ExecutaRequest()
        {
            resp = client.Execute(endpoint);
            return resp;
        }

        public void Post()
        {
            endpoint.Method = Method.Post;
            endpoint.RequestFormat = DataFormat.Json;
        }

        public void Put()
        {
            endpoint.Method = Method.Put;
            endpoint.RequestFormat = DataFormat.Json;
        }

        public void Get()
        {
            endpoint.Method = Method.Get;
        }
        public void Delete()
        {
            endpoint.Method = Method.Delete;
        }

        public void ReturnResponse(string msgApi)
        {
            JObject resposta = JObject.Parse(resp.Content);
        }

        public void Header(string chave, string value)
        {
            endpoint.AddHeader(chave, value);
        }

    }
}
