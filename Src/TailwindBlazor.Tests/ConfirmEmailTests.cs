using System.Net;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using TailwindBlazor.Web.Client.Components.Account;

namespace TailwindBlazor.Tests;

public class ConfirmEmailTests : TestContext
{
    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"success\":true}")
            });
        }
    }

    public ConfirmEmailTests()
    {
        var handler = new FakeHttpMessageHandler();
        Services.AddSingleton(new HttpClient(handler) { BaseAddress = new Uri("http://localhost") });
        Services.AddSingleton<IdentityAuthenticationStateProvider>();
    }

    [Fact]
    public void ConfirmEmail_Shows_Error_When_Params_Missing()
    {
        // Act - sans paramètres userId/code
        var cut = RenderComponent<ConfirmEmail>();

        // Assert
        Assert.Contains("Lien de confirmation invalide", cut.Markup);
    }

    [Fact]
    public void ConfirmEmail_Shows_Success_When_Confirmed()
    {
        // Act - avec userId et code valides
        var cut = RenderComponent<ConfirmEmail>(parameters => parameters
            .Add(p => p.UserId, "user-1")
            .Add(p => p.Code, "valid-code"));

        // Assert - l'état de succès s'affiche
        cut.WaitForAssertion(() =>
            Assert.Contains("Email confirmé", cut.Markup));
    }
}
