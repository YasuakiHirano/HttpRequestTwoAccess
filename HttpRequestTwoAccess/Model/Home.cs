using System;
using Newtonsoft.Json;

namespace HttpRequestTwoAccess
{
    public class Home
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "zip_code")]
        public string ZipCode { get; set; }
    }
}
