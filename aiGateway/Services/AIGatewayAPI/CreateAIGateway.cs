using aiGateway.Model;
using Cms.Shared.Extensions;
using Cms.Shared.Services;
using Newtonsoft.Json;
using RestSharp;
using TechTalk.SpecFlow.Infrastructure;

namespace aiGateway.Services.AIGatewayAPI
{
    public class CreateAIGateway : BaseServices
    {

        public string endpointCreateAIGateway;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        public CreateAIGateway(ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public void BodyCreate(CreateAIGatewayRequest createAIGateway)
        {
            endpoint.RequestFormat = DataFormat.Json;
            endpoint.AddJsonBody(createAIGateway);
        }

        public (string, CoreExceptionDto) CreateAIGatewayPost(string modelo, string servicesId, string apiKey)
        {
            endpointCreateAIGateway = $"/{servicesId}/openai/deployments/{modelo}/chat/completions?api-version=2023-07-01-preview";
            Client(urlBase);
            Endpoint(endpointCreateAIGateway);
            Header("Content-Type", "application/json");
            Header("api-key", apiKey);
            Post();

            var newMessage = new CreateAIGatewayRequest
            {
                model = modelo,
                messages = new Messages[] {
                    new Messages {
                        role = "user",
                        content = "Baseado no seguinte texto:\nO USS Iowa foi um encouraçado pré-dreadnought construído para a Marinha dos Estados Unidos em meados da década de 1890. O navio representou uma melhoria em relação à classe Indiana, corrigindo muitos dos defeitos de desenho dessas embarcações. Entre as melhorias mais importantes estavam a navegabilidade, devido à sua borda livre mais elevada e um arranjo mais eficiente do armamento. O Iowa foi projetado para operar em alto mar, o que foi o ímpeto para aumentar a borda livre. Ele estava armado com uma bateria de quatro canhões de 305 milímetros em duas torres duplas, apoiadas por uma bateria secundária de oito canhões de 203 milímetros. Ao entrar em serviço em junho de 1897, o Iowa conduziu operações de treinamento no Oceano Atlântico antes de ter sua base de operações alterada para o Caribe, no início de 1898, quando as tensões entre os Estados Unidos e a Espanha sobre Cuba aumentaram, levando à Guerra Hispano-Americana. O navio participou do bombardeio de San Juan, Porto Rico, e posteriormente do bloqueio de Cuba durante a guerra.\nResuma o texto em poucas palavras."
                    }
                },
                temperature = 0.7F
            };

            _specFlowOutputHelper.WriteLine($"New message: {newMessage}.");

            BodyCreate(newMessage);
            ExecutaRequest();

            if (resp.IsSuccessful)
            {
                if (resp.ContentType == "application/json")
                {

                    _specFlowOutputHelper.WriteLine($"Return AI Gateway Response: {resp.Content}.");
                    _specFlowOutputHelper.WriteLine($"Return Status Code AI Gateway Response: {resp.StatusCode}.");
                    return (resp.Content, null);
                }
                else
                {
                    var correlationId = JsonConvert.DeserializeObject<string>(resp.Content);
                    return (correlationId, null);
                }
            }
            else
            {
                _specFlowOutputHelper.WriteLine($"Error: {resp.Content ?? resp.ErrorMessage}.");

                CoreExceptionDto response;
                if (resp.StatusCode == 0)
                {
                    response = new CoreExceptionDto
                    {
                        //status = resp.StatusCode,
                        detail = resp.ErrorMessage
                    };
                }
                else
                {
                    response = resp.ConvertError();
                }

                return (null, response);
            }
        }
    }
}
