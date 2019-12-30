using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace HttpRequestTwoAccess
{
    public static class HttpRequestTwoAccess
    {
        [FunctionName("HttpRequestTwoAccess")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req
        )
        {
            // success pattern
            var userRequest = new User();
            using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                userRequest = JsonConvert.DeserializeObject<User>(await sr.ReadToEndAsync());
            }
            req.Body.Seek(0, SeekOrigin.Begin);

            var homeRequest = new Home();
            using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                homeRequest = JsonConvert.DeserializeObject<Home>(await sr.ReadToEndAsync());
            }
            req.Body.Seek(0, SeekOrigin.Begin);

            // error pattern
            // error content: System.Private.CoreLib: Exception while executing function: HttpRequestTwoAccess. Microsoft.AspNetCore.WebUtilities: Cannot access a disposed object.
            // error content: Object name: 'FileBufferingReadStream'
            //var userRequest = new User();
            //using (var sr = new StreamReader(req.Body))
            //{
            //    userRequest = JsonConvert.DeserializeObject<User>(await sr.ReadToEndAsync());
            //}

            //var homeRequest = new Home();
            //using (var sr = new StreamReader(req.Body))
            //{
            //    homeRequest = JsonConvert.DeserializeObject<Home>(await sr.ReadToEndAsync());
            //}

            var response = new Response()
            {
                Name = userRequest.Name,
                Age = userRequest.Age,
                PhoneNumber = userRequest.PhoneNumber,
                Address = homeRequest.Address,
                ZipCode = homeRequest.ZipCode,
            };


            return new OkObjectResult(response);
        }

        public class Response {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "age")]
            public int Age { get; set; }

            [JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }

            [JsonProperty(PropertyName = "address")]
            public string Address { get; set; }

            [JsonProperty(PropertyName = "zip_code")]
            public string ZipCode { get; set; }
        }
    }
}
