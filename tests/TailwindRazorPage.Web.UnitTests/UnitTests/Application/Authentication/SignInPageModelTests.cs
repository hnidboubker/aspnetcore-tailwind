using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TailwindIdentity.Core.Models;
using TailwindRazorPage.Web.Pages.Account;

namespace Tests.UnitTests.Application.Authentication;

[TestClass]
public class SignInPageModelTests
{
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock = null!;
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
    private SignInPageModel _pageModel = null!;
    private Mock<IUrlHelper> _urlHelperMock = null!;

    [TestInitialize]
    public void Setup()
    {
        _signInManagerMock = CreateMockSignInManager();
        _userManagerMock = CreateMockUserManager();

        _pageModel = new SignInPageModel(_signInManagerMock.Object, _userManagerMock.Object);

        _urlHelperMock = new Mock<IUrlHelper>();
        _pageModel.Url = _urlHelperMock.Object;

        var tempDataProvider = new Mock<ITempDataProvider>();
        var tempDataDictionary = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
        _pageModel.TempData = tempDataDictionary;
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

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var storeMock = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            storeMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    [TestMethod]
    public void OnGet_ReturnsPage_WhenUserNotAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity());
        _pageModel.PageContext = new PageContext { HttpContext = httpContext };

        // Act
        var result = _pageModel.OnGet("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.AreEqual("/Home", _pageModel.ReturnUrl);
    }

    [TestMethod]
    public void OnGet_RedirectsToReturnUrl_WhenUserAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var identity = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "testuser")
        }, "TestAuth");
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(identity);
        _pageModel.PageContext = new PageContext { HttpContext = httpContext };

        // Act
        var result = _pageModel.OnGet("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = result as LocalRedirectResult;
        Assert.AreEqual("/Home", redirectResult?.Url);
    }

    [TestMethod]
    public async Task OnPostAsync_ReturnsPage_WhenModelStateInvalid()
    {
        // Arrange
        _pageModel.ModelState.AddModelError("UserNameOrEmail", "Required");
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "",
            Password = "",
            RememberMe = false
        };

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
    }

    [TestMethod]
    public async Task OnPostAsync_ReturnsPageWithError_WhenUserNotFound()
    {
        // Arrange
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "nonexistent@test.com",
            Password = "password123",
            RememberMe = false
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("nonexistent@test.com"))
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.IsTrue(_pageModel.ModelState.ContainsKey(string.Empty));
        Assert.AreEqual("Nom d'utilisateur/e-mail ou mot de passe invalide.", _pageModel.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToIndex_WhenSignInSucceeds()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "test@test.com",
            Password = "password123",
            RememberMe = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "password123", true, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("/Index", redirectResult?.PageName);
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToReturnUrl_WhenSignInSucceedsAndReturnUrlIsLocal()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "test@test.com",
            Password = "password123",
            RememberMe = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "password123", true, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _urlHelperMock.Setup(x => x.IsLocalUrl("/Home")).Returns(true);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
        var redirectResult = result as LocalRedirectResult;
        Assert.AreEqual("/Home", redirectResult?.Url);
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToLoginWith2fa_WhenTwoFactorRequired()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "test@test.com",
            Password = "password123",
            RememberMe = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "password123", true, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("./LoginWith2fa", redirectResult?.PageName);
        Assert.IsNotNull(redirectResult?.RouteValues["ReturnUrl"]);
        Assert.AreEqual("/Home", redirectResult?.RouteValues["ReturnUrl"]);
        Assert.IsTrue((bool)(redirectResult?.RouteValues["RememberMe"] ?? false));
    }

    [TestMethod]
    public async Task OnPostAsync_RedirectsToLockout_WhenLockedOut()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "test@test.com",
            Password = "password123",
            RememberMe = false
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "password123", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("./Lockout", redirectResult?.PageName);
    }

    [TestMethod]
    public async Task OnPostAsync_ReturnsPageWithError_WhenSignInFails()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "test@test.com",
            Password = "wrongpassword",
            RememberMe = false
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "wrongpassword", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(PageResult));
        Assert.IsTrue(_pageModel.ModelState.ContainsKey(string.Empty));
        Assert.AreEqual("Nom d'utilisateur/e-mail ou mot de passe invalide.", _pageModel.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task OnPostAsync_FindsUserByUserName_WhenNoEmailFormat()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@test.com" };
        _pageModel.Input = new SignInPageModel.InputModel
        {
            UserNameOrEmail = "testuser",
            Password = "password123",
            RememberMe = false
        };

        _userManagerMock
            .Setup(x => x.FindByNameAsync("testuser"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.PasswordSignInAsync(user, "password123", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _pageModel.OnPostAsync("/Home");

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        _userManagerMock.Verify(x => x.FindByNameAsync("testuser"), Times.Once);
        _userManagerMock.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task OnPostLogoutAsync_SignsOutAndRedirectsToIndex()
    {
        // Arrange
        _signInManagerMock
            .Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _pageModel.OnPostLogoutAsync();

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        var redirectResult = result as RedirectToPageResult;
        Assert.AreEqual("/Index", redirectResult?.PageName);
        _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
    }
}