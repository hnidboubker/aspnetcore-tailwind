using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace TailwindBlazor.Web.Client.Components.Account;

public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public IdentityAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("account/user");

            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

                if (userInfo?.Authenticated == true)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.Email ?? ""),
                        new Claim("firstName", userInfo.FirstName ?? ""),
                        new Claim("lastName", userInfo.LastName ?? "")
                    };

                    var identity = new ClaimsIdentity(claims, "cookie");
                    var principal = new ClaimsPrincipal(identity);
                    return new AuthenticationState(principal);
                }
            }
        }
        catch
        {
            // Ignore errors (e.g., server not running)
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<bool> Login(string email, string password, bool rememberMe)
    {
        var response = await _httpClient.PostAsJsonAsync("account/login",
            new { Email = email, Password = password, RememberMe = rememberMe });
        var result = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (result?.Success == true)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        return result?.Success == true;
    }

    public async Task<(bool Success, string[] Errors)> Register(
        string firstName, string lastName, string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("account/register",
            new { FirstName = firstName, LastName = lastName, Email = email, Password = password });
        var result = await response.Content.ReadFromJsonAsync<RegisterResult>();

        if (result?.Success == true)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        return (result?.Success == true, result?.Errors ?? Array.Empty<string>());
    }

    public async Task Logout()
    {
        await _httpClient.PostAsync("account/logout", null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private class UserInfo
    {
        public bool Authenticated { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }

    private class LoginResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    private class RegisterResult
    {
        public bool Success { get; set; }
        public string[]? Errors { get; set; }
    }
}
