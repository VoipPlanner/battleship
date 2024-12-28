using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Battleship.Integration.Tests
{
    public class ApiResult
    {
        public dynamic Response { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }

    public static class HelperExtensions
    {
        public static  async Task<ApiResult> ParseResponse( this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;
            var expConverter = new ExpandoObjectConverter();

            dynamic responseObject;
            try
            {
                responseObject = JsonConvert.DeserializeObject<ExpandoObject>(content, expConverter);
            }
            catch (Exception)
            {
                responseObject = content;
            }

            return new ApiResult
            {
                Response = responseObject,
                HttpStatusCode = status
            };
        }
    }
}