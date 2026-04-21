using AutoMapper;
using CheckListApp.Data;
using CheckListApp.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckListApp.API
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly ProjectEntities _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ProjectEntities appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }

    ////[Route("[controller]/[action]")]
    ////public class AccountController : Controller
    ////{
    ////    [HttpGet]
    ////    public IActionResult SignIn()
    ////    {
    ////        var redirectUrl = Url.Page("/Index");
    ////        return Challenge(
    ////            new AuthenticationProperties { RedirectUri = redirectUrl },
    ////            OpenIdConnectDefaults.AuthenticationScheme
    ////        );
    ////    }

    ////    [HttpGet]
    ////    public IActionResult SignOut()
    ////    {
    ////        var callbackUrl = Url.Page("/Account/SignedOut", pageHandler: null, values: null, protocol: Request.Scheme);
    ////        return SignOut(
    ////            new AuthenticationProperties { RedirectUri = callbackUrl },
    ////            CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme
    ////        );
    ////    }
    ////}
}
