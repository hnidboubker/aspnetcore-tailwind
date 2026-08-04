using Bunit;
using Xunit;
using TailwindBlazor.Web.Client.Components.Account;

namespace TailwindBlazor.Tests;

public class LoginTests : TestContext
{
    public LoginTests()
    {
        // Register required services for Account components
        Services.AddSingleton(new HttpClient { BaseAddress = new Uri("http://localhost") });
        Services.AddSingleton<IdentityAuthenticationStateProvider>();
    }

    [Fact]
    public void Login_Renders_Form()
    {
        // Act
        var cut = RenderComponent<Login>();

        // Assert
        Assert.NotNull(cut.Find("form"));
        Assert.NotNull(cut.Find("input[type='email']"));
        Assert.NotNull(cut.Find("input[type='password']"));
    }

    [Fact]
    public void Login_Has_RememberMe_Checkbox()
    {
        // Act
        var cut = RenderComponent<Login>();

        // Assert
        Assert.NotNull(cut.Find("input[type='checkbox']"));
    }

    [Fact]
    public void Login_Has_Submit_Button()
    {
        // Act
        var cut = RenderComponent<Login>();

        // Assert
        Assert.NotNull(cut.Find("button[type='submit']"));
    }
}
