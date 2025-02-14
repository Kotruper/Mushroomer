﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebAssemblyUI.Contracts;
using WebAssemblyUI.Models;

namespace WebAssemblyUI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IConfiguration _config;
    private string bearerTokenStorageKey;

    public AuthenticationService(HttpClient client,
                                 AuthenticationStateProvider authStateProvider,
                                 ILocalStorageService localStorage,
                                 IConfiguration config)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _config = config;
        bearerTokenStorageKey = _config["bearerTokenStorageKey"];
    }

    public async Task Login(UserCredentialsModel userCredentials)
    {
        //var data = MapToUserCredentialsContract(userCredentials);
        var data = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", userCredentials.Email),
            new KeyValuePair<string, string>("password", userCredentials.Password)
            });
        string tokenEndpoint = _config["api"] + _config["tokenEndpoint"];
        
        using var response = await _client.PostAsync(tokenEndpoint, data);
        if (response.IsSuccessStatusCode)
        {
            string access_token = await response.Content.ReadAsStringAsync();

            await _localStorage.SetItemAsync(bearerTokenStorageKey, access_token);
            await ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(access_token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", access_token);
        }
        else
        {
            throw new Exception("Incorrect usernmae or password.");
        }
    }

    private UserCredentials MapToUserCredentialsContract(UserCredentialsModel userCredentials)
        => new UserCredentials(userCredentials.Email, userCredentials.Password, "password");

    public async Task Logout()
    {
        await ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task Register(RegisterModel model)
    {
        var data = new
        {
            model.FirstName,
            model.LastName,
            model.EmailAddress,
            model.Password
        };
        string apiEndpoint = _config["api"] + _config["registerEndpoint"];

        using (HttpResponseMessage response = await _client.PostAsJsonAsync(apiEndpoint, data))
        {
            if (response.IsSuccessStatusCode)
            {
                // Log successful call
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
