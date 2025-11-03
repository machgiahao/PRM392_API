using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _baseUrl = "https://api-m.sandbox.paypal.com";

        public PayPalService(IConfiguration config)
        {
            _clientId = config["PayPal:ClientId"];
            _secret = config["PayPal:ClientSecret"];
            _httpClient = new HttpClient();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            var data = new Dictionary<string, string> { { "grant_type", "client_credentials" } };
            var response = await _httpClient.PostAsync($"{_baseUrl}/v1/oauth2/token", new FormUrlEncodedContent(data));
            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<string> CreateOrderAsync(decimal amount)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var order = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                new { amount = new { currency_code = "USD", value = amount.ToString("F2") } }
            },
                application_context = new
                {
                    return_url = "myapp://paypal/success",
                    cancel_url = "myapp://paypal/cancel"
                }
            };

            var json = JsonSerializer.Serialize(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/v2/checkout/orders", content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            return doc.RootElement.GetProperty("links").EnumerateArray()
                .First(x => x.GetProperty("rel").GetString() == "approve")
                .GetProperty("href").GetString();
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync($"{_baseUrl}/v2/checkout/orders/{orderId}/capture", null);
            return response.IsSuccessStatusCode;
        }
    }
}
