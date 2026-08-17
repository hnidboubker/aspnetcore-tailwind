using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TailwindIdentity.Core.Models;
using TailwindIdentity.EntityFrameworkCore.Services;
using TailwindRazorPage.Web.Pages.Account;

namespace Tests.UnitTests.Application.Authentication;

[TestClass]
public class SignUpPageModelTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock = null!;
    private Mock<IEmailService> _emailServiceMock = null!;
    private IConfiguration _configuration = null!;
    private IConfiguration _configurationWithEmailConfirmation = null!;
    private SignUpPageModel _pageModel = null!;
    private Mock<IUrlHelper> _urlHelperMock = null!;
    private DefaultHttpContext _httpContext = null!;

    [TestInitialize]
    public void Setup()
    {
        _userManagerMock = CreateMockUserManager();
        _signInManagerMock = CreateMockSignInManager();
        _emailServiceMock = new Mock<IEmailService>();

        // Create real configuration with test values (email confirmation disabled by default)
        var configurationBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Identity:RequireConfirmedEmail"] = "false"
            });
        _configuration = configurationBuilder.Build();

        // Configuration with email confirmation enabled
        var configurationBuilderWithConfirmation = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Identity:RequireConfirmedEmail"] = "true"
            });
        _configurationWithEmailConfirmation = configurationBuilderWithConfirmation.Build();

        _pageModel = new SignUpPageModel(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object,
            _configuration);

        _httpContext = new DefaultHttpContext();
        _httpContext.Request.Scheme = "https";
        _httpContext.Request.Host = new HostString("localhost");
        _pageModel.PageContext = new PageContext { HttpContext = _httpContext };

        // Create a real UrlHelper with ActionContext for proper Url.Page() support
        var actionContext = new ActionContext(_httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
        var urlHelper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(actionContext);
        _pageModel.Url = urlHelper;

        var tempDataProvider = new Mock<ITempDataProvider>();
        var tempDataDictionary = new TempDataDictionary(_httpContext, tempDataProvider.Object);
        _pageModel.TempData = tempDataDictionary;
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var storeMock = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            storeMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private static Mock<SignInManager<ApplicationUser>> CreateMockSignInManager()
    {
        var userManagerMock = CreateMockUserManager();
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        return new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            contextAccessorMock.Object,
            userPrincipalFactoryMock.Object,
            null!, null!, null!, null!);
    }

    [TestMethod]
    public void OnGet_SetsReturnUrl()
    {
        // Act
        _pageModel.OnGet("/Home");

        // Assert
        Assert.AreEqual("/Home", _pageModel.ReturnUrl);
    }

    [TestMethod]
    public async Task OnPostAsync_ReturnsPage_WhenModelStateInvalid()
    {
        // Arrange
        _pageModel.ModelState.AddModelError("Email", "Required");
        _pageModel.Input = new SignUpPageModel.InputModel
        {
            FirstName = "",
            LastName = "",
            Email = "",
            Password = "",
            ConfirmPassword = ""
        };

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task OnPostAsync_ReturnsPageWithErrors_WhenUserCreationFails()
    {
        // Arrange
        _pageModel.Input = new SignUpPageModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        var identityError = new IdentityError { Description = "Email already exists" };
        var identityResult = IdentityResult.Failed(identityError);

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "password123"))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.IsTrue(_pageModel.ModelState.ContainsKey(string.Empty));
        Assert.AreEqual("Email already exists", _pageModel.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToSendConfirmation_WhenEmailConfirmationRequired()
    {
        // Arrange - use configuration with email confirmation enabled
        var pageModelWithConfirmation = new SignUpPageModel(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object,
            _configurationWithEmailConfirmation);

        // Use the same setup as in TestInitialize for consistency
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost");
        pageModelWithConfirmation.PageContext = new PageContext { HttpContext = httpContext };

        // Create a real UrlHelper with a stub router
        // Url.Page() requires a router to be present in RouteData
        var stubRouter = new StubRouter();
        var routeData = new RouteData();
        routeData.Routers.Add(stubRouter);
        var actionDescriptor = new ActionDescriptor();
        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
        var urlHelper = new UrlHelper(actionContext);
        pageModelWithConfirmation.Url = urlHelper;

        var tempDataProvider = new Mock<ITempDataProvider>();
        var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider.Object);
        pageModelWithConfirmation.TempData = tempDataDictionary;

        pageModelWithConfirmation.Input = new SignUpPageModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "password123"))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync("token123");

        // Act
        var result = await pageModelWithConfirmation.OnPostAsync("/Home");

        // Assert - we only verify the redirect to SendConfirmation happens
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("/Account/SendConfirmation", redirectResult?.PageName);

        // Verify email was sent with the confirmation token
        _emailServiceMock.Verify(x => x.SendAsync(
            "john@test.com",
            "Confirmez votre adresse e-mail",
            It.Is<string>(s => s.Contains("Confirmez votre adresse e-mail"))), Times.Once);
    }

    [TestMethod]
    public async Task OnPostAsync_SignsInAndRedirects_WhenEmailConfirmationNotRequired()
    {
        // Arrange
        _pageModel.Input = new SignUpPageModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "password123"))
            .ReturnsAsync(IdentityResult.Success);

        _signInManagerMock
            .Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null!))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert - since it's a local URL, it should redirect to it
        Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = result as LocalRedirectResult;
        Assert.AreEqual("/Home", redirectResult?.Url);
        _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null!), Times.Once);
        _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToIndex_WhenNoReturnUrlAndEmailConfirmationNotRequired()
    {
        // Arrange
        _pageModel.Input = new SignUpPageModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "password123"))
            .ReturnsAsync(IdentityResult.Success);

        _signInManagerMock
            .Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null!))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _pageModel.OnPostAsync(null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("/Index", redirectResult?.PageName);
    }

    [TestMethod]
    public async Task OnPostAsync_CreatesUserWithCorrectProperties()
    {
        // Arrange
        _pageModel.Input = new SignUpPageModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        ApplicationUser? capturedUser = null;
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "password123"))
            .Callback<ApplicationUser, string>((u, p) => capturedUser = u)
            .ReturnsAsync(IdentityResult.Success);

        _signInManagerMock
            .Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null!))
            .Returns(Task.CompletedTask);

        // Act
        await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsNotNull(capturedUser);
        Assert.AreEqual("john@test.com", capturedUser.UserName);
        Assert.AreEqual("john@test.com", capturedUser.Email);
        Assert.AreEqual("John", capturedUser.FirstName);
        Assert.AreEqual("Doe", capturedUser.LastName);
    }

// Stub router for testing UrlHelper
public class StubRouter : IRouter
{
    public VirtualPathData? GetVirtualPath(VirtualPathContext context)
    {
        return new VirtualPathData(this, context.RouteName ?? "/");
    }

    public Task RouteAsync(RouteContext context)
    {
        return Task.CompletedTask;
    }
}
}