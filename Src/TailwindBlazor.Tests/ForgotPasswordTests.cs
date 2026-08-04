using System.Net;
using System.Net.Http.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using TailwindBlazor.Web.Client.Components.Account;

namespace TailwindBlazor.Tests;

public class ForgotPasswordTests : TestContext
{
    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        public string? LastRequestBody { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequestBody = request.Content?.ReadAsStringAsync(cancellationToken).Result;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    public ForgotPasswordTests()
    {
        var handler = new FakeHttpMessageHandler();
        Services.AddSingleton(new HttpClient(handler) { BaseAddress = new Uri("http://localhost") });
        Services.AddSingleton<IdentityAuthenticationStateProvider>();
    }

    [Fact]
    public void ForgotPassword_Renders_Form()
    {
        // Act
        var cut = RenderComponent<ForgotPassword>();

        // Assert
        Assert.NotNull(cut.Find("form"));
        Assert.NotNull(cut.Find("input[type='email']"));
        Assert.NotNull(cut.Find("button[type='submit']"));
    }

    [Fact]
    public void ForgotPassword_Shows_Confirmation_After_Submit()
    {
        // Arrange
        var cut = RenderComponent<ForgotPassword>();

        // Act
        cut.Find("input[type='email']").Change("user@example.com");
        cut.Find("form").Submit();

        // Assert - le message de confirmation s'affiche après soumission
        cut.WaitForAssertion(() =>
            Assert.Contains("Si un compte existe", cut.Markup));
    }
}
