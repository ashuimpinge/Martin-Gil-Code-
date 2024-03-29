﻿using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.ComponentModel.Design;

namespace MartinGilDemo.Client.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);

        Task<T> Put<T>(string uri, object value);

        Task<T> Delete<T>(string uri);
    }

    public class HttpService: IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        //  private ILocalStorageService _localStorageService;
        private IConfiguration _configuration;

        // Need to remove 
        private object _localStorageService;

        public HttpService(HttpClient httpClient,NavigationManager navigationManager,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _configuration = configuration;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        // Need to check this method 

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            // add basic auth header if user is logged in and request is to the api url
            // var user = await _localStorageService.GetItem<Person>("user");
            
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;
            //if (user != null && isApiUrl)
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", user.AuthData);
       
            using var response = await _httpClient.SendAsync(request);
       
            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("logout");
                return default;
            }
       
            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
