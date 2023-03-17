using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;

namespace UnitTest
{
   public class AccountControllerTests
{
    private readonly AccountController _controller;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountControllerTests()
    {
        var userStore = new Mock<IUserStore<AppUser>>();
        _userManager = new UserManager<AppUser>(userStore.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var logger = new Mock<ILogger<SignInManager<AppUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        _signInManager = new SignInManager<AppUser>(_userManager, contextAccessor.Object, userPrincipalFactory.Object, options.Object, logger.Object, schemes.Object, null);

        _controller = new AccountController(_signInManager, _userManager);
    }

    [Fact]
    public void Create_InvalidModel_ReturnsView()
    {
        // Arrange
        _controller.ModelState.AddModelError("UserName", "UserName is required.");
        var user = new User();

        // Act
        var result = _controller.Create(user).Result;

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<User>(viewResult.ViewData.Model);
        Assert.Equal(user, model);
    }

       

    [Fact]
    public void Login_InvalidModel_ReturnsView()
    {
        // Arrange
        _controller.ModelState.AddModelError("UserName", "UserName is required.");
        var loginVM = new LoginViewModel();


        // Act
        var result = _controller.Login(loginVM).Result;

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
        Assert.Equal(loginVM, model);
    }



        [Fact]
        public async Task Login_InvalidModel_ReturnsViewWithError()
        {
            // Arrange
            var loginVM = new LoginViewModel { UserName = "testuser", Password = "WrongPassword" };
            var user = new User();

            // Act
            var result = await _controller.Login(loginVM) as ViewResult;

            // Assert
            Assert.NotNull(result);
            //Assert.False(_signInManager.IsSignedIn(User));
            Assert.True(_controller.ModelState.ContainsKey(""));
        }

    }

}