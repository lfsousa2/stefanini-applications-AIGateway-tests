using aiGateway.Model;
using Cms.Shared.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Cms.Shared.Extensions
{
    public static class MapperExtensions
    {
        public static CoreExceptionDto ConvertError(this RestResponse restResponse)
        {
            CoreExceptionDto? error;

            try
            {
                error = JsonConvert.DeserializeObject<CoreExceptionDto>(restResponse.Content);

                if (error?.Errors?.Count() == 0)
                {
                    var problemException = JsonConvert.DeserializeObject<ProblemDetailsDto>(restResponse.Content);

                    error = new CoreExceptionDto(problemException?.Status ?? 0, problemException?.Detail);
                }
            }
            catch (Exception)
            {
                error = new CoreExceptionDto();

                var problemException = JsonConvert.DeserializeObject<ProblemDetailsDto>(restResponse.Content);

                if (problemException?.Errors.Any() ?? false)
                {
                    problemException.Errors.Select(x => x).ToList().ForEach(x =>
                    {
                        if (int.TryParse(x.Key, out int status))
                        {
                            error.Errors.Add(new CoreExceptionDto.Error(status, string.Join("-", x.Value)));
                        }
                    });
                }
            }

            return error;
        }
    }
}
