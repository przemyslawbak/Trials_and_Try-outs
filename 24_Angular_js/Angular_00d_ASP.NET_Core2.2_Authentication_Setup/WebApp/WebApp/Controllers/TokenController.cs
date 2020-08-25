using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Data.ViewModels;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        IConfiguration _configuration;
        UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public TokenController(IConfiguration configuration, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr)
        {
            _configuration = configuration;
            _signInManager = signinMgr;
            _userManager = userMgr;
        }

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody]TokenRequestViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            switch (model.grant_type)
            {
                case "password":
                    return await GetToken(model);
                default:
                    // not supported - return a HTTP 401 (Unauthorized)
                    return new UnauthorizedResult();
            }
        }
        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            try
            {
                // check if there's an user with the given username
                var user = await _userManager.FindByNameAsync(model.username);
                // fallback to support e-mail address instead of username
            if (user == null && model.username.Contains("@"))
                    user = await _userManager.FindByEmailAsync(model.username);
                if (user == null || !await _userManager.CheckPasswordAsync(user,
                model.password))
                {
                    // user does not exists or password mismatch
                    return new UnauthorizedResult();
                }
                // username & password matches: create and return theJwt token.
                DateTime now = DateTime.UtcNow;
                // add the registered claims for JWT (RFC7519).
                // For more info, see https://tools.ietf.org/html/rfc7519#section-4.1
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
                    // TODO: add additional claims here
                    };
                var tokenExpirationMins = _configuration.GetValue<int> ("Auth:Jwt:TokenExpirationInMinutes");
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Jwt:Key"]));
                var token = new JwtSecurityToken(
                issuer: _configuration["Auth:Jwt:Issuer"],
                audience: _configuration["Auth:Jwt:Audience"],
                claims: claims,
                notBefore: now,
                expires:
                now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                signingCredentials: new SigningCredentials(
                issuerSigningKey,
                SecurityAlgorithms.HmacSha256)
                );
            var encodedToken = new
            JwtSecurityTokenHandler().WriteToken(token);
                // build & return the response
                var response = new TokenResponseViewModel()
                {
                    token = encodedToken,
                    expiration = tokenExpirationMins
                };
                return Json(response);
            }
            catch (Exception ex)
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet("ExternalLogin/{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            switch (provider.ToLower())
            {
                case "facebook":
                    // case "google":
                    // case "twitter":
                    // todo: add all supported providers here
                    // Redirect the request to the external provider.
                    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Token", new { ReturnUrl = returnUrl });
                    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, "http://localhost:53242" + redirectUrl);
                    return new ChallengeResult(provider, properties);
                default:
                    // provider not supported
                    return BadRequest(new
                    {
                        Error = string.Format("Provider '{0}' is not supported.", provider)
                    });
            }
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (!string.IsNullOrEmpty(remoteError))
            {
                // TODO: handle external provider errors
                throw new Exception(string.Format("External Provider error: {0}", remoteError));
            }
            // Extract the login info obtained from the External Provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // if there's none, emit an error
                throw new Exception("ERROR: No login info available.");
            }
            // Check if this user already registered himself with this external provider before
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                // If we reach this point, it means that this user never tried to logged in
                // using this external provider. However, it could have used other providers
                // and /or have a local account.
                // We can find out if that's the case by looking for his e-mail address.
                // Retrieve the 'emailaddress' claim
                var emailKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                var email = info.Principal.FindFirst(emailKey).Value;
                // Lookup if there's an username with this e-mail address in the Db
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // No user has been found: register a new user
                    // using the info retrieved from the provider
                    DateTime now = DateTime.Now;
                    // Create a unique username using the 'nameidentifier' claim
                    var idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                    var username = string.Format("{0}{1}{2}",
                    info.LoginProvider,
                    info.Principal.FindFirst(idKey).Value,
                    Guid.NewGuid().ToString("N"));
                    user = new IdentityUser()
                    {
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = username,
                        Email = email
                    };
                    // Add the user to the Db with a random password
                    IdentityResult register = await _userManager.CreateAsync(user, "SomerandoMp94084as(($_@$sowrdhardTobreak@*#%(!13285038");
                    // Remove Lockout and E-Mail confirmation
                    if (register.Succeeded)
                    {
                        //todo
                    }
                    else
                    {
                        //todo
                    };
                }
                else throw new Exception("Authentication error");
            }

            return Content("finito?"); //brakuje zalogowania jeśli już zarejestrowany, nei zamyka okna facebooka!!!!!!!!!!!!!!!!!!!!
        }

        /*
        [HttpPost("Facebook")]
        public async Task<IActionResult> Facebook([FromBody]ExternalLoginRequestViewModel model)
        {
            try
            {
                var fbAPI_url = "https://graph.facebook.com/v2.10/";
                var fbAPI_queryString = string.Format("me?scope=email&access_token={0}&fields=id,name,email", model.access_token);
                string result = null;
                // fetch the user info from Facebook Graph v2.10
                using (var c = new HttpClient())
                {
                    c.BaseAddress = new Uri(fbAPI_url);
                    var response = await c
                    .GetAsync(fbAPI_queryString);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                    else throw new Exception("Authentication error");
                };
                // load the resulting Json into a dictionary
                var epInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                var info = new UserLoginInfo("facebook", epInfo["id"], "Facebook");
                // Check if this user already registered himself with this external provider before
                var user = await _userManager.FindByLoginAsync(
                info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    // If we reach this point, it means that this user never tried to logged in
                    // using this external provider. However, it could have used other providers
                    // and /or have a local account.
                    // We can find out if that's the case by looking for his eAdvanced mail address.
                    // Lookup if there's an username with this e-mail address in the Db
                    user = await _userManager.FindByEmailAsync(epInfo["email"]);
                    if (user == null)
                    {
                        // No user has been found: register a new user using the info
                        // retrieved from the provider
                        DateTime now = DateTime.Now;
                        var username = String.Format("FB{0}{1}",
                        epInfo["id"],
                        Guid.NewGuid().ToString("N"));
                        user = new IdentityUser()
                        {
                            SecurityStamp = Guid.NewGuid().ToString(),
                            // ensure the user will have an unique username
                            UserName = username,
                            Email = epInfo["email"]
                        };
                        // Add the user to the Db with a random password
                        IdentityResult register = await _userManager.CreateAsync(user, "SomerandoMp94084as(($_@$sowrdhardTobreak@*#%(!13285038");
                        // Remove Lockout and E-Mail confirmation
                        if (register.Succeeded)
                        {
                            //todo
                        }
                        else
                        {
                            //todo
                        }
                    }
                }

                return Json(true);
            }
            catch (Exception ex)
            {
                // return a HTTP Status 400 (Bad Request) to the client
                return BadRequest(new { Error = ex.Message });
            }
        }
        */
    }
}
