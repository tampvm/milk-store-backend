using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static MilkStore.Service.Models.ResponseModels.ZaloResponseModel;

namespace MilkStore.Service.Utils
{
    public class ZaloService : IZaloService
    {
        private readonly HttpClient _httpClient;
        private readonly AppConfiguration _configuration;

        public ZaloService(HttpClient httpClient, AppConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ResponseModel> SendVerificationCodeAsync(string phoneNumber)
        {
            string appId = _configuration.Zalo.AppId;
            string appSecret = _configuration.Zalo.AppSecret;
            string accessToken = await GetAccessTokenAsync(appId, appSecret);

            string verificationCode = GenerateVerificationCode();
            var payload = new
            {
                phone = phoneNumber,
                message = $"Your verification code is {verificationCode}"
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://openapi.zalo.me/v2.0/oa/message?access_token={accessToken}", content);

            if (response.IsSuccessStatusCode)
            {
                //SaveVerificationCode(phoneNumber, verificationCode);

                return new SuccessResponseModel<string>
                {
                    Success = true,
                    Message = "Verification code sent successfully.",
                    Data = verificationCode
                };
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "Failed to send verification code."
                };
            }
        }

        private async Task<string> GetAccessTokenAsync(string appId, string appSecret)
        {
            var tokenUrl = "https://oauth.zaloapp.com/v4/access_token"; // Cập nhật URL endpoint
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("app_id", appId),
                new KeyValuePair<string, string>("app_secret", appSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
            {
                Content = requestContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var tokenResponse = JsonConvert.DeserializeObject<ZaloTokenResponse>(content);
                    return tokenResponse.AccessToken;
                }
                catch (JsonReaderException)
                {
                    throw new Exception($"Failed to parse access token response: {content}");
                }
            }
            else
            {
                throw new Exception($"Failed to retrieve access token from Zalo: {content}");
            }
        }

        private class ZaloTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }



        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        //private void SaveVerificationCode(string phoneNumber, string verificationCode)
        //{
        //    var expirationTime = DateTime.UtcNow.AddMinutes(10);
        //    var phoneVerification = new PhoneVerification
        //    {
        //        PhoneNumberOrEmail = phoneNumber,
        //        VerificationCode = verificationCode,
        //        ExpirationTime = expirationTime
        //    };

        //    _context.PhoneVerifications.Add(phoneVerification);
        //    _context.SaveChanges();
        //}

    }
}
