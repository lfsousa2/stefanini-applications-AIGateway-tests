using aiGateway.Model;
using aiGateway.Services.AIGatewayAPI;
using Allure.Commons;
using Cms.Shared.Model;
using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.UnitTestProvider;
using static aiGateway.Model.CreateAIGatewayResponse;

namespace aiGateway.StepDefinitions
{
    [Binding]
    public class AIGatewayStepDefinitions
    {

        private CreateAIGateway createAIGateway;
        private ISpecFlowOutputHelper _specFlowOutputHelper;
        private IUnitTestRuntimeProvider _unitTestRuntimeProvider;
        private ScenarioContext _scenarioContext;
        private static AllureLifecycle allureLifecycle = AllureLifecycle.Instance;

        public AIGatewayStepDefinitions(ISpecFlowOutputHelper specFlowOutputHelper, IUnitTestRuntimeProvider unitTestRuntimeProvider, ScenarioContext scenarioContext)
        {
            _specFlowOutputHelper = specFlowOutputHelper;
            _unitTestRuntimeProvider = unitTestRuntimeProvider;
            _scenarioContext = scenarioContext;

            createAIGateway = new CreateAIGateway(_specFlowOutputHelper);
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            allureLifecycle.CleanupResultDirectory();
        }

        [BeforeScenario]
        public void SetupTestUsers()
        {

        }

        [AfterScenario]
        public void AfterScenario()
        {
            var correlation = GetContentValue<string>(ContextFields.errorDescription);
            var tenantName = GetContentValue<string>(ContextFields.errorDescription);

            if (correlation != null && tenantName != null)
            {
                //deleteMessage.DeleteMessage(correlation, tenantName); (DELETE NAO TEM)
            }
        }

        private CoreExceptionDto? AddNewAIGateway(string modelo, string servicesId, string apiKey)
        {
            var (return_response, messageErrorResponse) = createAIGateway.CreateAIGatewayPost(modelo, servicesId, apiKey);

            CreateAIGatewayResponse return_response_total = JsonConvert.DeserializeObject<CreateAIGatewayResponse>(return_response);
            Usage usage = return_response_total.usage;

            int promptTokens = usage.prompt_tokens;
            int completionTokens = usage.completion_tokens;
            int totalTokens = usage.total_tokens;


            if (totalTokens != 0)
            {
                SetContentValue(ContextFields.promptTokens, promptTokens);
                SetContentValue(ContextFields.completionTokens, completionTokens);
                SetContentValue(ContextFields.totalTokens, totalTokens);
            }

            SetContentValue(ContextFields.errorDescription, messageErrorResponse?.GetErros());

            return messageErrorResponse;
        }

        private void ApplyAIGatewayAssertions(int promptTokens, int completionTokens, int totalTokens)
        {
            var messageErrorResponse = GetContentValue<CoreExceptionDto>(ContextFields.errorDescription);

            messageErrorResponse.Should().BeNull($"Errors: {messageErrorResponse?.GetErros()}");

            if (totalTokens != 0)
            {
                SetContentValue(ContextFields.promptTokens, promptTokens);
                SetContentValue(ContextFields.completionTokens, completionTokens);
                SetContentValue(ContextFields.totalTokens, totalTokens);
                _specFlowOutputHelper.WriteLine($"Esse é o valor do CompletionTokens: {completionTokens}");
            }
        }

        private T GetContentValue<T>(ContextFields key)
        {
            _scenarioContext.TryGetValue(key.ToString(), out T content);
            return content;
        }


        private void SetContentValue(ContextFields key, object value)
        {
            _scenarioContext[key.ToString()] = value;
        }





        [Given(@"que eu tenha os dados servicesId e apiKey")]
        public void GivenQueEuTenhaOsDadosServicesIdEApiKey()
        {
            SetContentValue(ContextFields.servicesId, "b41b68a2-c0b0-4353-8b42-08dbd3c07d35");
            SetContentValue(ContextFields.apiKey, "954bd8c023f6f1d9c9b0e61856b631f67aa46e87c555cbbc886bd1e93b99d9");
        }

        [Given(@"quero criar o modelo ""([^""]*)""")]
        public void GivenQueroCriarOModelo(string modelo)
        {
            SetContentValue(ContextFields.modelo, modelo);
        }

        [When(@"envio a requisicao para o endpoint do AI Gateway")]
        public void WhenEnvioARequisicaoParaOEndpointDoAIGateway()
        {
            string servicesId = GetContentValue<string>(ContextFields.servicesId);
            string apiKey = GetContentValue<string>(ContextFields.apiKey);
            string modelo = GetContentValue<string>(ContextFields.modelo);
            AddNewAIGateway(modelo, servicesId, apiKey);
        }

        [Then(@"entao vejo o status ok e o atributo completion_tokens maior que Zero")]
        public void ThenEntaoVejoOStatusOkEOAtributoCompletion_TokensMaiorQueZero()
        {
            var promptTokens = GetContentValue<int>(ContextFields.promptTokens);
            var completionTokens = GetContentValue<int>(ContextFields.completionTokens);
            var totalTokens = GetContentValue<int>(ContextFields.totalTokens);

            ApplyAIGatewayAssertions(promptTokens, completionTokens, totalTokens);
        }
    }
}
