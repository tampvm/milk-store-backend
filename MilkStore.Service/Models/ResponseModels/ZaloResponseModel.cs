using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ResponseModels
{
    public class ZaloResponseModel
    {
        public class ZaloMessageRequest
        {
            public string PhoneNumber { get; set; }
            public string TemplateId { get; set; }
            public Dictionary<string, string> Params { get; set; }
        }

        public class ZaloMessageResponse
        {
            public int ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class ZaloAccessTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }

        
    }
}
